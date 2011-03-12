using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Controls;
using Jounce.Core.Event;
using Jounce.Core.View;
using Jounce.Regions.Adapters;

namespace RegionManagement.Adapters
{
    /// <summary>
    ///     The tab control is a bit complex with the header, so it is not part of core Jounce
    ///     This adapter is an example of how it could be implemented using the built-in metadata
    /// </summary>
    [RegionAdapterFor(typeof(TabControl))]
    public class TabRegion : RegionAdapterBase<TabControl>
    {
        [Import]
        public IEventAggregator EventAggregator { get; set; }

        /// <summary>
        ///     Keep track of views already added
        /// </summary>
        private readonly List<string> _addedViews = new List<string>();

        /// <summary>
        ///     View metadata
        /// </summary>
        [ImportMany(AllowRecomposition=true)]
        public Lazy<UserControl, IExportAsViewMetadata>[] Views { get; set; }               
        
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

            if (!_addedViews.Contains(viewName))
            {
                _addedViews.Add(viewName);
                _SetupTabForView(region, viewName);
            }
                                   
            region.SelectedIndex = _addedViews.IndexOf(viewName);
        }

        /// <summary>
        ///     Set up the tab - insert the view
        /// </summary>
        /// <param name="region">The region</param>
        /// <param name="viewName">The view</param>
        private void _SetupTabForView(ItemsControl region, string viewName)
        {
            var metadata =
                (from v in Views where v.Metadata.ExportedViewType.Equals(viewName) select v.Metadata).FirstOrDefault();

            var header = metadata == null ? viewName : metadata.MenuName;
            
            var tabControlItem = new TabItem {Header = header, Content = Controls[viewName]};            

            region.Items.Add(tabControlItem);           
        }
    }
}