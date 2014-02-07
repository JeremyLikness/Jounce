using JounceDesktop.Core.Application;
using System;

namespace JounceDektop.Core.Application
{
    /// <summary>
    /// Logger interface
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Sets the severity 
        /// </summary>
        /// <param name="minimumLevel">Minimum level</param>
        void SetSeverity(LogSeverity minimumLevel);

        /// <summary>
        ///     Log with a message
        /// </summary>
        /// <param name="severity">The severity</param>
        /// <param name="source">The source</param>
        /// <param name="message">The message</param>
        void Log(LogSeverity severity, string source, string message);

        /// <summary>
        ///     Log with an exception
        /// </summary>
        /// <param name="severity">The severity</param>
        /// <param name="source">The source</param>
        /// <param name="exception">The exception</param>
        void Log(LogSeverity severity, string source, Exception exception);

        /// <summary>
        ///     Log with formatting
        /// </summary>
        /// <param name="severity">The severity</param>
        /// <param name="source">The source</param>
        /// <param name="messageTemplate">The message template</param>
        /// <param name="arguments">The lines to log</param>
        void LogFormat(LogSeverity severity, string source, string messageTemplate, params object[] arguments);
    }
}