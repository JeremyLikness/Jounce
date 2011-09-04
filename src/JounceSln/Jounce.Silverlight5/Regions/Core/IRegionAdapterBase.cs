using System.Windows.Controls;

namespace Jounce.Regions.Core
{
    /// <summary>
    ///     Base for a region adapter
    /// </summary>
    public interface IRegionAdapterBase 
    {
        /// <summary>
        ///     Add a region to the list
        /// </summary>
        /// <param name="region">The region</param>
        /// <param name="targetRegion">The region name to target</param>
        void AddRegion(object region, string targetRegion);

        /// <summary>
        ///     Adds a new control
        /// </summary>
        /// <param name="view"></param>
        /// <param name="viewName"></param>
        /// <param name="targetRegion"></param>
        void AddView(UserControl view, string viewName, string targetRegion);

        /// <summary>
        ///     Activates a control for a region
        /// </summary>
        /// <param name="viewName">The name of the control</param>
        /// <param name="targetRegion">The name of the region</param>
        void ActivateControl(string viewName, string targetRegion);

        /// <summary>
        ///     Deactivate a control
        /// </summary>
        /// <param name="viewName">The name of the control</param>
        void DeactivateControl(string viewName);

        /// <summary>
        ///     Does it have a view?
        /// </summary>
        /// <param name="viewName">The name of the view</param>
        /// <returns>True if it does</returns>
        bool HasView(string viewName);
    }
}