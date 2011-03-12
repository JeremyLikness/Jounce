using System;
using System.Collections.Generic;
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
        ///     Allow fluent addition of parameters
        /// </summary>
        /// <typeparam name="T">The type of the parameter</typeparam>
        /// <param name="name">The name of the parameter</param>
        /// <param name="value">The value of the parameter</param>
        /// <returns></returns>
        public static ViewNavigationArgs AddNamedParameter<T>(this ViewNavigationArgs args, string name, T value)
        {
            args.ViewParameters.Add(name, value);
            return args;
        }

        /// <summary>
        ///     Retrieve the parameter value from the dictionary
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="parameters">The parameter collection</param>
        /// <param name="name">The name of the parameter</param>
        /// <returns>The parameter value</returns>
        public static T ParameterValue<T>(this IDictionary<string, object> parameters, string name)
        {
            if (parameters.ContainsKey(name) && parameters[name] is T)
            {
                return (T) parameters[name];
            }

            return default(T);
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
