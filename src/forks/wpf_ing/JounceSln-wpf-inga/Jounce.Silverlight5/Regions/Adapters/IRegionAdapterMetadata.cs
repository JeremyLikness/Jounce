using System;

namespace Jounce.Regions.Adapters
{
    /// <summary>
    /// Meta data for exporting a region adapter
    /// </summary>
    public interface IRegionAdapterMetadata
    {
        /// <summary>
        /// The type of the control the region adapter manages
        /// </summary>
        Type TargetType { get; }
    }
}
