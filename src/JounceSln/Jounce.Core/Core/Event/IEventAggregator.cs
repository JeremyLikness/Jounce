namespace Jounce.Core.Event
{
    /// <summary>
    ///     Event aggregator for generic pub/sub messaging
    /// </summary>
    public interface IEventAggregator
    {
        /// <summary>
        ///     Publish an event
        /// </summary>
        /// <typeparam name="TEvent">The type of the event to publish</typeparam>
        /// <param name="sampleEvent">The event data</param>
        void Publish<TEvent>(TEvent sampleEvent);

        /// <summary>
        ///     Get a handle to subscribe/listen to an event
        /// </summary>
        /// <typeparam name="TEvent">The event type to listen for</typeparam>
        /// <param name="subscriber">The subscriber</param>
        /// <returns>An observable handle to the event</returns>
        void Subscribe<TEvent>(IEventSink<TEvent> subscriber);

        /// <summary>
        ///     Get a handle to subscribe/listen to an event
        /// </summary>
        /// <typeparam name="TEvent">The event type to listen for</typeparam>
        /// <param name="subscriber">The subscriber</param>
        /// <returns>An observable handle to the event</returns>
        void SubscribeOnDispatcher<TEvent>(IEventSink<TEvent> subscriber);        

        /// <summary>
        ///     Unsubscriber
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="unsubscriber"></param>
        void Unsubscribe<TEvent>(IEventSink<TEvent> unsubscriber);
    }
}