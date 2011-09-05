using System;
using System.Windows;
using Jounce.Core.Workflow;

namespace Jounce.Framework.Workflow
{
    /// <summary>
    ///     Workflow that manages a routed event
    /// </summary>
    /// <remarks>
    /// Use this to wrap a routed event into the asynchronous workflow
    /// <example>The following example demonstrates linking the action:
    /// yield return new WorkflowEvent(
    ///    () => { }, 
    ///    h => Control.Click += h, 
    ///    h => Control.Click -= h);
    /// </example>
    /// </remarks>
    public class WorkflowRoutedEvent : IWorkflow
    {
        /// <summary>
        /// Begin the event
        /// </summary>
        private readonly Action _begin;

        /// <summary>
        /// Handler for the routed event
        /// </summary>
        private readonly RoutedEventHandler _handler;

        /// <summary>
        /// Delegate to unregistre to the event
        /// </summary>
        private readonly Action<RoutedEventHandler> _unregister;

        /// <summary>
        ///  Delegate to see whether the event was handled
        /// </summary>
        private readonly Action<RoutedEventArgs> _handle;

        /// <summary>
        /// Main constructor
        /// </summary>
        /// <param name="begin">Action to kick things off</param>
        /// <param name="register">Delegate to register to the event</param>
        /// <param name="unregister">Delegate to unregister from the event</param>
        /// <param name="handle">Delegate to call with the event args and set whether the event was handled</param>
        public WorkflowRoutedEvent(Action begin, Action<RoutedEventHandler> register, Action<RoutedEventHandler> unregister, Action<RoutedEventArgs> handle = null)
        {
            _begin = begin;
            _unregister = unregister;
            _handler = Completed;
            _handle = handle;
            register(_handler);
        }

        /// <summary>
        /// Called when completed
        /// </summary>
        /// <param name="sender">The host for the event</param>
        /// <param name="args">The args</param>
        public void Completed(object sender, RoutedEventArgs args)
        {
            Result = args; 
            if (_handle != null)
            {
                _handle(args);
            }
            _unregister(_handler);
            Invoked();
        }

        /// <summary>
        /// Result of the event
        /// </summary>
        public RoutedEventArgs Result { get; private set; }

        /// <summary>
        /// Called to kick it off
        /// </summary>
        public void Invoke()
        {
            _begin();
        }

        /// <summary>
        ///  Handled by the controller
        /// </summary>
        public Action Invoked { get; set; }
    }

    /// <summary>
    /// Typed version of <see cref="WorkflowRoutedEvent"/>
    /// </summary>
    /// <typeparam name="T">The type of routed event to handle</typeparam>
    /// <remarks>
    /// <see cref="WorkflowRoutedEvent"/> for an example
    /// </remarks>
    public class WorkflowRoutedEvent<T> : IWorkflow where T: RoutedEventArgs
    {
        /// <summary>
        ///  Begin the event
        /// </summary>
        private readonly Action _begin;

        /// <summary>
        ///  Handler for the event
        /// </summary>
        private readonly RoutedEventHandler _handler;

        /// <summary>
        /// Delegate to unregister the event
        /// </summary>
        private readonly Action<RoutedEventHandler> _unregister;

        /// <summary>
        ///  Delegate to mark the event as handled or not
        /// </summary>
        private readonly Action<T> _handle;

        /// <summary>
        /// Default constructor for an event wrapper
        /// </summary>
        /// <param name="begin">Action to begin</param>
        /// <param name="register">Delegate to register to the event</param>
        /// <param name="unregister">Delegate to unregister from the event</param>
        /// <param name="handle">Delegate to mark if the event was handled</param>
        public WorkflowRoutedEvent(Action begin, Action<RoutedEventHandler> register, Action<RoutedEventHandler> unregister, Action<T> handle = null)
        {
            _begin = begin;
            _unregister = unregister;
            _handler = Completed;
            _handle = handle;
            register(_handler);
        }

        /// <summary>
        /// Called when completed
        /// </summary>
        /// <param name="sender">The host</param>
        /// <param name="args">The args</param>
        public void Completed(object sender, RoutedEventArgs args)
        {
            Result = (T)args;
            if (_handle != null)
            {
                _handle((T)args);
            }
            _unregister(_handler);
            Invoked();
        }

        /// <summary>
        /// Result of the event
        /// </summary>
        public T Result { get; private set; }

        /// <summary>
        ///  Invoke is called to kick it off
        /// </summary>
        public void Invoke()
        {
            _begin();
        }

        /// <summary>
        ///  Invoked is called/wired by the controller
        /// </summary>
        public Action Invoked { get; set; }
    }   
}