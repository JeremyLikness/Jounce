using System.Windows;
using System.Windows.Controls;

namespace Jounce.Core.Fluent
{
    /// <summary>
    ///     Fluent region manager
    /// </summary>
    /// <remarks>
    /// Use this for runtime (non-attribute based) configuration of regions
    /// </remarks>
    public interface IFluentRegionManager
    {
        /// <summary>
        /// Export a view to a region
        /// </summary>
        /// <param name="viewName">The tag for the view</param>
        /// <param name="regionTag">The tag for the region</param>
        void ExportViewToRegion(string viewName, string regionTag);

        /// <summary>
        /// Export a view to a region by type
        /// </summary>
        /// <typeparam name="T">The type of the view - the full name will be used as the tag</typeparam>
        /// <param name="regionTag">The region tag</param>
        void ExportViewToRegion<T>(string regionTag) where T : UserControl;

        /// <summary>
        /// Register a region
        /// </summary>
        /// <param name="region">The region itself</param>
        /// <param name="regionTag">The tag for the region</param>
        void RegisterRegion(UIElement region, string regionTag);
    }
}