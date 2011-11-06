using System;
using System.Collections.Generic;
using System.Windows;
using Jounce.Core;
using Jounce.Core.View;

namespace Jounce.Framework
{
    /// <summary>
    ///     Application core extensions
    /// </summary>
    /// <remarked>
    /// Used for easy extension methods, thread management, etc.
    /// </remarked>
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
        /// <param name="viewName">The tag for the view</param>
        /// <returns>The tag converted to a <see cref="ViewNavigationArgs"/></returns>
        public static ViewNavigationArgs AsViewNavigationArgs(this string viewName)
        {
            return new ViewNavigationArgs(viewName);            
        }

        /// <summary>
        ///    Publish as an out of browser window
        /// </summary>
        /// <param name="args">The original <see cref="ViewNavigationArgs"/></param>
        /// <returns>The args with the parameters appended</returns>
        public static ViewNavigationArgs AsOutOfBrowserWindow(this ViewNavigationArgs args)
        {
            args.AddNamedParameter(Constants.AS_WINDOW, true);
            return args;
        }

        /// <summary>
        /// Add a title to the parameter
        /// </summary>
        /// <param name="args">The original <see cref="ViewNavigationArgs"/></param>
        /// <param name="title">The title</param>
        /// <returns>The args with the parameter for title appended</returns>
        public static ViewNavigationArgs WithTitle(this ViewNavigationArgs args, string title)
        {
            args.AddNamedParameter(Constants.WINDOW_TITLE, title);
            return args;
        }

        /// <summary>
        /// Add a width for a window in OOB mode
        /// </summary>
        /// <param name="args">The original <see cref="ViewNavigationArgs"/></param>
        /// <param name="width">The width for the window</param>
        /// <returns>The args with the width parameter appended</returns>
        public static ViewNavigationArgs WindowWidth(this ViewNavigationArgs args, double width)
        {
            args.AddNamedParameter(Constants.WINDOW_WIDTH, width);
            return args;
        }

        /// <summary>
        /// Add a width for a window in OOB mode
        /// </summary>
        /// <param name="args">The original <see cref="ViewNavigationArgs"/></param>
        /// <param name="height">The height for the window</param>
        /// <returns>The args with the height parameter appended</returns>
        public static ViewNavigationArgs WindowHeight(this ViewNavigationArgs args, double height)
        {
            args.AddNamedParameter(Constants.WINDOW_HEIGHT, height);
            return args;
        }

        /// <summary>
        ///     Allow fluent addition of parameters
        /// </summary>
        /// <typeparam name="T">The type of the parameter</typeparam>
        /// <param name="args">The view navigation arguments</param>
        /// <param name="name">The name of the parameter</param>
        /// <param name="value">The value of the parameter</param>
        /// <returns>The instance of <see cref="ViewNavigationArgs"/></returns>
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
