using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Jounce.Core.Application;
using Jounce.Core.Fluent;
using Jounce.Core.View;
using Jounce.Core.ViewModel;

namespace Jounce.Framework.ViewModel
{
    /// <summary>
    ///     This class routes views and view models
    /// </summary>
    [Export(typeof (IViewModelRouter))]
    [Export(typeof (IFluentViewModelRouter))]
    public class ViewModelRouter : IViewModelRouter, IFluentViewModelRouter
    {
        private const string LAYOUT_ROOT = "LayoutRoot";

        /// <summary>
        ///     The defined routes
        /// </summary>
        [ImportMany(AllowRecomposition = true)]
        public ViewModelRoute[] Routes { get; set; }

        private readonly List<ViewModelRoute> _fluentRoutes = new List<ViewModelRoute>();

        /// <summary>
        ///     The list of views
        /// </summary>
        [ImportMany(AllowRecomposition = true)]
        public Lazy<UserControl, IExportAsViewMetadata>[] Views { get; set; }

        /// <summary>
        ///     View factories
        /// </summary>
        [ImportMany(AllowRecomposition = true)]
        public List<ExportFactory<UserControl, IExportAsViewMetadata>> ViewFactory { get; set; }

        /// <summary>
        ///     The list of view models
        /// </summary>
        [ImportMany(AllowRecomposition = true)]
        public Lazy<IViewModel, IExportAsViewModelMetadata>[] ViewModels { get; set; }

        /// <summary>
        ///     View model factories
        /// </summary>
        [ImportMany(AllowRecomposition = true)]
        public List<ExportFactory<IViewModel, IExportAsViewModelMetadata>> ViewModelFactory { get; set; }

        /// <summary>
        ///     Get the view model tag for the view
        /// </summary>
        /// <param name="view">The view</param>
        /// <returns>The view model tag</returns>
        public string GetViewModelTagForView(string view)
        {
            var vm = _GetViewModelInfoForView(view);
            return vm == null ? string.Empty : vm.Metadata.ViewModelType;
        }

        /// <summary>
        ///     Get the view model tag for the view
        /// </summary>
        /// <param name="viewModel">The view model tag</param>
        /// <returns>The view tag</returns>
        public string[] GetViewTagsForViewModel(string viewModel)
        {
            var fluent = from r in _fluentRoutes where r.ViewModelType.Equals(viewModel) select r.ViewType;
            var exported = from r in Routes where r.ViewModelType.Equals(viewModel) select r.ViewType;

            return (from v in fluent.Union(exported) select v).Distinct().ToArray();
        }

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
            var v = _GetViewInfo(name);
            return v == null ? null : v.Value;
        }

        /// <summary>
        ///     True if the view exists
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasView(string name)
        {
            return _GetViewInfo(name) != null;
        }


        /// <summary>
        ///     Get info for a view
        /// </summary>
        /// <param name="viewName">The name of the view</param>
        /// <returns></returns>
        private Lazy<UserControl, IExportAsViewMetadata> _GetViewInfo(string viewName)
        {
            return (from v in Views where v.Metadata.ExportedViewType.Equals(viewName) select v).FirstOrDefault();
        }

