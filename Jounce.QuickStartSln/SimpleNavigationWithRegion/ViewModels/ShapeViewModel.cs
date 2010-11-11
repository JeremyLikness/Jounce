using Jounce.Core.View;
using Jounce.Core.ViewModel;

namespace SimpleNavigationWithRegion.ViewModels
{
    [ExportAsViewModel("ShapeViewModel")]
    public class ShapeViewModel : BaseViewModel
    {
        private string _lastView = string.Empty;

        public override void _Activate(string viewName)
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

        public override void _Deactivate(string viewName)
        {
            GoToVisualStateForView(viewName, "HideState", true);
        }
    }
}