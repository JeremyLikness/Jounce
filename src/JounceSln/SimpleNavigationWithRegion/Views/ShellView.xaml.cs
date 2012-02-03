using System.ComponentModel.Composition;
using Jounce.Core.View;
using Jounce.Core.ViewModel;

namespace SimpleNavigationWithRegion.Views
{
    [ExportAsView("Shell",IsShell = true)]
    public partial class ShellView
    {
        public ShellView()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     This will allow for the binding of the view model to the view
        /// </summary>
        [Export]
        public ViewModelRoute Binding
        {
            get
            {
                return ViewModelRoute.Create("Shell", "Shell");
            }
        }

    }

}
