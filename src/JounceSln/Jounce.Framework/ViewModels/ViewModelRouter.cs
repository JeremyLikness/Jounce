using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Jounce.Core.Application;
using Jounce.Core.View;
using Jounce.Core.ViewModel;

namespace Jounce.Framework.ViewModels
{
    /// <summary>
    ///     This class routes views and view models
    /// </summary>
    [Export(typeof (IViewModelRouter))]
    public class ViewModelRouter : IViewModelRouter
    {
        const string LAYOUT_ROOT = "LayoutRoot";
            
        /// <summary>
        ///     Indexer to user controls
        /// </summary>
        /// <param name="name">The name</param>
        /// <returns>The control</returns>
        public UserControl this[string name]
        {
            get { return ViewQuery(name); }
        }

        /// <summary>
        ///     Query the view and return it
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public UserControl ViewQuery(string name)
        {
            var v = GetViewInfo(name);
            return v == null ? null : v.Value;
        }

        /// <summary>
        ///     True if the view exists
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasView(string name)
        {
            return GetViewInfo(name) != null;
        }

        /// <summary>
        ///     The defined routes
        /// </summary>
        [ImportMany(AllowRecomposition = true)]
        public ViewModelRoute[] Routes { get; set; }

        /// <summary>
        ///     The list of views
        /// </summary>
        [ImportMany(AllowRecomposition = true)]
        public Lazy<UserControl, IExportAsViewMetadata>[] Views { get; set; }

        /// <summary>
        ///     Get info for a view
        /// </summary>
        /// <param name="viewName">The name of the view</param>
        /// <returns></returns>
        private Lazy<UserControl, IExportAsViewMetadata> GetViewInfo(string viewName)
        {
            return (from v in Views where v.Metadata.ExportedViewType.Equals(viewName) select v).FirstOrDefault();
        }

        /// <summary>
        ///     The list of view models
        /// </summary>
        [ImportMany(AllowRecomposition = true)]
        public Lazy<IViewModel, IExportAsViewModelMetadata>[] ViewModels { get; set; }

        /// <summary>
        ///     Gets the view model information for a view
        /// </summary>
        /// <param name="view">The view</param>
        /// <returns>The corresponding view model information</returns>
        private Lazy<IViewModel, IExportAsViewModelMetadata> GetViewModelInfoForView(string view)
        {
            return (from r in Routes
                    from vm in ViewModels
                    where r.ViewType.Equals(view)
                          && r.ViewModelType.Equals(vm.Metadata.ViewModelType)
                    select vm).FirstOrDefault();
        }

        [Import(AllowDefault = true, AllowRecomposition = true)]
        public ILogger Logger { get; set; }

        /// <summary>
        ///     Deactivates a view
        /// </summary>
        /// <param name="viewName">The view name</param>
        /// <returns>The user control</returns>
        public bool DeactivateView(string viewName)
        {
            if (HasView(viewName))
            {
                var vm = GetViewModelInfoForView(viewName);
                if (vm != null)
                {                    
                    if (vm.IsValueCreated)
                    {
                        vm.Value.Deactivate(viewName);
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///     Resolve the view model
        /// </summary>
        /// <param name="viewModelType">The type of view model</param>
        /// <returns>The view model</returns>
        public IViewModel ResolveViewModel(string viewModelType)
        {
            return
                (from vm in ViewModels where vm.Metadata.ViewModelType.Equals(viewModelType) select vm.Value).
                    FirstOrDefault();
        }

        /// <summary>
        ///     Resolve using type
        /// </summary>
        /// <param name="viewModelType"></param>
        /// <returns></returns>
        public IViewModel ResolveViewModel(Type viewModelType)
        {
            return ResolveViewModel(viewModelType.FullName);
        }

        /// <summary>
        ///     Resolve the view model
        /// </summary>
        /// <returns>The view model</returns>
        public T ResolveViewModel<T>(string viewModelType = null) where T : IViewModel
        {
            return ResolveViewModel<T>(true, viewModelType);
        }

        /// <summary>
        ///     Resolve the view model based on type
        /// </summary>
        /// <typeparam name="T">Type of the view model</typeparam>
        /// <param name="activate">False to suppress activation</param>
        /// <param name="viewModelType">Optional type when not using type names</param>
        /// <returns>The view model instance</returns>
        public T ResolveViewModel<T>(bool activate, string viewModelType = null) where T : IViewModel
        {
            if (viewModelType == null)
            {
                viewModelType = typeof (T).FullName;
            }

            var vmInfo = (from vm in ViewModels
                          where vm.Metadata.ViewModelType.Equals(viewModelType)
                          select vm).FirstOrDefault();

            if (vmInfo == null)
            {
                return default(T);
            }

            var initialize = !vmInfo.IsValueCreated;

            var viewModel = vmInfo.Value;

            if (initialize)
            {
                viewModel.Initialize();
            }

            if (activate)
            {
                viewModel.Activate(string.Empty);
            }

            return (T) viewModel;
        }

        /// <summary>
        ///     Activates a view and returns the view
        /// </summary>
        /// <param name="viewName">The view name</param>
        /// <returns>The user control</returns>
        public bool ActivateView(string viewName)
        {
            Logger.LogFormat(LogSeverity.Verbose, GetType().FullName, Resources.ViewModelRouter_ActivateView,
                             MethodBase.GetCurrentMethod().Name,
                             viewName);

            if (HasView(viewName))
            {
                var view = ViewQuery(viewName);

                var viewModelInfo = GetViewModelInfoForView(viewName);

                if (viewModelInfo != null)
                {
                    var firstTime = !viewModelInfo.IsValueCreated;

                    var viewModel = viewModelInfo.Value;

                    var baseViewModel = (BaseViewModel) viewModel;

                    if (!baseViewModel.RegisteredViews.Contains(viewName))
                    {
                        baseViewModel.RegisterVisualState(viewName, 
                            (state, transitions) =>
                            JounceHelper.ExecuteOnUI(() => VisualStateManager.GoToState(view, state,
                                                                                        transitions)));
                        _BindViewModel(view, viewModel);
                        baseViewModel.RegisteredViews.Add(viewName);
                    }
                    
                    if (firstTime)
                    {
                        viewModel.Initialize();
                    }

                    viewModel.Activate(viewName);
                }

                return true;
            }
            return false;
        }

        /// <summary>
        ///     Binds 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="viewModel"></param>
        private static void _BindViewModel(FrameworkElement view, IViewModel viewModel)
        {
            var root = view.FindName(LAYOUT_ROOT);
            if (root != null)
            {
                ((FrameworkElement) root).DataContext = viewModel;
            }
            else
            {
                view.Loaded += (o, e) => view.DataContext = viewModel;
            }
        }
    }
}