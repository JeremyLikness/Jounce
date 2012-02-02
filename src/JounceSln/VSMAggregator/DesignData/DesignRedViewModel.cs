using System;
using VSMAggregator.Contracts;

namespace VSMAggregator.DesignData
{
    public class DesignRedViewModel : IRedViewModel 
    {
        public string CurrentDate
        {
            get { return "January 15, 2011"; }
        }
    }
}