using System;
using Jounce.Core.Workflow;

namespace Jounce.Framework.Workflow
{
    /// <summary>
    ///     Workflow that manages an event
    /// </summary>
    public class WorkflowEvent : IWorkflow 
    {
        private readonly Action _begin;

        private readonly EventHandler _handler;

        private readonly Action<EventHandler> _unregister;
        
        public WorkflowEvent(Action begin, Action<EventHandler> register, Action<EventHandler> unregister)
        {
            _begin = begin;
            _unregister = unregister;
            _handler = Completed;
            register(_handler);            
        }

        public void Completed(object sender, EventArgs args)
        {
            Result = args;
            _unregister(_handler);
            Invoked();
        }

        public EventArgs Result { get; private set; }

        public void Invoke()
        {
            _begin();
        }

        public Action Invoked { get; set; }
    }

    public class WorkflowEvent<T> : IWorkflow where T: EventArgs
    {
        private readonly Action _begin;

        private readonly EventHandler<T> _handler;

        private readonly Action<EventHandler<T>> _unregister;

        public WorkflowEvent(Action begin, Action<EventHandler<T>> register, Action<EventHandler<T>> unregister)
        {
            _begin = begin;
            _unregister = unregister;
            _handler = Completed;
            register(_handler);
        }

        public void Completed(object sender, T args)
        {
            Result = args;
            _unregister(_handler);
            Invoked();
        }

        public T Result { get; private set; }

        public void Invoke()
        {
            _begin();
        }

        public Action Invoked { get; set; }
    }
}