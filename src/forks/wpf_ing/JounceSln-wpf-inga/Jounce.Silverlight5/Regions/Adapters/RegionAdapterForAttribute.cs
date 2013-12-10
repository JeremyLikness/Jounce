using System;
using System.ComponentModel.Composition;
using Jounce.Regions.Core;

namespace Jounce.Regions.Adapters
{
    /// <summary>
    /// Tag to export a region adapter for a type
    /// </summary>
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = false)]
    public class RegionAdapterForAttribute : ExportAttribute   
    {
        /// <summary>
        /// Constructor takes in the type the adapter is meant for
        /// </summary>
        /// <param name="targetType"></param>
        public RegionAdapterForAttribute(Type targetType) : base(typeof(IRegionAdapterBase))
        {
            TargetType = targetType;
        }

        /// <summary>
        /// The type of control the region adapter is for
        /// </summary>
        public Type TargetType { get; set; }
    }    
}
