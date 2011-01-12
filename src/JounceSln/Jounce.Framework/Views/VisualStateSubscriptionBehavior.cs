using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace Jounce.Framework.Views
{
    public class VisualStateSubscriptionBehavior : Behavior<FrameworkElement>
    {
        public VisualStateSubscriptionBehavior()
        {
            if (!DesignerProperties.IsInDesignTool)
            {
                CompositionInitializer.SatisfyImports(this);
            }
        }

        /// <summary>
        ///     Aggreagtor reference
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

        protected override void OnAttached()
        {
            AssociatedObject.Loaded += AssociatedObject_Loaded;
        }

        /// <summary>
        ///     When loaded, subscribe
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Loaded -= AssociatedObject_Loaded; // don't repeat this

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
                if (parent is Control)
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