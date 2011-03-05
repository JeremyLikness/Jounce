using System;
using System.ComponentModel.Composition;
using Jounce.Regions.Core;

namespace Jounce.Regions.Adapters
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = false)]
    public class RegionAdapterForAttribute : ExportAttribute   
    {
        public RegionAdapterForAttribute(Type targetType) : base(typeof(IRegionAdapterBase))
        {
            TargetType = targetType;
        }

        public Type TargetType { get; set; }
    }    
}
