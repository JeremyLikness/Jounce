using System.Collections.Generic;
using Jounce.Core.View;
using Jounce.Core.ViewModel;

namespace SimpleNavigationWithRegion.ViewModels
{
    [ExportAsViewModel("ShapeViewModel")]
    public class ShapeViewModel : BaseViewModel
    {
        private string _lastView = string.Empty;

        protected override void ActivateView(string viewName, IDictionary<string, object> parameters)
        {
            if (string.IsNullOrEmpty(_lastView))
            {
                _lastView = viewName;
            }
            else if (!_lastView.Equals(viewName))
            {
                EventAggregator.Publish(new ViewNavigationArgs(_lastView) {Deactivate = true});
                _lastView = viewName;
            }

            GoToVisualStateForView(viewName, "ShowState", true);
        }

        protected override void DeactivateView(string viewName)
        {
            GoToVisualStateForView(viewName, "HideState", true);
        }
    }
}