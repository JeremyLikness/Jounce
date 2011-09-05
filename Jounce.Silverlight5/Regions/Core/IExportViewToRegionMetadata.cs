namespace Jounce.Regions.Core
{
    /// <summary>
    ///     Metadata for targetted views
    /// </summary>
    public interface IExportViewToRegionMetadata
    {
        /// <summary>
        /// The tag for the view
        /// </summary>
        string ViewTypeForRegion { get; }

        /// <summary>
        /// The tag for the region
        /// </summary>
        string TargetRegion { get; }
    }
}