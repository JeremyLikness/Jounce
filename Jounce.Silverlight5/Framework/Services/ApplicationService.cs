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
using Jounce.Framework.View;

namespace Jounce.Framework.Services
{
    /// <summary>
    ///     Main application service
    /// </summary>   
    /// <remarks>
    /// This is placed in the main <value>App.xaml</value> file 
    /// </remarks>     
    public class ApplicationService : IApplicationService, IApplicationLifetimeAware, IDisposable
    {
        private bool _disposed;

        /// <summary>
        ///     Main aggregate catalog used by the system
        /// </summary>
        private AggregateCatalog _mainCatalog;

        /// <summary>
        ///     Container
        /// </summary>
        private CompositionContainer _container;

        /// <summary>
        ///     Mef debugger
        /// </summary>
        private MefDebugger _mefDebugger;

        /// <summary>
        /// Deployment service reference to <see cref="IDeploymentService"/>
        /// </summary>
        [Import]
        public IDeploymentService DeploymentService { get; set; }

        /// <summary>
        /// Reference to the <see cref="IEventAggregator"/>
        /// </summary>
        [Import]        
        public IEventAggregator EventAggregator { get; set; }

        /// <summary>
        /// Reference to the main instance of the <see cref="ViewRouter"/>
        /// </summary>
        [Import]
        public ViewRouter Router { get; set; }
        
        /// <summary>
        /// Holds a list of all views and their <see cref="IExportAsViewMetadata"/>
        /// </summary>
        [ImportMany(AllowRecomposition = true)]
        public Lazy<UserControl, IExportAsViewMetadata>[] Views { get; set; }

        /// <summary>
        /// The reference to <see cref="ILogger"/>
        /// </summary>
        [Import(AllowDefault = true, AllowRecomposition = true)]
        public ILogger Logger { get; set; }

        /// <summary>
        /// Set to true to suppress Jounce from intercepting unhandled exceptions
        /// </summary>
        public bool IgnoreUnhandledExceptions { get; set; }

        private LogSeverity _severity = LogSeverity.Warning;

        /// <summary>
        /// Sets the initial severity level for the log.         
        /// </summary>
        /// <remarks>
        /// Overridden if this is set in the init parameters
        /// </remarks>        
        public LogSeverity LogSeverityLevel
        {
            get { return _severity; }
            set { _severity = value; }
        }

        /// <summary>
        /// Called by an application in order to initialize the application extension service.
        /// </summary>
        /// <param name="context">Provides information about the application state. </param>
        public void StartService(ApplicationServiceContext context)
        {
            var logLevel = LogSeverityLevel;

            if (context.ApplicationInitParams.ContainsKey(Constants.INIT_PARAM_LOGLEVEL))
            {
                logLevel =
                    (LogSeverity)
                    Enum.Parse(typeof (LogSeverity), context.ApplicationInitParams[Constants.INIT_PARAM_LOGLEVEL], true);
            }

            _mainCatalog = new AggregateCatalog(new DeploymentCatalog()); // empty one adds current deployment (xap)

            _container = new CompositionContainer(_mainCatalog);
            
            CompositionHost.Initialize(_container);
            CompositionInitializer.SatisfyImports(this);

            if (Logger == null)
            {
                ILogger defaultLogger = new DefaultLogger(logLevel);
                _container.ComposeExportedValue(defaultLogger);
            }
            else
            {
                Logger.SetSeverity(logLevel);
            }

            DeploymentService.Catalog = _mainCatalog;
            DeploymentService.Container = _container;
            _mefDebugger = new MefDebugger(_container, Logger);
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
            if (!IgnoreUnhandledExceptions)
            {
                Application.Current.UnhandledException += _CurrentUnhandledException;
            }

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
        void _CurrentUnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // we didn't come back from a broadcast
            if (_unhandled)
            {
                return;
            }

            _unhandled = true;

            var exception = new UnhandledExceptionEvent
                                {
                                    Handled = e.Handled,
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

        /// <summary>
        ///  When disposed - likely will never get called
        /// </summary>
        /// <param name="disposing">True when disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (!disposing) return;

            if (_mainCatalog != null)
            {
                _mainCatalog.Dispose();
                _mainCatalog = null;
            }

            if (_container != null)
            {
                _container.Dispose();
                _container = null;
            }

            _mefDebugger = null;

            _disposed = true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}