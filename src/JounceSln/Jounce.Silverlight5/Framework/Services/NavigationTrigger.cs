using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Interactivity;
using Jounce.Core.Event;

namespace Jounce.Framework.Services
{
    /// <summary>
    ///     Trigger for navigation
    /// </summary>
    /// <remarks>
    /// Use this in Xaml to automatically raise a navigation event
    /// </remarks>
    public class NavigationTrigger : TriggerAction<UIElement>
    {
        /// <summary>
        /// Reference to the <see cref="IEventAggregator"/>
        /// </summary>
        private static IEventAggregator _eventAggregator; 

        /// <summary>
        ///     Event aggregator
        /// </summary>
        [Import]
        public IEventAggregator EventAggregator { get; set; }

        /// <summary>
        /// Property for the target view name
        /// </summary>
        public static readonly DependencyProperty TargetProperty = DependencyProperty.Register(
            "Target",
            typeof (string),
            typeof (NavigationTrigger),
            new PropertyMetadata(string.Empty));

        /// <summary>
        /// The view tag to navigate to
        /// </summary>
        public string Target
        {
            get { return GetValue(TargetProperty).ToString(); }
            set { SetValue(TargetProperty, value);}
        }
        
        /// <summary>
        /// Called when the trigger fires
        /// </summary>
        /// <param name="parameter">Optional parameter that is not used</param>
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