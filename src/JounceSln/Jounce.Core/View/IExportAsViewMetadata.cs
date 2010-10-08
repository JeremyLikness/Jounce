namespace Jounce.Core.View
{
    /// <summary>
    ///     Meta data for the export as view
    /// </summary>
    public interface IExportAsViewMetadata
    {
        string ExportedViewType { get; }
        bool IsShell { get; }
        string Category { get; }
        string CommandName { get; }
        string ToolTip { get; }
    }
}