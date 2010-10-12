using Jounce.Core.ViewModel;

namespace RegionManagement.ViewModels
{
    /// <summary>
    ///     This view model demonstrates a feature of Jounce:
    ///     when the view is bound to the view model, the visual state
    ///     manager is also wired in. Here, we transition states based
    ///     on the view navigation calling the view model interface.
    /// </summary>
    [ExportAsViewModel(CIRCLE_VM)]
    public class CircleViewModel : BaseViewModel
    {
       public const string CIRCLE_VM = "CircleViewModel";

        public override void _Activate(string viewName)
        {
            GoToVisualState("ShowState", true);
        }

        public override void _Deactivate(string viewName)
        {
            GoToVisualState("HideState", true);
        }
    }
}