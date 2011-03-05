using System;
using System.Windows;
using Jounce.Core.View;

namespace Jounce.Framework
{
    /// <summary>
    ///     Application core extensions
    /// </summary>
    public static class JounceHelper
    {         
        /// <summary>
        ///     Extension to cast a view as event args
        /// </summary>
        /// <param name="view">The view</param>
        /// <returns>Return the view navigation args</returns>
        public static ViewNavigationArgs AsViewNavigationArgs(this Type view)
        {
            return new ViewNavigationArgs(view);
        }

        /// <summary>
        ///     String to navigation
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public static ViewNavigationArgs AsViewNavigationArgs(this string viewName)
        {
            return new ViewNavigationArgs(viewName);            
        }

        /// <summary>
        ///     Safely dispatch an action
        /// </summary>
        /// <param name="action">The action</param>
        public static void ExecuteOnUI(Action action)
        {
            var dispatcher = Deployment.Current.Dispatcher;
            if (dispatcher.CheckAccess())
                action();
            else dispatcher.BeginInvoke(action);
        }
    }
}
