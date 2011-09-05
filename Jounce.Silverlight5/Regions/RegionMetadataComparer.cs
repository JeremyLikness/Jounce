using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Jounce.Regions.Core;

namespace Jounce.Regions
{
    /// <summary>
    ///     Comparer for region metadata
    /// </summary>
    /// <remarks>
    /// Facilitates filtering in the region manager
    /// </remarks>
    public class RegionMetadataComparer : IEqualityComparer<Lazy<UserControl,IExportViewToRegionMetadata>>
    {
        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        /// <param name="x">The first object of type <paramref name="T"/> to compare.</param><param name="y">The second object of type <paramref name="T"/> to compare.</param>
        public bool Equals(Lazy<UserControl, IExportViewToRegionMetadata> x, Lazy<UserControl, IExportViewToRegionMetadata> y)
        {
            return x.Metadata.ViewTypeForRegion.Equals(y.Metadata.ViewTypeForRegion);
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <returns>
        /// A hash code for the specified object.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> for which a hash code is to be returned.</param><exception cref="T:System.ArgumentNullException">The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.</exception>
        public int GetHashCode(Lazy<UserControl, IExportViewToRegionMetadata> obj)
        {
            return obj.Metadata.ViewTypeForRegion.GetHashCode();
        }
    }
}