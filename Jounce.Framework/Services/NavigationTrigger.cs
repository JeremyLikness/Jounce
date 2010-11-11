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
    public class NavigationTrigger : TriggerAction<UIElement>
    {
        private static IEventAggregator _eventAggregator; 

        /// <summary>
        ///     Event aggregator
        /// </summary>
        [Import]
        public IEventAggregator EventAggregator { get; set; }

        public static readonly DependencyProperty TargetProperty = DependencyProperty.Register(
            "Target",
            typeof (string),
            typeof (NavigationTrigger),
            new PropertyMetadata(string.Empty));

        public string Target
        {
            get { return GetValue(TargetProperty).ToString(); }
            set { SetValue(TargetProperty, value);}
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
                
    }    
}