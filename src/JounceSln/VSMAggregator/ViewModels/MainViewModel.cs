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
        protected override void InitializeVm()
        {
            EventAggregator.Publish(Globals.VIEW_RED.AsViewNavigationArgs());
        }
    }
}