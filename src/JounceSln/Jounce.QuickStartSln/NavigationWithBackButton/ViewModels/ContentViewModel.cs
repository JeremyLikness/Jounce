using Jounce.Core.ViewModel;

namespace NavigationWithBackButton.ViewModels
{
    public abstract class ContentViewModel : BaseViewModel 
    {
        public override void _Activate(string viewName)
        {
            GoToVisualState("VisibleState", true);
            base._Activate(viewName);
        }

        public override void _Deactivate(string viewName)
        {
            GoToVisualState("InvisibleState", true);
            base._Deactivate(viewName);
        }
    }
}