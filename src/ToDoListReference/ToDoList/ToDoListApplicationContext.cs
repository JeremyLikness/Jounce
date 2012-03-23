using System.ComponentModel.Composition;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Browser;
using ToDoList.Contracts;

namespace ToDoList
{
    [Export(typeof (IToDoListApplicationContext))]
    public class ToDoListApplicationContext
        : IToDoListApplicationContext
    {        
        private readonly IsolatedStorageSettings _appSettings =
            IsolatedStorageSettings.ApplicationSettings;
        
        public ToDoListApplicationContext()
        {
            if (HtmlPage.IsEnabled)
            {
                HtmlPage.RegisterScriptableObject(GetType().Name, this);

                var pluginName = HtmlPage.Plugin.Id;
                HtmlPage.Window.Eval(string.Format(
                    @"window.onbeforeunload = function () {{
                    var slApp = document.getElementById('{0}');
                    var result = slApp.Content.{1}.OnBeforeUnload();
                    if(result.length > 0)
                        return result;
                }}",
                    pluginName, GetType().Name)
                    );
                _appSettings[Global.Constants.XAP_PATH] =
                    Application.Current.Host.Source;
            }
            else if (IsRunningOutOfBrowser)
            {
                Application.Current.MainWindow.Closing += MainWindowClosing;
            }
        }

        private void MainWindowClosing(object sender, System.ComponentModel.ClosingEventArgs e)
        {
            if (PromptBeforeClose)
            {
                var result = MessageBox.Show(
                    "Are you sure you wish to close the application? Changes will be lost.",
                    "Confirm Close",
                    MessageBoxButton.OKCancel);
                e.Cancel = result.Equals(MessageBoxResult.Cancel);
            }
        }

        [ScriptableMember]
        public string OnBeforeUnload()
        {
            return PromptBeforeClose
                       ? "You may have unsaved changes!"
                       : string.Empty;
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                if (HtmlPage.IsEnabled)
                {
                    HtmlPage.Document.SetProperty("title", value);
                }
                else if (IsRunningOutOfBrowser)
                {
                    Application.Current.MainWindow.Title = value;
                }
            }
        }

        public bool PromptBeforeClose { get; set; }

        private const string FILTER_KEY = "Filter";

        public string FilterName
        {
            get
            {
                return _appSettings.Contains(FILTER_KEY)
                           ? (string) _appSettings[FILTER_KEY]
                           : string.Empty;
            }
            set { _appSettings[FILTER_KEY] = value; }
        }

        private const string SORT_KEY = "Sort";

        public string SortName
        {
            get
            {
                return _appSettings.Contains(SORT_KEY)
                           ? (string) _appSettings[SORT_KEY]
                           : string.Empty;
            }
            set { _appSettings[SORT_KEY] = value; }
        }

        public bool IsInstalled
        {
            get
            {
                return Application.Current
                    .InstallState.Equals(InstallState.Installed);                   
            }
        }

        public bool IsRunningOutOfBrowser
        {
            get
            {
                return Application.Current
                    .IsRunningOutOfBrowser;
            }
        }

        public string XapSource
        {
            get
            {
                return _appSettings.Contains(Global.Constants.XAP_PATH) ? 
                    _appSettings[Global.Constants.XAP_PATH].ToString() : 
                    string.Empty;
            }
        }
    }
}