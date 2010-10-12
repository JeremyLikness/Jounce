using System.Windows;
using System.Windows.Controls;

namespace Jounce.Regions.Adapters
{
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
            _ValidateControlName(viewName);
            _ValidateRegionName(targetRegion);

            var region = Regions[targetRegion];            
            region.Content = Controls[viewName];           
        }        
    }
}
