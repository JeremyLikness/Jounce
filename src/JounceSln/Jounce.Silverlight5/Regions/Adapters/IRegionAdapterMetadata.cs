using System;

namespace Jounce.Regions.Adapters
{
    public interface IRegionAdapterMetadata
    {
        Type TargetType { get; }
    }
}
