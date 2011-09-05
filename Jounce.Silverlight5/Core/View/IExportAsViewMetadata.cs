namespace Jounce.Core.View
{
    /// <summary>
    ///     Meta data for the export as view
    /// </summary>
    public interface IExportAsViewMetadata
    {
        /// <summary>
        /// The tag for the view. If exported as a type, will be the full name for the type.
        /// </summary>        
        string ExportedViewType { get; }

        /// <summary>
        /// True if the view is the main shell for the application
        /// </summary>
        bool IsShell { get; }

        /// <summary>
        /// Category for the view, used to group similar views
        /// </summary>
        string Category { get; }

        /// <summary>
        /// Name for the view when it appears on a menu or navigation button
        /// </summary>
        string MenuName { get; }

        /// <summary>
        /// A tool tip to further describe the view
        /// </summary>
        string ToolTip { get; }
    }
}