using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Jounce.Core.Application;
using Jounce.Core.ViewModel;
using Jounce.Framework;

namespace RegionManagement.ViewModels
{
    /// <summary>
    ///     This is an example of creating a custom logger. In this case, the logger
    ///     happens to be a view model as well that we can bind to a list and show
    ///     the most recent messages. Increase capacity to see more messages.
    /// </summary>
    [Export(typeof(ILogger))]
    [ExportAsViewModel(DEBUG_VM)]
    public class DebugViewModel : BaseViewModel, ILogger 
    {
        public const string DEBUG_VM = "DebugViewModel";

        private const int CAPACITY = 10;

        private LogSeverity _severity = LogSeverity.Verbose;

        /// <summary>
        ///     A queue to hold just the most recent messages
        /// </summary>
        private readonly Queue<string> _messages = new Queue<string>(CAPACITY);

        /// <summary>
        ///     Messages
        /// </summary>
        public IEnumerable<string> Messages
        {
            get
            {
                return from m in _messages orderby m descending select m;
            }
        }

        /// <summary>
        ///     Sets the severity 
        /// </summary>
        /// <param name="minimumLevel">Minimum level</param>
        public void SetSeverity(LogSeverity minimumLevel)
        {
            _severity = minimumLevel;
        }

        private void _Enqueue(string message)
        {
            _messages.Enqueue(string.Format("{0} {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), message));
            if (_messages.Count == CAPACITY)
            {
                _messages.Dequeue();
            }
            JounceHelper.ExecuteOnUI(()=>RaisePropertyChanged(()=>Messages));
        }

        /// <summary>
        ///     Log with a message
        /// </summary>
        /// <param name="severity">The severity</param>
        /// <param name="source">The source</param>
        /// <param name="message">The message</param>
        public void Log(LogSeverity severity, string source, string message)
        {
            if ((int)severity >= (int)_severity)
            {
                _Enqueue(string.Format("{0} {1} {2}", severity, source, message));
            }
        }

        /// <summary>
        ///     Log with an exception
        /// </summary>
        /// <param name="severity">The severity</param>
        /// <param name="source">The source</param>
        /// <param name="exception">The exception</param>
        public void Log(LogSeverity severity, string source, Exception exception)
        {
            if ((int)severity >= (int)_severity)
            {
                _Enqueue(string.Format("{0} {1} {2}", severity, source, exception));
            }
        }

        /// <summary>
        ///     Log with formatting
        /// </summary>
        /// <param name="severity">The severity</param>
        /// <param name="source">The source</param>
        /// <param name="messageTemplate">The message template</param>
        /// <param name="arguments">The lines to log</param>
        public void LogFormat(LogSeverity severity, string source, string messageTemplate, params object[] arguments)
        {
            if ((int)severity >= (int)_severity)
            {
                _Enqueue(string.Format("{0} {1} {2}", severity, source, string.Format(messageTemplate, arguments)));
            }
        }
    }
}