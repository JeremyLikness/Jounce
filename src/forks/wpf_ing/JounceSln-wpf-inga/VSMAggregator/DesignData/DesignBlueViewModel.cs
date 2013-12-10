using System;
using System.Collections.ObjectModel;
using VSMAggregator.Contracts;

namespace VSMAggregator.DesignData
{
    public class DesignBlueViewModel : IBlueViewModel 
    {
        private readonly ObservableCollection<string> _dates = new ObservableCollection<string>(new[] { "January 1, 2011", "January 15, 2011"});

        public ObservableCollection<string> Dates
        {
            get { return _dates; }
        }
    }
}