        /// <summary>
        ///     Gets the view model information for a view
        /// </summary>
        /// <param name="view">The view</param>
        /// <returns>The corresponding view model information</returns>
        private Lazy<IViewModel, IExportAsViewModelMetadata> _GetViewModelInfoForView(string view)
        {
            return (from r in _fluentRoutes
                    from vm in ViewModels
                    where r.ViewType.Equals(view)
                          && r.ViewModelType.Equals(vm.Metadata.ViewModelType)
                    select vm).FirstOrDefault() ??
                   (from r in Routes
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
                var vm = _GetViewModelInfoForView(viewName);
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
        ///     Old resolve view model
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="activate">True to call activate</param>
        /// <param name="viewModelType">The type of the view model</param>
        /// <returns>The view model</returns>
        public T ResolveViewModel<T>(bool activate, string viewModelType = null) where T : IViewModel
        {
            return ResolveViewModel<T>(activate, viewModelType, new Dictionary<string, object>());
        }

        /// <summary>
        ///     Get a non-shared version of the view model
        /// </summary>
        /// <param name="viewModelType">The tag for the view model</param>
        /// <returns>A new instance</returns>
        public IViewModel GetNonSharedViewModel(string viewModelType)
        {
            return (from factory in ViewModelFactory
                    where factory.Metadata.ViewModelType.Equals(viewModelType)
                    select factory.CreateExport().Value).FirstOrDefault();
        }

        /// <summary>
        ///     Returns a non-shared version of the view
        /// </summary>
        /// <param name="viewTag">The view tag</param>
        /// <param name="dataContext">Data context to wire</param>
        /// <returns>The view</returns>
        public UserControl GetNonSharedView(string viewTag, object dataContext)
        {
            var view = (from factory in ViewFactory
                        where factory.Metadata.ExportedViewType.Equals(viewTag)
                        select factory.CreateExport().Value).FirstOrDefault();

            if (view == null)
            {
                return null;
            }

            var baseViewModel = dataContext as BaseViewModel;
            if (baseViewModel != null)
            {
                baseViewModel.RegisterVisualState(viewTag,
                                                  (state, transitions) =>
                                                  JounceHelper.ExecuteOnUI(
                                                      () => VisualStateManager.GoToState(view, state,
                                                                                         transitions)));
                _BindViewModel(view, baseViewModel);
                baseViewModel.RegisteredViews.Add(viewTag);
                baseViewModel.Initialize();
                RoutedEventHandler loaded = null;
                loaded = (o, e) =>
                             {
                                 // ReSharper disable AccessToModifiedClosure
                                 ((UserControl) o).Loaded -= loaded;
                                 // ReSharper restore AccessToModifiedClosure
                                 baseViewModel.Activate(viewTag, new Dictionary<string, object>());
                             };
                view.Loaded += loaded;
            }
            return view;
        }

        /// <summary>
        ///     Resolve the view model based on type
        /// </summary>
        /// <typeparam name="T">Type of the view model</typeparam>
        /// <param name="activate">False to suppress activation</param>
        /// <param name="viewModelType">Optional type when not using type names</param>
        /// <param name="parameters">Parameters for the view</param>
        /// <returns>The view model instance</returns>
        public T ResolveViewModel<T>(bool activate, string viewModelType = null,
                                     IDictionary<string, object> parameters = null) where T : IViewModel
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
                viewModel.Activate(string.Empty, parameters);
            }

            return (T) viewModel;
        }

        /// <summary>
        ///     Get the meta data for a view
        /// </summary>
        /// <param name="view">The view</param>
        /// <returns>The meta data for the view</returns>
        public IExportAsViewMetadata GetMetadataForView(string view)
        {
            var viewInfo = _GetViewInfo(view);
            return viewInfo == null ? null : viewInfo.Metadata;
        }

        /// <summary>
        ///     Activates a view and returns the view
        /// </summary>
        /// <param name="viewName">The view name</param>
        /// <param name="parameters">The parameters for the view</param>
        /// <returns>The user control</returns>
        public bool ActivateView(string viewName, IDictionary<string, object> parameters)
        {
            Logger.LogFormat(LogSeverity.Verbose, GetType().FullName, Resources.ViewModelRouter_ActivateView,
                             MethodBase.GetCurrentMethod().Name,
                             viewName);

            if (HasView(viewName))
            {
                var view = ViewQuery(viewName);

                var viewModelInfo = _GetViewModelInfoForView(viewName);

                if (viewModelInfo != null)
                {
                    var firstTime = !viewModelInfo.IsValueCreated;

                    var viewModel = viewModelInfo.Value;

                    var baseViewModel = (BaseViewModel) viewModel;

                    if (!baseViewModel.RegisteredViews.Contains(viewName))
                    {
                        baseViewModel.RegisterVisualState(viewName,
                                                          (state, transitions) =>
                                                          JounceHelper.ExecuteOnUI(
                                                              () => VisualStateManager.GoToState(view, state,
                                                                                                 transitions)));
                        _BindViewModel(view, viewModel);
                        baseViewModel.RegisteredViews.Add(viewName);
                    }

                    if (firstTime)
                    {
                        viewModel.Initialize();
                        RoutedEventHandler loaded = null;
                        loaded = (o, e) =>
                                     {
// ReSharper disable AccessToModifiedClosure
                                         ((UserControl) o).Loaded -= loaded;
// ReSharper restore AccessToModifiedClosure
                                         viewModel.Activate(viewName, parameters);
                                     };
                        view.Loaded += loaded;
                    }
                    else
                    {
                        viewModel.Activate(viewName, parameters);
                    }
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

        /// <summary>
        ///     Route a view to a view model
        /// </summary>
        /// <param name="viewModel">The view model</param>
        /// <param name="view">The view</param>
        public void RouteViewModelForView(string viewModel, string view)
        {
            _fluentRoutes.Add(ViewModelRoute.Create(viewModel, view));
        }
    }
}