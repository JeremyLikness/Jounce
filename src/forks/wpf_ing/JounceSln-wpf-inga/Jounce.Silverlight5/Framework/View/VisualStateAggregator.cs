using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace Jounce.Framework.View
{
    /// <summary>
    ///     Handles aggregation of visual state events between separate controls
    /// </summary>
    /// <remarks>
    /// Use to coordinate visual states across views and controls
    /// </remarks>
    [Export]
    public class VisualStateAggregator
    {
        /// <summary>
        ///     Subscriptions
        /// </summary>
        private readonly List<VisualStateSubscription> _subscribers = new List<VisualStateSubscription>();

        /// <summary>
        ///     Subscribe
        /// </summary>
        /// <param name="control">Control</param>
        /// <param name="eventName">Name of the event to subscribe to</param>
        /// <param name="stateName">State to transition to when the event is raised</param>
        /// <param name="useTransitions">True to use transitions</param>
        public void AddSubscription(Control control, string eventName, string stateName, bool useTransitions)
        {
            _subscribers.Add(new VisualStateSubscription(control, eventName, stateName, useTransitions));
        }

        /// <summary>
        ///     Publish an aggregated visual state change
        /// </summary>
        /// <param name="eventName">The name of the event</param>
        public void PublishEvent(string eventName)
        {
            // list for references that have gone out of scope
            var expired = new List<VisualStateSubscription>();

            // iterate and either add to expirations or raise the event
            foreach (var subscriber in _subscribers)
            {
                if (subscriber.IsExpired)
                {
                    expired.Add(subscriber);
                }
                else
                {
                    subscriber.RaiseEvent(eventName);
                }
            }

            // scrub the stale references
            expired.ForEach(s => _subscribers.Remove(s));
        }
    }

}