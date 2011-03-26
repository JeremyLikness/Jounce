using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Jounce.Core.View;

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
        /// <param name="parameters">The parameters for the view</param>
        /// <returns>The user control</returns>
        bool ActivateView(string viewName, IDictionary<string, object> parameters);

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
        ///     Get a non-shared version of the view model
        /// </summary>
        /// <param name="viewModelType">The tag for the view model</param>
        /// <returns>A new instance</returns>
        IViewModel GetNonSharedViewModel(string viewModelType);

        /// <summary>
        ///     Returns a non-shared version of the view
        /// </summary>
        /// <param name="viewTag">The view tag</param>
        /// <param name="dataContext">Data context to wire</param>
        /// <returns>The view</returns>
        UserControl GetNonSharedView(string viewTag, object dataContext);

        /// <summary>
        ///     Get the meta data for a view
        /// </summary>
        /// <param name="view">The view</param>
        /// <returns>The meta data for the view</returns>
        IExportAsViewMetadata GetMetadataForView(string view);

        /// <summary>
        ///     Get the view model tag for the view
        /// </summary>
        /// <param name="view">The view</param>
        /// <returns>The view model tag</returns>
        string GetViewModelTagForView(string view);

        /// <summary>
        ///     Get the view model tag for the view
        /// </summary>
        /// <param name="viewModel">The view model tag</param>
        /// <returns>The view tag</returns>
        string[] GetViewTagsForViewModel(string viewModel);   

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
