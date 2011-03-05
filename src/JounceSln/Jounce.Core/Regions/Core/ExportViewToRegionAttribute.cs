using System;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace Jounce.Regions.Core
{
    /// <summary>
    ///     Tag to export a view to a region
    /// </summary>
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false)]
    public class ExportViewToRegionAttribute : ExportAttribute
    {
        public ExportViewToRegionAttribute(string viewType, string targetRegion) : base(typeof(UserControl))
        {
            ViewTypeForRegion = viewType;
            TargetRegion = targetRegion;
        }

        public string ViewTypeForRegion { get; private set; }

        public string TargetRegion { get; private set; }
    }
}