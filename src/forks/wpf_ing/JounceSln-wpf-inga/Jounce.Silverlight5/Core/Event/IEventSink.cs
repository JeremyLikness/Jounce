namespace Jounce.Core.Event
{
    /// <summary>
    ///     Event sink for published events
    /// </summary>
    /// <typeparam name="T">The type of the event</typeparam>
    /// <remarks>
    /// Implement this interface when you wish to listen for published events. You must
    /// also subscribe using the <see cref="IEventAggregator"/> before the messages
    /// will be pushed to the <see cref="HandleEvent"/> method.
    /// </remarks>
    public interface IEventSink<in T>
    {
        /// <summary>
        /// This method is called when a message of type <typeparamref name="T"/> is received
        /// </summary>
        /// <param name="publishedEvent">The published message</param>
        void HandleEvent(T publishedEvent);
    }
}