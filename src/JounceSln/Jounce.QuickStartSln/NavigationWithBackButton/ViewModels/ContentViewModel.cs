using Jounce.Core.ViewModel;

namespace NavigationWithBackButton.ViewModels
{
    public abstract class ContentViewModel : BaseViewModel 
    {
        protected override void ActivateView(string viewName)
        {
            GoToVisualState("VisibleState", true);
            base.ActivateView(viewName);
        }

        protected override void DeactivateView(string viewName)
        {
            GoToVisualState("InvisibleState", true);
            base.DeactivateView(viewName);
        }
    }
}