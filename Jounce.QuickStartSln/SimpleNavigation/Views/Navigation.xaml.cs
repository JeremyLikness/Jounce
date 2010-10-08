using System.ComponentModel.Composition;
using Jounce.Core.View;
using Jounce.Core.ViewModel;

namespace SimpleNavigation.Views
{
    [ExportAsView("Navigation")]
    public partial class Navigation
    {
        public Navigation()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     This will allow the binding of the view model to the view
        /// </summary>
        [Export]
        public ViewModelRoute Binding
        {
            get { return ViewModelRoute.Create("Navigation", "Navigation"); }
        }
    }
}
