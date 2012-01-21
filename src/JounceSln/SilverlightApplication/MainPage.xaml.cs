using Jounce.Core.View;

namespace SilverlightApplication
{
    /// <summary>
    /// The shell view is the main view over all of the application
    /// </summary>
    [ExportAsView(typeof(MainPage), IsShell=true)]
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // the code below shows how to export a binding rather than using the fluent
        // method in the boot strapper - add a using for System.ComponentModel.Composition,
        // Jounce.Core.ViewModel, and SilverlightApplication.ViewModels in order to 
        // uncomment the code and use this method instead

        //[Export]
        //public ViewModelRoute Binding
        //{
        //    get { return ViewModelRoute.Create<MainViewModel, MainPage>(); }
        //}
    }
}
