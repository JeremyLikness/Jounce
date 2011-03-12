using System.Collections.Generic;
using System.ComponentModel.Composition;
using Jounce.Core.ViewModel;
using Jounce.Framework;

namespace EventAggregator.ViewModels
{
    [ExportAsViewModel(Constants.VM_MAIN)]
    public class MainViewModel : BaseViewModel 
    {
        /// <summary>
        ///     We only import to wire it up and start listening for errors
        /// </summary>
        [Import(Constants.VIEW_EXCEPTION)]
        public object ExceptionHandler { get; set; }

        /// <summary>
        ///     On activation, active the sub-views as well
        /// </summary>
        /// <param name="viewName"></param>
        protected override void ActivateView(string viewName, IDictionary<string, object> parameters)
        {
            EventAggregator.Publish(Constants.VIEW_SENDER.AsViewNavigationArgs());
            EventAggregator.Publish(Constants.VIEW_RECEIVER.AsViewNavigationArgs());            
        }
    }
}