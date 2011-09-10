using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;
using Jounce.Core;
using Jounce.Core.Application;
using Jounce.Core.Event;
using Jounce.Core.Workflow;
using Jounce.Framework.Exceptions;
using Jounce.Framework.Workflow;

namespace Jounce.Framework.Services
{
    /// <summary>
    ///     Implementation of the deployment service
    /// </summary>
    [Export(typeof(IDeploymentService))]
    public class DeploymentService : IDeploymentService, IPartImportsSatisfiedNotification
    {
        /// <summary>
        /// Flag indicates if it has been initialized yet or not
        /// </summary>
        private bool _init;

        /// <summary>
        ///     List of Xap files already loaded
        /// </summary>
        private readonly List<Uri> _loaded = new List<Uri>();
        
        /// <summary>
        /// List of <see cref="IModuleInitializer"/> instances brought in by
        /// importing dynamic XAP files
        /// </summary>
        [ImportMany(AllowRecomposition = true)]
        public IModuleInitializer[] Modules { get; set; }

        /// <summary>
        /// Event aggregator reference to <see cref="IEventAggregator"/>
        /// </summary>  
        [Import]     
        public IEventAggregator EventAggregator { get; set; }

        /// <summary>
        /// The main <see cref="CompositionContainer"/>
        /// </summary>
        public CompositionContainer Container { get; set; }
        
        /// <summary>
        /// Instance of the <see cref="ILogger"/>
        /// </summary>      
        [Import(AllowDefault = true, AllowRecomposition = true)]
        public ILogger Logger { get; set; }
                
        /// <summary>
        /// The main <see cref="AggregateCatalog"/>
        /// </summary>
        public AggregateCatalog Catalog { get; set; }                       

        /// <summary>
        ///  Request the xap
        /// </summary>
        /// <param name="xapName">The name of the xap</param>
        public void RequestXap(string xapName)
        {
            RequestXap(xapName, null);
        }       

        /// <summary>
        /// Request the xap with notification when finished
        /// </summary>
        /// <param name="xapName">The name of the XAP</param>
        /// <param name="xapLoaded">Callback when complete</param>
        public void RequestXap(string xapName, Action<Exception> xapLoaded)
        {
            RequestXap(xapName, xapLoaded, null);
        }

        /// <summary>
        ///     Request a xap to be downloaded and integrated
        /// </summary>
        /// <param name="xapName">The name of the XAP to download</param>
        /// <param name="xapLoaded">Callback once xap is loaded</param>
        /// <param name="xapProgress">Callback for xap progress with bytes received, pct, and total bytes</param>
        public void RequestXap(string xapName, Action<Exception> xapLoaded,
            Action<long,int,long> xapProgress)
        {
            if (string.IsNullOrEmpty(xapName))
            {
                throw new ArgumentNullException("xapName");
            }

            var uri = new Uri(Application.Current.Host.Source, xapName);
            if (!uri.GetComponents(UriComponents.Path, UriFormat.Unescaped).EndsWith(".xap", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ArgumentOutOfRangeException("xapName", Resources.DeploymentService_RequestXap_XAPExtensionError);
            }   
       
            WorkflowController.Begin(DownloadWorkflow(xapName, xapLoaded, xapProgress));
        }

        /// <summary>
        /// An internal workflow to facilitate downloading the XAP file
        /// </summary>
        /// <param name="xapName">The name of the XAP</param>
        /// <param name="xapLoaded">The action to call once it is loaded</param>
        /// <param name="xapProgress">Action to call to report download progress</param>
        /// <returns>A list of <see cref="IWorkflow"/> items to execute</returns>
        private IEnumerable<IWorkflow> DownloadWorkflow(
            string xapName, 
            Action<Exception> xapLoaded,
            Action<long, int, long> xapProgress)
        {
            var xap = xapName.Trim().ToLower();            

            Logger.LogFormat(LogSeverity.Verbose, GetType().FullName, "{0}::{1}", MethodBase.GetCurrentMethod().Name, xapName);

            var uri = new Uri(xap, UriKind.Relative);

            if (_loaded.Contains(uri))
            {
                if (xapLoaded != null)
                {
                    xapLoaded(null);
                }
                yield break;
            }            

            var deploymentCatalog = new DeploymentCatalog(uri);

            EventHandler<DownloadProgressChangedEventArgs> eventHandler = null;

            if (xapProgress != null)
            {
                eventHandler = (o, args) => xapProgress(
                    args.BytesReceived,
                    args.ProgressPercentage,
                    args.TotalBytesToReceive);
                deploymentCatalog.DownloadProgressChanged += eventHandler;
            }            

            var downloadAction = new WorkflowEvent<AsyncCompletedEventArgs>(deploymentCatalog.DownloadAsync,
                                                   h => deploymentCatalog.DownloadCompleted += h,
                                                   h => deploymentCatalog.DownloadCompleted -= h);

            Catalog.Catalogs.Add(deploymentCatalog);
            
            EventAggregator.Publish(Constants.BEGIN_BUSY);

            yield return downloadAction;

            if (xapProgress != null)
            {
                deploymentCatalog.DownloadProgressChanged -= eventHandler;
            }

            InitModules();

            EventAggregator.Publish(Constants.END_BUSY);
            
            Logger.LogFormat(LogSeverity.Verbose, GetType().FullName, "{0}::{1}", MethodBase.GetCurrentMethod().Name, deploymentCatalog.Uri);

            var e = downloadAction.Result;

            if (e.Error != null)
            {
                var exception = new DeploymentCatalogDownloadException(e.Error);

                Logger.Log(LogSeverity.Critical, string.Format("{0}::{1}", GetType().FullName,
                MethodBase.GetCurrentMethod().Name), exception);

                if (xapLoaded == null)
                {
                    throw exception;
                }
            }
            else
            {
                _loaded.Add(deploymentCatalog.Uri);
                Logger.LogFormat(LogSeverity.Verbose, string.Format("{0}::{1}", GetType().FullName,
                                                                    MethodBase.GetCurrentMethod().Name),
                                 Resources.DeploymentService_DeploymentCatalogDownloadCompleted_Finished, deploymentCatalog.Uri);                

                if (xapLoaded != null)
                {
                    xapLoaded(null);
                }
            }
        }       

        /// <summary>
        ///  Initialize modules
        /// </summary>
        /// <remarks>
        /// Fires any time a new module is loaded
        /// </remarks>
        private void InitModules()
        {
            foreach(var moduleInitializer in from m in Modules where !m.Initialized select m)
            {
                moduleInitializer.Initialize();
            }
        }

        /// <summary>
        /// Called when a part's imports have been satisfied and it is safe to use.
        /// </summary>
        public void OnImportsSatisfied()
        {
            if (_init) return;

            _init = true;
            InitModules();
        }
    }
}