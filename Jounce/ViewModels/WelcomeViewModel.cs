using Jounce.Core.ViewModel;

namespace Jounce.ViewModels
{
    /// <summary>
    ///     Main view model - just export the type to register
    /// </summary>
    [ExportAsViewModel("Welcome")]
    public class WelcomeViewModel : BaseViewModel 
    {
        /// <summary>
        ///     Set up whatever we need here
        /// </summary>
        public WelcomeViewModel()
        {
            Title = "Welcome to Jounce!";            
        }

        private string _title;

        /// <summary>
        ///     Title
        /// </summary>
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged(()=>Title);
            }
        }

        /// <summary>
        ///     On activate, use the auto-linking to go to a visual state and animate the welcome
        /// </summary>
        /// <param name="viewName"></param>
        public override void _Activate(string viewName)
        {            
            GoToVisualState("WelcomeState",true);
        }
    }
}