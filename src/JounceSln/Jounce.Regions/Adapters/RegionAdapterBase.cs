using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using Jounce.Core.Application;
using Jounce.Regions.Core;

namespace Jounce.Regions.Adapters
{
    /// <summary>
    ///     Base for adapters
    /// </summary>
    /// <typeparam name="TRegionType">The type of region this adapter handles</typeparam>
    public abstract class RegionAdapterBase<TRegionType> : IRegionAdapterBase where TRegionType: Control 
    {
        protected readonly Dictionary<string, TRegionType> Regions = new Dictionary<string,TRegionType>();
        protected readonly Dictionary<string, UserControl> Controls = new Dictionary<string, UserControl>();

        [Import(Constants.SHOW_STATE, AllowDefault = true, AllowRecomposition = true)]
        public string ShowState { get; set; }

        [Import(Constants.HIDE_STATE, AllowDefault = true, AllowRecomposition = true)]
        public string HideState { get; set; }

        /// <summary>
        ///     Logger
        /// </summary>
        [Import(AllowDefault = true,AllowRecomposition = true)]
        public ILogger Logger { get; set; }

        /// <summary>
        ///     Add a region to the list
        /// </summary>
        /// <param name="region">The region</param>
        /// <param name="targetRegion">The name of the region</param>
        public virtual void AddRegion(object region, string targetRegion)
        {
            if (region == null)
            {
                Logger.LogFormat(LogSeverity.Error, GetType().FullName, "AddRegion: null region.");
                throw new ArgumentNullException("region");
            }

            if (!(region is TRegionType))
            {
                Logger.LogFormat(LogSeverity.Error, GetType().FullName, "Region {0} is not a {1}.", region.GetType().FullName, typeof(TRegionType).FullName);
                throw new ArgumentOutOfRangeException("region");
            }
          

            if (Regions.ContainsKey(targetRegion))
            {
                Logger.LogFormat(LogSeverity.Error, GetType().FullName, "Attempt to add duplicate region name: {0}",
                                 targetRegion);
                throw new Exception(string.Format("Duplicate region is not allowed: {0}", targetRegion));
            }

            Regions.Add(targetRegion, (TRegionType)region);
        }

        /// <summary>
        ///     Adds a new control
        /// </summary>
        /// <param name="view"></param>
        /// <param name="viewName"></param>
        /// <param name="targetRegion"></param>
        public virtual void AddView(UserControl view, string viewName, string targetRegion)
        {
            if (view == null)
            {
                throw new ArgumentNullException("view");
            }

            if (string.IsNullOrEmpty(viewName))
            {
                throw new ArgumentNullException("viewName");
            }

            if (Controls.ContainsKey(viewName))
            {
                return;
            }

            _ValidateRegionName(targetRegion);

            Controls.Add(viewName, view);
        }


        /// <summary>
        ///     Activates a control for a region
        /// </summary>
        /// <param name="viewName">The name of the control</param>
        /// <param name="targetRegion">The name of the region</param>
        public abstract void ActivateControl(string viewName, string targetRegion);

        /// <summary>
        ///     Deactivate the control
        /// </summary>
        /// <param name="viewName">The name of the view</param>
        public virtual void DeactivateControl(string viewName)
        {
            _ValidateControlName(viewName);

            if (!string.IsNullOrEmpty(HideState))
            {
                VisualStateManager.GoToState(Controls[viewName], HideState, true);            
            }
        }
        
        /// <summary>
        ///     Does it have a view?
        /// </summary>
        /// <param name="viewName">The name of the view</param>
        /// <returns>True if it does</returns>
        public virtual bool HasView(string viewName)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                throw new ArgumentNullException("viewName");
            }

            return Controls.ContainsKey(viewName);
        }

        protected virtual void _ValidateControlName(string controlName)
        {
            if (string.IsNullOrEmpty(controlName))
            {
                throw new ArgumentNullException("controlName");
            }

            if (!Controls.ContainsKey(controlName))
            {
                throw new Exception(string.Format(Resources.RegionAdapterBase_ValidateControlName_Control_not_found, controlName));
            }
        }

        protected virtual void _ValidateRegionName(string targetRegion)
        {           
            if (!Regions.ContainsKey(targetRegion))
            {
                throw new Exception(string.Format(Resources.RegionAdapterBase_ValidateRegionName_Region_not_found, targetRegion));
            }
        }
    }
}