using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
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
        ///     Request a xap to be downloaded and integrated
        /// </summary>
        /// <param name="xapName">The name of the XAP to download</param>
        /// <param name="xapLoaded">Callback once xap is loaded</param>
        public void RequestXap(string xapName, Action<Exception> xapLoaded)
        {
            if (string.IsNullOrEmpty(xapName))
            {
                throw new ArgumentNullException("xapName");
            }

            if (!xapName.EndsWith(".xap", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ArgumentOutOfRangeException("xapName", Resources.DeploymentService_RequestXap_XAPExtensionError);
            }   
       
            WorkflowController.Begin(DownloadWorkflow(xapName, xapLoaded));
        }

        /// <summary>
        /// An internal workflow to facilitate downloading the XAP file
        /// </summary>
        /// <param name="xapName">The name of the XAP</param>
        /// <param name="xapLoaded">The action to call once it is loaded</param>
        /// <returns>A list of <see cref="IWorkflow"/> items to execute</returns>
        private IEnumerable<IWorkflow> DownloadWorkflow(string xapName, Action<Exception> xapLoaded)
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

            var downloadAction = new WorkflowEvent<AsyncCompletedEventArgs>(deploymentCatalog.DownloadAsync,
                                                   h => deploymentCatalog.DownloadCompleted += h,
                                                   h => deploymentCatalog.DownloadCompleted -= h);
            Catalog.Catalogs.Add(deploymentCatalog);
            
            EventAggregator.Publish(Constants.BEGIN_BUSY);

            yield return downloadAction;

            _InitModules();

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
        private void _InitModules()
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
            if (!_init)
            {
                _init = true;
                _InitModules();
            }
        }
    }
}