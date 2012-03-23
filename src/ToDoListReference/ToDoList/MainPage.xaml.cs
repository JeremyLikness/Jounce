using System.ComponentModel.Composition;
using System.Windows;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using ToDoList.Behaviors;
using ToDoList.ViewModels;

namespace ToDoList
{
    [ExportAsView(typeof(MainPage), IsShell = true)]
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            Loaded += MainPageLoaded;            
        }
                
        void MainPageLoaded(object sender, RoutedEventArgs e)
        {            
            Loaded -= MainPageLoaded;
            EditContainer.Content = null;
            OverlayHelper.Overlay = OverlayRectangle;
            OverlayHelper.Hide();
        }
        
        [Export]
        public ViewModelRoute Binding
        {
            get { return ViewModelRoute.Create<MainViewModel, MainPage>(); }
        }
    }
}
