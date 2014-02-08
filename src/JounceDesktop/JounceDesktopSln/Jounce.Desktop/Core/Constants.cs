namespace Jounce.Desktop.Core
{
    /// <summary>
    ///     Application-wide constants
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Generic message showing something is happening for a busy overlay
        /// </summary>
        /// <remarks>
        /// Jounce will raise this when loading a dynamic module DLL
        /// </remarks>
        public const string BEGIN_BUSY = "Jounce.Desktop.Begin.Busy";

        /// <summary>
        /// Generic message to end the busy overlay 
        /// </summary>
        public const string END_BUSY = "Jounce.Desktop.End.Busy";

        /// <summary>
        /// Initialization parameter name for specifying the logging level
        /// </summary>
        public const string INIT_PARAM_LOGLEVEL = "Jounce.Desktop.LogLevel";

        /// <summary>
        /// Parameter to set a view as a window
        /// </summary>
        public const string AS_WINDOW = "Jounce.Desktop.Window";

        /// <summary>
        /// Window width
        /// </summary>
        public const string WINDOW_WIDTH = "Jounce.Desktop.WindowWidth";

        /// <summary>
        /// Window height
        /// </summary>
        public const string WINDOW_HEIGHT = "Jounce.Desktop.WindowHeight";

        /// <summary>
        /// Set window title
        /// </summary>
        public const string WINDOW_TITLE = "Jounce.Title";

        /// <summary>
        /// Set a reference to the window
        /// </summary>
        public const string WINDOW_REFERENCE = "Jounce.WindowReference";
    }
}