using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Jounce.Regions.Adapters
{
    [RegionAdapterFor(typeof(ItemsControl))]
    public class ItemsRegion : RegionAdapterBase<ItemsControl> 
    {
        /// <summary>
        ///     Keep track of views already added
        /// </summary>
        private readonly List<string> _addedViews = new List<string>();

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

            if (!_addedViews.Contains(viewName))
            {
                _addedViews.Add(viewName);                                
            }
            else
            {
                region.Items.Remove(Controls[viewName]); // remove to re-add so it is on top
            }

            region.Items.Add(Controls[viewName]);

            if (ShowState != null)
            {
                VisualStateManager.GoToState(Controls[viewName], ShowState, true);
            }
        }        
    }
}
