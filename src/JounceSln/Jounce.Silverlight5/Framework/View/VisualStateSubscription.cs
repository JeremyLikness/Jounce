using System;
using System.Windows;
using System.Windows.Controls;

namespace Jounce.Framework.View
{
    /// <summary>
    ///     Subscription to the visual state aggregator service
    /// </summary>
    /// <remarks>
    /// Use this to indicate when an event is published, the view or control should
    /// transition to a state
    /// </remarks>
    public class VisualStateSubscription
    {
        /// <summary>
        /// Target control with the visual state
        /// </summary>
        private readonly WeakReference _targetControl;

        /// <summary>
        /// The state to transition to
        /// </summary>
        private readonly string _state;

        /// <summary>
        ///  True if the change should use the transitions
        /// </summary>
        private readonly bool _useTransitions;

        /// <summary>
        ///  The name of the event
        /// </summary>
        private readonly string _event;

        /// <summary>
        /// A unique identifier for the subscription
        /// </summary>
        private Guid _id = Guid.NewGuid();

        private readonly int _hashCode;

        /// <summary>
        /// Constructor with the needed parameters
        /// </summary>
        /// <param name="control">The control subscribing</param>
        /// <param name="vsmEvent">The event name subscribed to</param>
        /// <param name="state">The state to go to when the event is published</param>
        /// <param name="useTransitions">True when it should use transitions</param>
        public VisualStateSubscription(Control control, string vsmEvent, string state, bool useTransitions)
        {
            _targetControl = new WeakReference(control);
            _event = vsmEvent;
            _state = state;
            _useTransitions = useTransitions;
            _hashCode = _id.GetHashCode();
        }

        /// <summary>
        ///     Did the reference expire?
        /// </summary>
        public bool IsExpired
        {
            get
            {
                return _targetControl == null || !_targetControl.IsAlive || _targetControl.Target == null;
            }
        }

        /// <summary>
        /// Called when the event is raised
        /// </summary>
        /// <param name="eventName">The event name</param>
        public void RaiseEvent(string eventName)
        {
            if (IsExpired || !_event.Equals(eventName)) return;

            var control = _targetControl.Target as Control;

            if (control != null)
            {
                VisualStateManager.GoToState(control, _state, _useTransitions);
            }
        }

        public override bool Equals(object obj)
        {
            return obj is VisualStateSubscription && ((VisualStateSubscription)obj)._id==_id;
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }
    }

}