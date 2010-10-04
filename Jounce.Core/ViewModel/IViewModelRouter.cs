using System;
using System.Windows.Controls;

namespace Jounce.Core.ViewModel
{
    /// <summary>
    ///     Router for views and view models
    /// </summary>
    public interface IViewModelRouter
    {
        /// <summary>
        ///     Activates a view
        /// </summary>
        /// <param name="viewName">The view name</param>
        /// <returns>The user control</returns>
        bool ActivateView(string viewName);

        /// <summary>
        ///     Deactivates a view
        /// </summary>
        /// <param name="viewName">The view name</param>
        /// <returns>The user control</returns>
        bool DeactivateView(string viewName);

        /// <summary>
        ///     Resolve a view model
        /// </summary>
        /// <param name="viewModelType">The type</param>
        /// <returns>The view model interface</returns>
        IViewModel ResolveViewModel(Type viewModelType);

        /// <summary>
        ///     Resolve a view model
        /// </summary>
        /// <param name="viewModelType">The type</param>
        /// <returns>The view model interface</returns>
        IViewModel ResolveViewModel(string viewModelType);

        /// <summary>
        ///     Resolve the view model based on type
        /// </summary>
        /// <typeparam name="T">Type of the view model</typeparam>
        /// <returns>The view model instance</returns>
        T ResolveViewModel<T>(string viewModelType) where T : IViewModel;

        /// <summary>
        ///     Resolve the view model based on type
        /// </summary>
        /// <typeparam name="T">Type of the view model</typeparam>
        /// <param name="activate">False to suppress activation</param>
        /// <param name="viewModelType">Optional type if typename not used</param>
        /// <returns>The view model instance</returns>
        T ResolveViewModel<T>(bool activate, string viewModelType) where T : IViewModel;

        /// <summary>
        ///     Indexer to view
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        UserControl this[string name] { get; }

        /// <summary>
        ///     View query
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        UserControl ViewQuery(string name);

        /// <summary>
        ///     Has view
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool HasView(string name);        
    }
}
