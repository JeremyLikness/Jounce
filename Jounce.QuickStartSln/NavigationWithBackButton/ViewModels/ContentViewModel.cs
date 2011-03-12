using System.Collections.Generic;
using Jounce.Core.ViewModel;

namespace NavigationWithBackButton.ViewModels
{
    public abstract class ContentViewModel : BaseViewModel 
    {
        protected override void ActivateView(string viewName, IDictionary<string, object> parameters)
        {
            GoToVisualState("VisibleState", true);
            base.ActivateView(viewName, parameters);
        }

        protected override void DeactivateView(string viewName)
        {
            GoToVisualState("InvisibleState", true);
            base.DeactivateView(viewName);
        }
    }
}