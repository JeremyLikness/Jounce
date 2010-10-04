using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Jounce.Core;
using Jounce.Core.Application;
using Jounce.Core.Event;
using Jounce.Core.View;
using Jounce.Framework.Views;

namespace Jounce.Framework.Services
{
    /// <summary>
    ///     Main application service
    /// </summary>        
    public class ApplicationService : IApplicationService, IApplicationLifetimeAware
    {
        /// <summary>
        ///     Main aggregate catalog used by the system
        /// </summary>
        private AggregateCatalog _mainCatalog;

        /// <summary>
        ///     Mef debugger
        /// </summary>
        private MefDebugger _mefDebugger;

        /// <summary>
        ///     Deployment service
        /// </summary>
        [Import]
        public IDeploymentService DeploymentService { get; set; }

        [Import]
        public IEventAggregator EventAggregator { get; set; }

        [Import]
        public ViewRouter Router { get; set; }
        
        [ImportMany(AllowRecomposition = true)]
        public Lazy<UserControl, IExportAsViewMetadata>[] Views { get; set; }

        /// <summary>
        ///     The logger
        /// </summary>
        [Import(AllowDefault = true, AllowRecomposition = true)]
        public ILogger Logger { get; set; }

        /// <summary>
        /// Called by an application in order to initialize the application extension service.
        /// </summary>
        /// <param name="context">Provides information about the application state. </param>
        public void StartService(ApplicationServiceContext context)
        {
            var logLevel = LogSeverity.Warning;

            if (context.ApplicationInitParams.ContainsKey(Constants.INIT_PARAM_LOGLEVEL))
            {
                logLevel =
                    (LogSeverity)
                    Enum.Parse(typeof (LogSeverity), context.ApplicationInitParams[Constants.INIT_PARAM_LOGLEVEL], true);
            }

            _mainCatalog = new AggregateCatalog(new DeploymentCatalog()); // empty one adds current deployment (xap)

            var container = new CompositionContainer(_mainCatalog);

            CompositionHost.Initialize(container);
            CompositionInitializer.SatisfyImports(this);

            if (Logger == null)
            {
                ILogger defaultLogger = new DefaultLogger(logLevel);
                container.ComposeExportedValue(defaultLogger);
            }

            DeploymentService.Catalog = _mainCatalog;
            _mefDebugger = new MefDebugger(container, Logger);
        }

        /// <summary>
        /// Called by an application in order to stop the application extension service. 
        /// </summary>
        public void StopService()
        {
            Logger.Log(LogSeverity.Information, GetType().FullName, MethodBase.GetCurrentMethod().Name);
            _mefDebugger.Close();
        }

        /// <summary>
        /// Called by an application immediately before the <see cref="E:System.Windows.Application.Startup"/> event occurs.
        /// </summary>
        public void Starting()
        {
            Application.Current.UnhandledException += Current_UnhandledException;

            var viewInfo = (from v in Views where v.Metadata.IsShell select v).FirstOrDefault();

            if (viewInfo == null)
            {
                var grid = new Grid();
                var tb = new TextBlock
                             {
                                 Text = Resources.ApplicationService_Starting_Jounce_Error_No_view
                             };
                grid.Children.Add(tb);
                Application.Current.RootVisual = grid;
                Logger.Log(LogSeverity.Critical, GetType().FullName,  Resources.ApplicationService_Starting_Jounce_Error_No_view);
                return;
            }

            Application.Current.RootVisual = viewInfo.Value;
            Logger.LogFormat(LogSeverity.Information, GetType().FullName,
                             Resources.ApplicationService_Starting_ShellResolved, MethodBase.GetCurrentMethod().Name,
                             viewInfo.Value.GetType().FullName);
            Logger.Log(LogSeverity.Information, GetType().FullName, MethodBase.GetCurrentMethod().Name);            

            EventAggregator.Publish(viewInfo.Metadata.ExportedViewType.AsViewNavigationArgs());
        }

        private bool _unhandled;

        /// <summary>
        ///     Broadcast the exception
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Current_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // we didn't come back from a broadcast
            if (_unhandled)
            {
                return;
            }

            _unhandled = true;

            var exception = new UnhandledExceptionEvent
                                {
                                    Handled = false,
                                    UncaughtException = e.ExceptionObject
                                };
            EventAggregator.Publish(exception);
            e.Handled = exception.Handled;

            _unhandled = false;
        }

        /// <summary>
        /// Called by an application immediately after the <see cref="E:System.Windows.Application.Startup"/> event occurs.
        /// </summary>
        public void Started()
        {
            Logger.Log(LogSeverity.Information, GetType().FullName, MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// Called by an application immediately before the <see cref="E:System.Windows.Application.Exit"/> event occurs. 
        /// </summary>
        public void Exiting()
        {
            Logger.Log(LogSeverity.Information, GetType().FullName, MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// Called by an application immediately after the <see cref="E:System.Windows.Application.Exit"/> event occurs. 
        /// </summary>
        public void Exited()
        {
            Logger.Log(LogSeverity.Information, GetType().FullName, MethodBase.GetCurrentMethod().Name);
        }
    }
}