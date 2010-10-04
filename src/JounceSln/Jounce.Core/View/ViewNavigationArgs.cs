using System;

namespace Jounce.Core.View
{
    /// <summary>
    ///     Args when a view navigation is requested
    /// </summary>
    public class ViewNavigationArgs : EventArgs
    {
        public ViewNavigationArgs(Type viewType)
        {
            ViewType = viewType.FullName;
        }

        public ViewNavigationArgs(string viewType)
        {
            ViewType = viewType;
        }

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