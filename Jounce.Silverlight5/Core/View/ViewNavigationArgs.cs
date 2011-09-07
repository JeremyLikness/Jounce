using System;
using System.Collections.Generic;

namespace Jounce.Core.View
{
    /// <summary>
    ///     Args when a view navigation is requested
    /// </summary>
    /// <remarks>
    /// This event is raised to request a navigation. If you listen to this event, the view may not
    /// yet be in the visual tree and the view model may not yet be bound if this is the first event
    /// for the given view
    /// </remarks>
    public class ViewNavigationArgs : EventArgs
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ViewNavigationArgs()
        {
            ViewParameters = new Dictionary<string, object>();
        }

        /// <summary>
        /// Constructor with a view type, defaults the tag to the full name of the type.
        /// </summary>
        /// <param name="viewType">The type of view to navigate to</param>
        public ViewNavigationArgs(Type viewType) : this()
        {
            ViewType = viewType.FullName;
        }

        /// <summary>
        /// Constructor with a tag.
        /// </summary>
        /// <param name="viewType">The view tag</param>
        public ViewNavigationArgs(string viewType) : this()
        {
            ViewType = viewType;
        }

        /// <summary>
        /// Constructor with an optional parameter dictionary
        /// </summary>
        /// <param name="viewType">The type of the view</param>
        /// <param name="parms">A list of parameters to pass to the view</param>
        public ViewNavigationArgs(Type viewType, IDictionary<string, object> parms)
        {
            ViewType = viewType.FullName;
            ViewParameters = parms;
        }

        /// <summary>
        /// Constructor with an optional parameter dictionary
        /// </summary>
        /// <param name="viewType">The tag for the view</param>
        /// <param name="parms">A list of parmaeters to pass to the view</param>
        public ViewNavigationArgs(string viewType, IDictionary<string, object> parms)            
        {
            ViewType = viewType;
            ViewParameters = parms;
        }

        /// <summary>
        /// Parameters for the view to parse 
        /// </summary>
        public IDictionary<string, object> ViewParameters { get; set; }

        /// <summary>
        /// Set to true if the event is a view being deactivated or removed from the visual tree
        /// </summary>
        public bool Deactivate { get; set; }

        /// <summary>
        ///     The view tag
        /// </summary>
        public string ViewType { get; private set; }

        /// <summary>
        /// String representation
        /// </summary>
        /// <returns>The string representation of the route</returns>
        public override string ToString()
        {
            return string.Format(Resources.ViewNavigationArgs_ToString_ViewNavigation,
                                 Deactivate
                                     ? Resources.ViewNavigationArgs_ToString_Deactivate
                                     : Resources.ViewNavigationArgs_ToString_Activate, ViewType);
        }
    }
}