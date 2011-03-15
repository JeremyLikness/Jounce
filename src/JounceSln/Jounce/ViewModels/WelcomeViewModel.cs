using Jounce.Core.ViewModel;

namespace JounceApplication.ViewModels
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
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            GoToVisualState("WelcomeState", true);
        }
    }
}