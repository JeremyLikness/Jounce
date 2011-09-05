using System;
using Jounce.Core.Workflow;

namespace Jounce.Framework.Workflow
{
    /// <summary>
    ///     Workflow that manages an event
    /// </summary>
    /// <remarks>
    /// Use this to wrap an event into the asynchronous workflow
    /// <example>The following example demonstrates linking the action:
    /// yield return new WorkflowEvent(
    ///    () => { }, 
    ///    h => DataAccess.LoadCompleted += h, 
    ///    h => DataAccess.LoadCompleted -= h);
    /// </example>
    /// </remarks>
    public class WorkflowEvent : IWorkflow 
    {
        /// <summary>
        /// Begin action
        /// </summary>
        private readonly Action _begin;

        /// <summary>
        /// Event handler to hook/unhook
        /// </summary>
        private readonly EventHandler _handler;

        /// <summary>
        ///  Delegate to unregister the event
        /// </summary>
        private readonly Action<EventHandler> _unregister;
        
        /// <summary>
        /// Constructor with the action to launch, and the delegates to hook and unhook the event
        /// </summary>
        /// <param name="begin">The action to execute that starts the event</param>
        /// <param name="register">The action to register for the event</param>
        /// <param name="unregister">The action to unregister the event when done</param>
        public WorkflowEvent(Action begin, Action<EventHandler> register, Action<EventHandler> unregister)
        {
            _begin = begin;
            _unregister = unregister;
            _handler = Completed;
            register(_handler);            
        }

        /// <summary>
        ///     Called when completed
        /// </summary>
        /// <param name="sender">The event host</param>
        /// <param name="args">The event arguments</param>
        public void Completed(object sender, EventArgs args)
        {
            Result = args;
            _unregister(_handler);
            Invoked();
        }

        /// <summary>
        /// Placeholder for the result of the event (the event args)
        /// </summary>
        public EventArgs Result { get; private set; }

        /// <summary>
        /// Launch the event
        /// </summary>
        public void Invoke()
        {
            _begin();
        }

        /// <summary>
        /// Handled by the controller
        /// </summary>
        public Action Invoked { get; set; }
    }

    /// <summary>
    /// Strongly-typed event wrapper
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="EventArgs"/> for the event</typeparam>
    /// <remarks>
    /// See <see cref="WorkflowEvent"/> for an example
    /// </remarks>
    public class WorkflowEvent<T> : IWorkflow where T: EventArgs
    {
        /// <summary>
        /// Begin the event
        /// </summary>
        private readonly Action _begin;

        /// <summary>
        /// Event handler reference
        /// </summary>
        private readonly EventHandler<T> _handler;

        /// <summary>
        /// Delegate to unhook the event
        /// </summary>
        private readonly Action<EventHandler<T>> _unregister;

        /// <summary>
        /// Constructor for the wrapper
        /// </summary>
        /// <param name="begin">Action to call when it starts</param>
        /// <param name="register">Delegate to register the event</param>
        /// <param name="unregister">Delegate to unregister the event</param>
        public WorkflowEvent(Action begin, Action<EventHandler<T>> register, Action<EventHandler<T>> unregister)
        {
            _begin = begin;
            _unregister = unregister;
            _handler = Completed;
            register(_handler);
        }

        /// <summary>
        /// Called when the event is completed
        /// </summary>
        /// <param name="sender">The event host</param>
        /// <param name="args">The arguments to capture</param>
        public void Completed(object sender, T args)
        {
            Result = args;
            _unregister(_handler);
            Invoked();
        }

        /// <summary>
        /// Holds the results of the event for inspection
        /// </summary>
        public T Result { get; private set; }

        /// <summary>
        /// Kicks off the workflow item
        /// </summary>
        public void Invoke()
        {
            _begin();
        }

        /// <summary>
        /// Handled by the controller
        /// </summary>
        public Action Invoked { get; set; }
    }
}