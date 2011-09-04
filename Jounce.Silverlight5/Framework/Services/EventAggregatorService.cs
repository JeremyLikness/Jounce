using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Threading;
using Jounce.Core.Application;
using Jounce.Core.Event;

namespace Jounce.Framework.Services
{
    /// <summary>
    ///     Implementation of event aggregator pattern using reactive extensions
    /// </summary>
    [Export(typeof (IEventAggregator))]
    public class EventAggregatorService : IEventAggregator
    {
        /// <summary>
        ///     Mutex for locking
        /// </summary>
        private static readonly object _mutex = new object();

        /// <summary>
        ///     Subscribers
        /// </summary>
        private static readonly Dictionary<Type, List<Tuple<bool,WeakReference>>>
            _subscribers = new Dictionary<Type, List<Tuple<bool,WeakReference>>>();

        /// <summary>
        ///     Logger
        /// </summary>
        [Import(AllowDefault = true, AllowRecomposition = true)]
        public ILogger Logger { get; set; }

        /// <summary>
        ///     Set up the event for the first time
        /// </summary>
        /// <typeparam name="TEvent">The type of the event</typeparam>
        private static void _RegisterEvent<TEvent>()
        {
            if (_subscribers.ContainsKey(typeof (TEvent)))
            {
                return;
            }

            Monitor.Enter(_mutex);
            if (!_subscribers.ContainsKey(typeof (TEvent)))
            {
                _subscribers.Add(typeof (TEvent), new List<Tuple<bool,WeakReference>>());
            }
            Monitor.Exit(_mutex);
        }

        /// <summary>
        ///     Publish an event
        /// </summary>
        /// <typeparam name="TEvent">The type of the event to publish</typeparam>
        /// <param name="sampleEvent">The event data</param>
        public void Publish<TEvent>(TEvent sampleEvent)
        {
            Logger.LogFormat(LogSeverity.Verbose, GetType().FullName, "{0} : {1} : {2}", MethodBase.GetCurrentMethod().Name,
                             sampleEvent.GetType().FullName, sampleEvent);

            if (!_subscribers.ContainsKey(typeof (TEvent)))
            {
                // no one is listening
                return;
            }

            // snapshot the list
            Monitor.Enter(_mutex);
            var subscribers = (from sub in _subscribers[typeof (TEvent)] select sub).ToArray();
            Monitor.Exit(_mutex);

            // now filter through and mark any dead subscriptions
            var dead = new List<Tuple<bool,WeakReference>>();
            foreach (var sub in subscribers)
            {
                var sink = sub.Item2.Target as IEventSink<TEvent>;
                if (sink == null || !sub.Item2.IsAlive)
                {
                    dead.Add(sub);
                }
                else
                {
                    if (sub.Item1)
                    {
                        JounceHelper.ExecuteOnUI(()=>sink.HandleEvent(sampleEvent));                        
                    }
                    else
                    {
                        sink.HandleEvent(sampleEvent);
                    }
                }
            }

            // scrub the dead subscriptions
            Monitor.Enter(_mutex);
            foreach (var deadSub in dead.Where(deadSub => _subscribers[typeof (TEvent)].Contains(deadSub)))
            {
                _subscribers[typeof (TEvent)].Remove(deadSub);
            }
            Monitor.Exit(_mutex);
        }

        /// <summary>
        ///     Get a handle to subscribe/listen to an event
        /// </summary>
        /// <typeparam name="TEvent">The event type to listen for</typeparam>
        /// <param name="subscriber">The subscriber</param>
        /// <returns>An observable handle to the event</returns>
        public void Subscribe<TEvent>(IEventSink<TEvent> subscriber)
        {
            _RegisterEvent<TEvent>();
            _subscribers[typeof(TEvent)].Add(Tuple.Create(false,new WeakReference(subscriber)));
        }       

        /// <summary>
        ///     Get a handle to subscribe/listen to an event
        /// </summary>
        /// <typeparam name="TEvent">The event type to listen for</typeparam>
        /// <param name="subscriber">The subscriber</param>
        /// <returns>An observable handle to the event</returns>
        public void SubscribeOnDispatcher<TEvent>(IEventSink<TEvent> subscriber)
        {
            _RegisterEvent<TEvent>();
            _subscribers[typeof(TEvent)].Add(Tuple.Create(true,new WeakReference(subscriber)));
        }       

        /// <summary>
        ///     Unsubscribe
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="unsubscriber"></param>
        public void Unsubscribe<TEvent>(IEventSink<TEvent> unsubscriber)
        {
            if (!_subscribers.ContainsKey(typeof (TEvent)))
            {
                return;
            }
            var unsub = (from s in _subscribers[typeof (TEvent)]
                         where ReferenceEquals(s.Item2.Target, unsubscriber)
                         select s).ToList();
            Monitor.Enter(_mutex);
            {
                foreach (var subCast in
                    unsub.Select(sub => sub)
                        .Where(subCast => _subscribers[typeof (TEvent)].Contains(subCast)))
                {
                    _subscribers[typeof (TEvent)].Remove(subCast);
                }
            }
            Monitor.Exit(_mutex);
        }
    }
}