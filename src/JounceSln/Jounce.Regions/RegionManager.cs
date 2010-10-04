﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Jounce.Core.Application;
using Jounce.Core.Event;
using Jounce.Core.View;
using Jounce.Regions.Adapters;
using Jounce.Regions.Core;

namespace Jounce.Regions
{
    /// <summary>
    ///     Region manager - handles all region adapters
    /// </summary>
    [Export(typeof (IRegionManager))]
    public class RegionManager : IRegionManager, IPartImportsSatisfiedNotification, IEventSink<ViewNavigatedArgs>
    {
        /// <summary>
        ///     Event aggregator to subscribe to view navigation events
        /// </summary>
        [Import]
        public IEventAggregator EventAggregator { get; set; }

        /// <summary>
        ///     Flag to avoid duplicate subscriptions when imports change
        /// </summary>
        private bool _subscribed;

        /// <summary>
        ///     Views already processed
        /// </summary>
        private readonly List<string> _processedViews = new List<string>();
     
        /// <summary>
        ///     Logging tool
        /// </summary>
        [Import(AllowDefault = true, AllowRecomposition = true)]
        public ILogger Logger { get; set; }

        /// <summary>
        ///     Region adapters
        /// </summary>
        [ImportMany(AllowRecomposition = true)]
        public Lazy<IRegionAdapterBase, IRegionAdapterMetadata>[] RegionAdapters { get; set; }        

        /// <summary>
        ///     Helper method to get the region adapter that manages a type
        /// </summary>
        /// <param name="targetRegionType">The type of the region</param>
        /// <returns>The region adapter</returns>
        private Lazy<IRegionAdapterBase, IRegionAdapterMetadata> GetRegionAdapterForType(Type targetRegionType)
        {
            return (from r in RegionAdapters where r.Metadata.TargetType.Equals(targetRegionType) select r).
                FirstOrDefault();
        }

        /// <summary>
        ///     Helper method to get all region adapters that manage a view
        /// </summary>
        /// <param name="viewName">The name of the view</param>
        /// <returns>The list of region adapters that manage the view</returns>
        private IEnumerable<IRegionAdapterBase> GetRegionAdaptersForView(string viewName)
        {
            return from r in RegionAdapters where r.IsValueCreated && r.Value.HasView(viewName) select r.Value;
        }

        /// <summary>
        ///     Views
        /// </summary>
        [ImportMany(AllowRecomposition = true)]
        public Lazy<UserControl, IExportViewToRegionMetadata>[] Views { get; set; }

        /// <summary>
        ///     Keep track of the regions
        /// </summary>
        private readonly Dictionary<string,UIElement> _regions = new Dictionary<string, UIElement>();

        /// <summary>
        ///     Get the info for a view
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        private Lazy<UserControl, IExportViewToRegionMetadata> GetViewInfo(string viewName)
        {
            return (from v in Views where v.Metadata.ViewTypeForRegion.Equals(viewName) select v).FirstOrDefault();
        }

        /// <summary>
        ///     Expose the view metadata
        /// </summary>
        /// <param name="viewType">The view type</param>
        /// <returns>The metadata for the view</returns>
        public IExportViewToRegionMetadata this[string viewType]
        {
            get
            {
                return (from v in Views
                        where v.Metadata.ViewTypeForRegion.Equals(viewType, StringComparison.InvariantCultureIgnoreCase)
                        select v.Metadata).FirstOrDefault();
            }
        }

        /// <summary>
        ///     Activate a view, anywhere
        /// </summary>
        /// <param name="viewName">The name of the view</param>
        public void ActivateView(string viewName)
        {
            var name = GetType().FullName;

            if (Logger != null)
            {
                Logger.LogFormat(LogSeverity.Verbose, name, Resources.RegionManager_ActivateView_Request_to_activate_view, viewName);
            }

            if (string.IsNullOrEmpty(viewName))
            {
                throw new ArgumentNullException("viewName");
            }

            var viewInfo = GetViewInfo(viewName);

            if (viewInfo == null)
            {
                return;
            }

            if (!_processedViews.Contains(viewName) && _regions.ContainsKey(viewInfo.Metadata.TargetRegion))
            {
                // add any views that were waiting for this region to become available 
                var region = _regions[viewInfo.Metadata.TargetRegion];
                var regionAdapterInfo = GetRegionAdapterForType(region.GetType());
                if (regionAdapterInfo != null)
                {
                    var regionAdapter = regionAdapterInfo.Value;
                    regionAdapter.AddView(viewInfo.Value, viewName, viewInfo.Metadata.TargetRegion);
                    _processedViews.Add(viewName);
                }
                
            }
            
            foreach(var ra in GetRegionAdaptersForView(viewName))
            {
                ra.ActivateControl(viewName, viewInfo.Metadata.TargetRegion);                
            }                                  
        }

        /// <summary>
        ///     Activate a view, anywhere
        /// </summary>
        /// <param name="viewName">The name of the view</param>
        public void DeactivateView(string viewName)
        {
            Logger.LogFormat(LogSeverity.Verbose, GetType().FullName, Resources.RegionManager_DeactivateView_Request_to_deactive_view, viewName);

            if (string.IsNullOrEmpty(viewName))
            {
                throw new ArgumentNullException("viewName");
            }

            var viewInfo = GetViewInfo(viewName);

            if (viewInfo == null)
            {
                return;
            }
            
            foreach (var ra in GetRegionAdaptersForView(viewName))
            {
                ra.DeactivateControl(viewName);
            }                                  
        }        

        /// <summary>
        ///     Register a region
        /// </summary>
        /// <param name="region">The region</param>
        /// <param name="regionTag">The tag for the region</param>
        public void RegisterRegion(UIElement region, string regionTag)
        {            
            if (_regions.ContainsKey(regionTag))
            {
                return;
            }

            var regionAdapterInfo = GetRegionAdapterForType(region.GetType());

            IRegionAdapterBase regionAdapter = null;

            if (regionAdapterInfo != null)
            {
                regionAdapter = regionAdapterInfo.Value;
            }
                
            if (regionAdapter == null)
            {
                throw new Exception(string.Format(Resources.RegionManager_RegisterRegion_Region_type_is_not_supported,
                                                  region.GetType().FullName));
            }

            if (Logger != null)
            {
                Logger.LogFormat(LogSeverity.Information, GetType().FullName, Resources.RegionManager_RegisterRegion_Added_region, regionTag);
            }

            _regions.Add(regionTag, region);
            regionAdapter.AddRegion(region, regionTag);

            // add any views that were waiting for this region to become available 
            foreach (var viewInfo in Views.Where(viewInfo => viewInfo.Metadata.TargetRegion.Equals(regionTag)))
            {
                regionAdapter.AddView(viewInfo.Value, 
                    viewInfo.Metadata.ViewTypeForRegion,                    
                    viewInfo.Metadata.TargetRegion);
            }
        }
      
        /// <summary>
        /// Called when a part's imports have been satisfied and it is safe to use.
        /// </summary>
        public void OnImportsSatisfied()
        {
            if (_subscribed) return;

            EventAggregator
                .SubscribeOnDispatcher(this);

            _subscribed = true;
        }

        public void HandleEvent(ViewNavigatedArgs navigationEvent)
        {
            if (navigationEvent.Deactivate)
            {
                DeactivateView(navigationEvent.ViewType);
            }
            else
            {
                ActivateView(navigationEvent.ViewType);
            }
        }
    }
}