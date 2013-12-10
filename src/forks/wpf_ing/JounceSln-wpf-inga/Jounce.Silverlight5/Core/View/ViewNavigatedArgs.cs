using System;

namespace Jounce.Core.View
{
    /// <summary>
    ///     Called after the view is navigated to
    /// </summary>
    /// <remarks>
    /// Listen for this event to react to a view after it has been navigated and the view model
    /// has been bound and wired to the view
    /// </remarks>
    public class ViewNavigatedArgs : ViewNavigationArgs
    {
        /// <summary>
        /// Navigate using a type - use the full name as the tag
        /// </summary>
        /// <param name="viewType">The type of the view</param>
        public ViewNavigatedArgs(Type viewType) : base(viewType)
        {
        }

        /// <summary>
        /// Naviging using a tag
        /// </summary>
        /// <param name="viewType">The view tag</param>
        public ViewNavigatedArgs(string viewType) : base(viewType)
        {
        }
    }
}