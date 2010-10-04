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
    public class DeploymentService : IDeploymentService
    {        
        /// <summary>
        ///     List of Xap files already loaded
        /// </summary>
        private readonly List<Uri> _loaded = new List<Uri>();
        
        [ImportMany(AllowRecomposition = true)]
        public IModuleInitializer[] Modules { get; set; }

        /// <summary>
        ///     Event aggregator
        /// </summary>  
        [Import]     
        public IEventAggregator EventAggregator { get; set; }

        /// <summary>
        ///     Logger
        /// </summary>      
        [Import(AllowDefault = true, AllowRecomposition = true)]
        public ILogger Logger { get; set; }
                
        /// <summary>
        ///     The main catalog
        /// </summary>
        public AggregateCatalog Catalog { get; set; }                       

        /// <summary>
        ///     Request the xap
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

            foreach(var moduleInitializer in from m in Modules where !m.Initialized select m)
            {
                moduleInitializer.Initialize();
            }

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
    }
}