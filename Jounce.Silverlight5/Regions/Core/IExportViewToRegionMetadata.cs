namespace Jounce.Regions.Core
{
    /// <summary>
    ///     Metadata for targetted views
    /// </summary>
    public interface IExportViewToRegionMetadata
    {
        string ViewTypeForRegion { get; }
        string TargetRegion { get; }
    }
}