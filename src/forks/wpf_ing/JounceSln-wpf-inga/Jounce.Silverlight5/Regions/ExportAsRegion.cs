using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows;
using Jounce.Regions.Core;

namespace Jounce.Regions
{
    /// <summary>
    ///     Behavior to export as a region
    /// </summary>
    public class ExportAsRegion
    {
        /// <summary>
        ///     Instance for composition
        /// </summary>
        private static readonly ExportAsRegion _instance = new ExportAsRegion();
        
        /// <summary>
        /// Reference to the <see cref="IRegionManager"/>
        /// </summary>
        [Import]
        public IRegionManager RegionManager { get; set; }

        /// <summary>
        /// Dependency property for the name (tag) of the region
        /// </summary>
        public static DependencyProperty RegionNameProperty = DependencyProperty.RegisterAttached("RegionName", 
            typeof(string), 
            typeof(ExportAsRegion),
            new PropertyMetadata(new PropertyChangedCallback(OnRegionAttached)));

        /// <summary>
        /// Called to import the references
        /// </summary>
        public void Import()
        {
            if (!DesignerProperties.IsInDesignTool)
            {
                CompositionInitializer.SatisfyImports(this);
            }
        }

        /// <summary>
        ///   Invoked when attached to the region
        /// </summary>
        /// <param name="sender">The host control</param>
        /// <param name="args">The event arguments</param>
        public static void OnRegionAttached(object sender, DependencyPropertyChangedEventArgs args)
        {
            if (_instance.RegionManager == null)
            {
                _instance.Import();       
            }

            if (!(args.NewValue is string)) return;
            if (!(sender is UIElement)) return;
            if (_instance.RegionManager == null) return;
            
            var region = (string) args.NewValue;
            _instance.RegionManager.RegisterRegion((UIElement) sender, region);
        }

        /// <summary>
        /// Called to get the name of the region
        /// </summary>
        /// <param name="obj">The object the region is tagged to</param>
        /// <returns>The name of the region</returns>
        public static string GetRegionName(DependencyObject obj)
        {
            return (string) obj.GetValue(RegionNameProperty);
        }

        /// <summary>
        /// Called to set the name of the region
        /// </summary>
        /// <param name="obj">The object it is tagged to</param>
        /// <param name="value">The region tag</param>
        public static void SetRegionName(DependencyObject obj, string value)
        {
            obj.SetValue(RegionNameProperty, value);
        }
    }
}
