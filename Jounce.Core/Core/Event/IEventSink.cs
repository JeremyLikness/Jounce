namespace Jounce.Core.Event
{
    /// <summary>
    ///     Event sink for published events
    /// </summary>
    /// <typeparam name="T">The type of the event</typeparam>
    public interface IEventSink<in T>
    {
        void HandleEvent(T publishedEvent);
    }
}