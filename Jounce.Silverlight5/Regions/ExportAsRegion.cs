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
        
        [Import]
        public IRegionManager RegionManager { get; set; }

        public static DependencyProperty RegionNameProperty = DependencyProperty.RegisterAttached("RegionName", 
            typeof(string), 
            typeof(ExportAsRegion),
            new PropertyMetadata(new PropertyChangedCallback(OnRegionAttached)));

        public void Import()
        {
            if (!DesignerProperties.IsInDesignTool)
            {
                CompositionInitializer.SatisfyImports(this);
            }
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
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

        public static string GetRegionName(DependencyObject obj)
        {
            return (string) obj.GetValue(RegionNameProperty);
        }

        public static void SetRegionName(DependencyObject obj, string value)
        {
            obj.SetValue(RegionNameProperty, value);
        }
    }
}
