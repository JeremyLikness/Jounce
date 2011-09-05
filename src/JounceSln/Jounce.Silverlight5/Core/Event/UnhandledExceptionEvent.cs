using System;

namespace Jounce.Core.Event
{
    /// <summary>
    ///     An unhandled exception - emitted via the event aggregator to be processed as needed
    /// </summary>
    /// <remarks>
    /// Subscribe to this message to handle unexpected expections
    /// </remarks>
    public class UnhandledExceptionEvent
    {
        /// <summary>
        /// The exception raised that was not caught by the application
        /// </summary>
        public Exception UncaughtException { get; set; }

        /// <summary>
        /// Set this to true to avoid the exception bubbling to the plug-in
        /// </summary>
        public bool Handled { get; set; }
    }
}