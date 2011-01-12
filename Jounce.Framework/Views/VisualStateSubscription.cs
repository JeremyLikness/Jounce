using System;
using System.Windows;
using System.Windows.Controls;

namespace Jounce.Framework.Views
{
    /// <summary>
    ///     Subscription to the visual state aggregator service
    /// </summary>
    public class VisualStateSubscription
    {
        private readonly WeakReference _targetControl;
        private readonly string _state;
        private readonly bool _useTransitions;
        private readonly string _event;
        private Guid _id = Guid.NewGuid();

        public VisualStateSubscription(Control control, string vsmEvent, string state, bool useTransitions)
        {
            _targetControl = new WeakReference(control);
            _event = vsmEvent;
            _state = state;
            _useTransitions = useTransitions;
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

        public void RaiseEvent(string eventName)
        {
            if (IsExpired || !_event.Equals(eventName)) return;

            var control = _targetControl.Target as Control;
            VisualStateManager.GoToState(control, _state, _useTransitions);
        }

        public override bool Equals(object obj)
        {
            return obj is VisualStateSubscription && ((VisualStateSubscription)obj)._id.Equals(_id);
        }

        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }
    }

}