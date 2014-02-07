namespace JounceDesktop.Core.Application
{
    /// <summary>
    ///     Severity for the log message
    /// </summary>
    public enum LogSeverity : short
    {
        /// <summary>
        /// All activities
        /// </summary>
        Verbose = 0,
        /// <summary>
        /// Only import information
        /// </summary>
        Information = 1,
        /// <summary>
        /// Warnings and worse
        /// </summary>
        Warning = 2,
        /// <summary>
        /// Errors and worse
        /// </summary>
        Error = 3,
        /// <summary>
        /// Critical errors that compromise application execution
        /// </summary>
        Critical = 4
    }
}