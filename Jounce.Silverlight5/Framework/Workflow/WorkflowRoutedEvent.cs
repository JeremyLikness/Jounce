using System;
using System.Windows;
using Jounce.Core.Workflow;

namespace Jounce.Framework.Workflow
{
    /// <summary>
    ///     Workflow that manages an event
    /// </summary>
    public class WorkflowRoutedEvent : IWorkflow
    {
        private readonly Action _begin;

        private readonly RoutedEventHandler _handler;

        private readonly Action<RoutedEventHandler> _unregister;

        private readonly Action<RoutedEventArgs> _handle;

        public WorkflowRoutedEvent(Action begin, Action<RoutedEventHandler> register, Action<RoutedEventHandler> unregister, Action<RoutedEventArgs> handle = null)
        {
            _begin = begin;
            _unregister = unregister;
            _handler = Completed;
            _handle = handle;
            register(_handler);
        }

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

        public RoutedEventArgs Result { get; private set; }

        public void Invoke()
        {
            _begin();
        }

        public Action Invoked { get; set; }
    }

    public class WorkflowRoutedEvent<T> : IWorkflow where T: RoutedEventArgs
    {
        private readonly Action _begin;

        private readonly RoutedEventHandler _handler;

        private readonly Action<RoutedEventHandler> _unregister;

        private readonly Action<T> _handle;

        public WorkflowRoutedEvent(Action begin, Action<RoutedEventHandler> register, Action<RoutedEventHandler> unregister, Action<T> handle = null)
        {
            _begin = begin;
            _unregister = unregister;
            _handler = Completed;
            _handle = handle;
            register(_handler);
        }

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

        public T Result { get; private set; }

        public void Invoke()
        {
            _begin();
        }

        public Action Invoked { get; set; }
    }   
}