using System;

namespace Jounce.Core.Event
{
    /// <summary>
    ///     An unhandled exception - emitted via the event aggregator to be processed as needed
    /// </summary>
    public class UnhandledExceptionEvent
    {
        public Exception UncaughtException { get; set; }
        public bool Handled { get; set; }
    }
}