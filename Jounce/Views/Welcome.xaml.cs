using System.ComponentModel.Composition;
using System.Windows.Controls;
using Jounce.Core;
using Jounce.Core.View;
using Jounce.Core.ViewModel;

namespace Jounce.Views
{
    /// <summary>
    ///     Jounce expects exactly one view to be exported with the "Shell" contract
    ///     to set as the root visual. We also export this as a view to register it
    ///     with the system. Jounce will automatically activate the shell view to
    ///     bind it to the view model and call initialize/activate. Run in debug
    ///     to see the verbose messages you can receive using the default logger.
    /// </summary>
    /// <remarks>
    ///     You can export all of your views and view models with any name or tag you like.
    ///     The only exception is the shell view. The name/tag must be the full name
    ///     of the view with namespace because the ApplicationService only knows how
    ///     to fetch the "type" when it exports the view.
    /// </remarks>
    [ExportAsView("Jounce.Views.Welcome")]
    [Export(Constants.SHELL,typeof(UserControl))]
    public partial class Welcome
    {
        public Welcome()
        {
            InitializeComponent();            
        }

        /// <summary>
        ///     We can do this anywhere, even in a separate file, but this export
        ///     tells Jounce that the "WelcomeViewModel" should be bound to the
        ///     "Welcome" view. Without this, the view will show but no view model
        ///     will get bound. Jounce allows you to bind view models to multiple
        ///     views.
        /// </summary>
        [Export]
        public ViewModelRoute Binding
        {
            get
            {
                return ViewModelRoute.Create("Welcome", "Jounce.Views.Welcome");
            }
        }
    }
}
