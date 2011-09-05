using System.Windows.Controls;

namespace Jounce.Regions.Adapters
{
    /// <summary>
    /// Region adapter for a <see cref="ContentControl"/> region
    /// </summary>
    /// <remarks>
    /// Simply swaps out the views when they are navigated to
    /// </remarks>
    [RegionAdapterFor(typeof(ContentControl))]
    public class ContentRegion : RegionAdapterBase<ContentControl>
    {        
        /// <summary>
        ///     Activates a control for a region
        /// </summary>
        /// <param name="viewName">The name of the control</param>
        /// <param name="targetRegion">The name of the region</param>
        public override void ActivateControl(string viewName, string targetRegion)
        {
            ValidateControlName(viewName);
            ValidateRegionName(targetRegion);

            var region = Regions[targetRegion];            
            region.Content = Controls[viewName];           
        }        
    }
}
