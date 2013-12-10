using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Jounce.Regions.Adapters
{
    /// <summary>
    /// Region adapter for <see cref="ItemsControl"/>
    /// </summary>
    /// <remarks>
    /// Simply adds views to the items control - view models should respond to navigation events to 
    /// hide/show as needed.
    /// </remarks>
    [RegionAdapterFor(typeof(ItemsControl))]
    public class ItemsRegion : RegionAdapterBase<ItemsControl> 
    {
        /// <summary>
        ///     Keep track of views already added
        /// </summary>
        private readonly List<string> _addedViews = new List<string>();
        private readonly Dictionary<string,ObservableCollection<UserControl>> _views 
            = new Dictionary<string, ObservableCollection<UserControl>>();
        private readonly List<string> _boundRegions = new List<string>(); 

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

            if (!_boundRegions.Contains(targetRegion))
            {
                if (!_views.ContainsKey(targetRegion))
                {
                    _views.Add(targetRegion, new ObservableCollection<UserControl>());
                }
                region.ItemsSource = _views[targetRegion];
                _boundRegions.Add(targetRegion);
            }

            if (_addedViews.Contains(viewName)) return;

            _addedViews.Add(viewName);
            _views[targetRegion].Add(Controls[viewName]);
        }        
    }
}
