using System.Windows;
using Jounce.Core.View;

namespace Jounce.Regions.Core
{
    /// <summary>
    ///     Region manager
    /// </summary>
    public interface IRegionManager
    {
        /// <summary>
        ///     Expose the view metadata
        /// </summary>
        /// <param name="viewType">The view type</param>
        /// <returns>The metadata for the view</returns>
        IExportViewToRegionMetadata this[string viewType] { get; }

        /// <summary>
        ///     Activate a view, anywhere
        /// </summary>
        /// <param name="viewName">The name of the view</param>
        void ActivateView(string viewName);     

        /// <summary>
        ///     Activate a view, anywhere
        /// </summary>
        /// <param name="viewName">The name of the view</param>
        void DeactivateView(string viewName);

        /// <summary>
        ///     Register a region
        /// </summary>
        /// <param name="region">The region</param>
        /// <param name="regionTag">The tag for the region</param>
        void RegisterRegion(UIElement region, string regionTag);
    }
}