using System;
using System.Collections.Generic;

namespace Jounce.Core.View
{
    /// <summary>
    ///     Args when a view navigation is requested
    /// </summary>
    public class ViewNavigationArgs : EventArgs
    {
        public ViewNavigationArgs()
        {
            ViewParameters = new Dictionary<string, object>();
        }

        public ViewNavigationArgs(Type viewType) : this()
        {
            ViewType = viewType.FullName;
        }

        public ViewNavigationArgs(string viewType) : this()
        {
            ViewType = viewType;
        }

        public ViewNavigationArgs(Type viewType, IDictionary<string, object> parms)
        {
            ViewType = viewType.FullName;
            ViewParameters = parms;
        }

        public ViewNavigationArgs(string viewType, IDictionary<string, object> parms)            
        {
            ViewType = viewType;
            ViewParameters = parms;
        }

        public IDictionary<string, object> ViewParameters { get; set; }

        public bool Deactivate { get; set; }

        /// <summary>
        ///     The name of the view
        /// </summary>
        public string ViewType { get; private set; }

        public override string ToString()
        {
            return string.Format(Resources.ViewNavigationArgs_ToString_ViewNavigation,
                                 Deactivate
                                     ? Resources.ViewNavigationArgs_ToString_Deactivate
                                     : Resources.ViewNavigationArgs_ToString_Activate, ViewType);
        }
    }
}