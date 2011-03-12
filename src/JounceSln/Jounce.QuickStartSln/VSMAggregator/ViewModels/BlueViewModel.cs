using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using Jounce.Core.Command;
using Jounce.Core.ViewModel;
using Jounce.Framework;
using Jounce.Framework.Command;
using Jounce.Framework.View;
using VSMAggregator.Contracts;

namespace VSMAggregator.ViewModels
{
    [ExportAsViewModel(Globals.VIEWMODEL_BLUE)]
    public class BlueViewModel : BaseViewModel, IBlueViewModel  
    {
        public BlueViewModel()
        {
            Dates = new ObservableCollection<string>();
            RedCommand = new ActionCommand<object>(o=>_RedAction());     
        }
       
        public ObservableCollection<string> Dates { get; private set; }

        public IActionCommand RedCommand { get; private set; }

        [Import]
        public VisualStateAggregator VsmAggregator { get; set; }

        private void _RedAction()
        {
            GoToVisualState("HideState", true);
            VsmAggregator.PublishEvent("NavigateRed");
            EventAggregator.Publish(Globals.VIEW_RED.AsViewNavigationArgs());
        }

        protected override void InitializeVm()
        {
            Dates.Add(string.Format("Initialized: {0}", DateTime.Now));
            GoToVisualState("HideState", false);
        }

        protected override void ActivateView(string viewName, IDictionary<string, object> parameters)
        {
            Dates.Add(string.Format("Activated: {0}", DateTime.Now));
            GoToVisualState("ShowState", true);
        }        
    }
}