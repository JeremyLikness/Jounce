using System.ComponentModel.Composition;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using NonSharedViews.ViewModels;

namespace NonSharedViews
{
    [ExportAsView(typeof(MainPage), IsShell=true)]
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        [Export]
        public ViewModelRoute Binding
        {
            get { return ViewModelRoute.Create<MainViewModel,MainPage>(); }
        }
    }
}
