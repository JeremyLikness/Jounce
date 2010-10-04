using System.ComponentModel.Composition;
using Jounce.Core.View;
using Jounce.Core.ViewModel;

namespace Jounce.Views
{
    /// <summary>
    ///     Jounce expects exactly one view to be exported with the "IsShell" contract
    ///     to set as the root visual. We also export this as a view to register it
    ///     with the system. Jounce will automatically activate the shell view to
    ///     bind it to the view model and call initialize/activate. Run in debug
    ///     to see the verbose messages you can receive using the default logger.
    /// </summary>
    [ExportAsView("Welcome",IsShell = true)]
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
                return ViewModelRoute.Create("Welcome", "Welcome");
            }
        }
    }
}
