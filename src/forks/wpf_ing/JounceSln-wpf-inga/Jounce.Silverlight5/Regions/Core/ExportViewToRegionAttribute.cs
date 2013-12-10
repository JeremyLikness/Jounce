using System;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace Jounce.Regions.Core
{
    /// <summary>
    ///  Tag to export a view to a region
    /// </summary>
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false)]
    public class ExportViewToRegionAttribute : ExportAttribute
    {
        /// <summary>
        ///     Allow typed view export
        /// </summary>
        /// <param name="viewType">The type of the view</param>
        /// <param name="targetRegion">The target region</param>
        public ExportViewToRegionAttribute(Type viewType, string targetRegion) : this(viewType.FullName, targetRegion)
        {
            
        }

        /// <summary>
        ///     Allow tagged view export
        /// </summary>
        /// <param name="viewType">The type of the view</param>
        /// <param name="targetRegion">The target region</param>
        public ExportViewToRegionAttribute(string viewType, string targetRegion) : base(typeof(UserControl))
        {
            ViewTypeForRegion = viewType;
            TargetRegion = targetRegion;
        }

        /// <summary>
        /// The tag for the view
        /// </summary>
        public string ViewTypeForRegion { get; private set; }

        /// <summary>
        ///  The tag for the region
        /// </summary>
        public string TargetRegion { get; private set; }
    }
}