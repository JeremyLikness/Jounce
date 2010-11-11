using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Interactivity;
using Jounce.Core.Event;

namespace Jounce.Framework.Services
{
    /// <summary>
    ///     Trigger for navigation
    /// </summary>
    public class NavigationTrigger : TriggerAction<UIElement>, INotifyPropertyChanged 
    {
        private static IEventAggregator _eventAggregator; 

        /// <summary>
        ///     Event aggregator
        /// </summary>
        [Import]
        public IEventAggregator EventAggregator { get; set; }

        private string _navigationTarget; 

        /// <summary>
        ///     Target (name/tag of view to navigate to)
        /// </summary>
        public string Target
        {
            get { return _navigationTarget; }
            set
            {
                _navigationTarget = value;
                var handler = PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs("NavigationTarget"));
                }
            }
        }        

        protected override void Invoke(object parameter)
        {
            if (_eventAggregator == null)
            {
                CompositionInitializer.SatisfyImports(this);
                _eventAggregator = EventAggregator;
            }
            _eventAggregator.Publish(Target.AsViewNavigationArgs());
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
    }    
}