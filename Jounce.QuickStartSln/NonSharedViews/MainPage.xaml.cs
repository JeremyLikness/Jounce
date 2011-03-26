using System.ComponentModel.Composition;
using Jounce.Core.View;
using Jounce.Core.ViewModel;

namespace NonSharedViews
{
    [ExportAsView("MainPage", IsShell=true)]
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        [Export]
        public ViewModelRoute Binding
        {
            get { return ViewModelRoute.Create("MainVM", "MainPage"); }
        }
    }
}
