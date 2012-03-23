using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Automation;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Jounce.Core.Event;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework;
using Jounce.Framework.Command;
using Microsoft.Expression.Interactivity.Core;
using ToDoList.Behaviors;
using ToDoList.Contracts;
using ToDoList.Model;
using ToDoList.Repository;

namespace ToDoList.ViewModels
{
    [ExportAsViewModel(typeof(MainViewModel))]
    public class MainViewModel : BaseViewModel, IEventSink<UnhandledExceptionEvent>, 
        IEventSink<MessageNewToDoItem>, IEventSink<MessageToDoItemComplete>
    {
        public MainViewModel()
        {
            if (InDesigner)
            {
                InstallText = "Install";
                InstallCommand = new ActionCommand(obj => { });
                PrintCommand = new ActionCommand(()=>{ });
                UserName = "Test User";
            }

            PrintCommand = new ActionCommand(()=>Report.ProcessReport());
        }

        [Import]
        public Global Globals { get; set; }        

        [Import]
        public IToDoListApplicationContext AppContext { get; set; }

        [Import]
        public INotification Notification { get; set; }

        [Import]
        public Lazy<WindowController> Controller { get; set; }

        [Import]
        public IExportAs Export { get; set; }

        [Import]
        public WcfRiaRepository Synchronization { get; set; }

        [Import]
        public ITaskGridReport Report { get; set; }

        public ICommand PrintCommand { get; private set; }

        public ICommand NewCommand { get; private set; }

        public ICommand InstallCommand { get; private set; }

        public ICommand ExportCommand { get; private set; }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                RaisePropertyChanged(() => UserName);
            }
        }

        public bool OutOfBrowser
        {
            get
            {
                return AppContext != null && AppContext.IsRunningOutOfBrowser;
            }
        }

        private string _installText;
        public string InstallText
        {
            get { return _installText; }
            set
            {
                _installText = value;
                RaisePropertyChanged(() => InstallText);
            }
        }

        protected override void ActivateView(string viewName, 
            System.Collections.Generic.IDictionary<string, object> 
            viewParameters)
        {
            if (!AppContext.IsInstalled || AppContext.IsRunningOutOfBrowser)
            {
                return;
            }

            OverlayHelper.Show();
            EventAggregator.Publish(Globals.OobView.AsViewNavigationArgs());
        }

        protected override void InitializeVm()
        {
            NewCommand = new ActionCommand(obj=>MakeNew());

            InstallCommand = new ActionCommand(obj=>Install());
            RaisePropertyChanged(() => InstallCommand);

            ExportCommand = new ActionCommand<object>(obj=>DoExport(), 
                obj=>AppContext.IsRunningOutOfBrowser);
            RaisePropertyChanged(() => OutOfBrowser);
            RaisePropertyChanged(() => ExportCommand);

            SetInstallText();
            
            EventAggregator.SubscribeOnDispatcher<UnhandledExceptionEvent>(this);

            if (AppContext.IsRunningOutOfBrowser)
            {
                EventAggregator.SubscribeOnDispatcher<MessageNewToDoItem>(this);
                EventAggregator.SubscribeOnDispatcher<MessageToDoItemComplete>(this);

                // need this to lazy load the controller to start listening for events
                var controller = Controller.Value;                

                var log = Globals.EventLogView.AsViewNavigationArgs()
                    .AsChildWindow()
                    .WindowTitle("To-Do Event Log")
                    .WindowWidth(600)
                    .WindowHeight(300);                    

                EventAggregator.Publish(log);
            }

            EventAggregator.Publish(Globals.StatusView.AsViewNavigationArgs());
            EventAggregator.Publish(Globals.ToDoListView.AsViewNavigationArgs());
            EventAggregator.Publish(Globals.QuickAddView.AsViewNavigationArgs());
            EventAggregator.Publish(Globals.FilterView.AsViewNavigationArgs());
            EventAggregator.Publish(Globals.TextFilterView.AsViewNavigationArgs());
            EventAggregator.Publish(Globals.SortView.AsViewNavigationArgs());
            EventAggregator.Publish(Globals.EditView.AsViewNavigationArgs());
            EventAggregator.Publish(new ViewNavigationArgs(Globals.EditView)
                                        .AddNamedParameter("State", "Closed"));
            
            base.InitializeVm();
            AppContext.Title = Global.Constants.MAIN_TITLE;

            if (AppContext.IsRunningOutOfBrowser)
            {
                Application.Current.CheckAndDownloadUpdateCompleted +=
                    CurrentCheckAndDownloadUpdateCompleted;
                Application.Current.CheckAndDownloadUpdateAsync();
                var size = 64;
                var sb = new StringBuilder(64);
                if (GetUserName(sb, ref size))
                {
                    UserName = sb.ToString();
                }
            }
        }

        [DllImport("Advapi32.dll")]
        private static extern bool GetUserName(StringBuilder lpBuffer, ref int nSize);

        private void DoExport()
        {
            var vm = Router.ResolveViewModel(Globals.ToDoListViewModel)
                                    as IToDoListViewModel;
            if (vm == null)
            {
                return;
            }

            var tasks = vm.Tasks;
            Export.Export(tasks);
        }

        void CurrentCheckAndDownloadUpdateCompleted(object sender, 
            CheckAndDownloadUpdateCompletedEventArgs e)
        {
            if (e.UpdateAvailable)
            {
                OverlayHelper.Show();
                EventAggregator.Publish(
                    Globals.UpdateView.AsViewNavigationArgs());
            }
        }

        private void SetInstallText()
        {
            InstallText = AppContext.IsInstalled
                                ? "Remove"
                                : "Install";
        }

        private const string LAUNCHER = @"c:\{0}\Microsoft Silverlight\sllauncher.exe";
        private const string PATH64 = @"program files (x86)";
        private const string PATH32 = @"program files";
        private const string PARMS = "/uninstall /origin:\"{0}\"";
        
        private void Install()
        {
            if (AppContext.IsInstalled)
            {
                if (string.IsNullOrEmpty(AppContext.XapSource))
                {
                    MessageBox.Show(
                        "Please remove the program by right-clicking anywhere on the application and choosing 'Remove'");
                }
                else
                {
                    var path = string.Format(LAUNCHER, PATH64);
                    if (!File.Exists(path))
                    {
                        path = string.Format(LAUNCHER, PATH32);
                    }
                    var application = AutomationFactory.CreateObject("Shell.Application");
                    application.ShellExecute(path, 
                        string.Format(PARMS, AppContext.XapSource),
                                             "", "", 0);
                    AppContext.PromptBeforeClose = false;
                    Thread.Sleep(200);
                    Application.Current.MainWindow.Close();
                }
            }
            else
            {
                Application.Current.InstallStateChanged += CurrentInstallStateChanged;
                Application.Current.Install();
                SetInstallText();
            }
        }

        private void CurrentInstallStateChanged(object sender, 
            EventArgs e)
        {
            if (!AppContext.IsInstalled)
            {
                return;
            }

            Application.Current.InstallStateChanged -= 
                CurrentInstallStateChanged;
            OverlayHelper.Show();
            EventAggregator.Publish(Globals.OobView.
                AsViewNavigationArgs());
        }

        private void MakeNew()
        {
            EventAggregator.Publish(new ViewNavigationArgs(Globals.EditView)
                .AddNamedParameter("State", "New"));            
        }

        public void HandleEvent(UnhandledExceptionEvent publishedEvent)
        {
            publishedEvent.Handled = true;
            var message = Debugger.IsAttached
                              ? publishedEvent.UncaughtException.ToString()
                              : publishedEvent.UncaughtException.Message;
            MessageBox.Show(message, "Unexpected Error", MessageBoxButton.OK);
        }

        public void HandleEvent(MessageNewToDoItem publishedEvent)
        {
            if (AppContext.IsRunningOutOfBrowser)
            {
                Notification.Notify("New Task Added",
                    string.Format("A new task with the title {0} was successfully added.", 
                    publishedEvent.Item.Title),
                    TimeSpan.FromSeconds(5));
            }
        }

        public void HandleEvent(MessageToDoItemComplete publishedEvent)
        {
            if (AppContext.IsRunningOutOfBrowser)
            {
                Notification.Notify("Task Marked Complete",
                    string.Format("The task with the title {0} was marked complete.",
                    publishedEvent.Item.Title),
                    TimeSpan.FromSeconds(5));
            }
        }
    }
}