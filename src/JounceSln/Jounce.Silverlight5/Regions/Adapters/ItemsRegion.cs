using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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
        private readonly ObservableCollection<UserControl> _views = new ObservableCollection<UserControl>();
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
                region.ItemsSource = _views;
                _boundRegions.Add(targetRegion);
            }

            if (_addedViews.Contains(viewName)) return;

            _addedViews.Add(viewName);
            _views.Add(Controls[viewName]);
        }        
    }
}
