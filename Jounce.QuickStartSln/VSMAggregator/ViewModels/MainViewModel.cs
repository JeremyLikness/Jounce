using Jounce.Core.ViewModel;
using Jounce.Framework;

namespace VSMAggregator.ViewModels
{
    [ExportAsViewModel(Globals.VIEWMODEL_MAIN)]
    public class MainViewModel : BaseViewModel 
    {
        /// <summary>
        ///     Just get the red view routed
        /// </summary>
        public override void _Initialize()
        {
            EventAggregator.Publish(Globals.VIEW_RED.AsViewNavigationArgs());
        }
    }
}