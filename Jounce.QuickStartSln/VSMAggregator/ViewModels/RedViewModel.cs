using System;
using System.Collections.Generic;
using Jounce.Core.ViewModel;
using Jounce.Framework;
using VSMAggregator.Contracts;

namespace VSMAggregator.ViewModels
{    
    [ExportAsViewModel(Globals.VIEWMODEL_RED)]
    public class RedViewModel : BaseViewModel, IRedViewModel
    {
        private string _currentDate;
        public string CurrentDate
        {
            get { return _currentDate; }
            set
            {
                _currentDate = value;
                RaisePropertyChanged(()=>CurrentDate);
            }
        }

        public RedViewModel()
        {
            CurrentDate = DateTime.Now.ToString();
        }

        protected override void ActivateView(string viewName, IDictionary<string, object> parameters)
        {
            CurrentDate = DateTime.Now.ToString();
        }

        protected override void InitializeVm()
        {
            EventAggregator.Publish(Globals.VIEW_GREEN.AsViewNavigationArgs());
            GoToVisualState("ShowState", false);
        }
    }
}