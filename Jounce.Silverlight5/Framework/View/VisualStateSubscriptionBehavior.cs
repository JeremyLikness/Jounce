using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace Jounce.Framework.View
{
    /// <summary>
    /// Behavior used to map events to visual states
    /// </summary>
    public class VisualStateSubscriptionBehavior : Behavior<FrameworkElement>
    {
        /// <summary>
        /// Constructor sets up the necessary references
        /// </summary>
        public VisualStateSubscriptionBehavior()
        {
            if (!DesignerProperties.IsInDesignTool)
            {
                CompositionInitializer.SatisfyImports(this);
            }
        }

        /// <summary>
        /// Reference to the <see cref="VisualStateAggregator"/>
        /// </summary>
        [Import]
        public VisualStateAggregator Aggregator { get; set; }

        /// <summary>
        ///     Event for the behavior
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        ///     State to transition to
        /// </summary>
        public string StateName { get; set; }

        /// <summary>
        ///     True if transitions are used
        /// </summary>
        public bool UseTransitions { get; set; }

        /// <summary>
        /// Called when attached to the control
        /// </summary>
        protected override void OnAttached()
        {
            AssociatedObject.Loaded += _AssociatedObjectLoaded;
        }

        /// <summary>
        ///     When loaded, subscribe
        /// </summary>
        /// <param name="sender">The control</param>
        /// <param name="e">The args for the laoded event</param>
        void _AssociatedObjectLoaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Loaded -= _AssociatedObjectLoaded; // don't repeat this

            Control control = null;

            // iterate to the parent control for the state subscription
            if (AssociatedObject is Control)
            {
                control = AssociatedObject as Control;
            }
            else
            {
                var parent = VisualTreeHelper.GetParent(AssociatedObject);
                while (!(parent is Control) && parent != null)
                {
                    parent = VisualTreeHelper.GetParent(parent);
                }

                if (parent != null)
                {
                    control = parent as Control;
                }
            }

            if (control != null)
            {
                Aggregator.AddSubscription(control, EventName, StateName, UseTransitions);
            }
        }
    }    

}