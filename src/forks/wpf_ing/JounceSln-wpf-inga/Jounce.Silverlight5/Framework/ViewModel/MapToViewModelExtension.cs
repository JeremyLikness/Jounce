using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Jounce.Core.ViewModel;

namespace Jounce.Framework.ViewModel
{
    /// <summary>
    ///     Markup extension that assembles a view model, binds to the target control
    ///     and wires in the appropriate transitions   
    /// </summary>
    /// <remarks>
    /// <example>Use this in a view to glue the view model in Xaml (for example, if the 
    /// view is instanced directly and not created through region management)
    /// </example>
    /// <code><![CDATA[ 
    /// <Grid x:Name="LayoutRoot" DataContext={jounce:MapToViewModel ViewModelName=MyViewModel>
    /// ]]>
    /// </code>
    /// </remarks>
    public class MapToViewModelExtension : MarkupExtension
    {
        /// <summary>
        /// Tag for the view model to spin up
        /// </summary>
        public string ViewModelName { get; set; }

        /// <summary>
        /// Set to true for a non-shared instance
        /// </summary>
        public bool CreateNew { get; set; }

        /// <summary>
        /// Set to true to automatically call deactivate on the 
        /// view model when unloaded
        /// </summary>
        public bool CallDeactivateOnUnload { get; set; }

        /// <summary>
        /// Instance of the <see cref="IViewModelRouter"/>
        /// </summary>
        [Import]
        public IViewModelRouter Router { get; set; }

        /// <summary>
        /// Override to provide the value requested
        /// </summary>
        /// <param name="serviceProvider">The service provider for pulling information from the Xaml parser</param>
        /// <returns>The instance of the view model if it can be resolved</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            // Ignore when in designer
            if (DesignerProperties.IsInDesignTool)
            {
                return null;
            }

            if (Router == null)
            {
                CompositionInitializer.SatisfyImports(this);
            }

            if (Router == null) return null; // not in runtime, return nothing

            var ipvt = serviceProvider.GetService(typeof(IProvideValueTarget))
                as IProvideValueTarget;

            var vm = 
                CreateNew ? 
                Router.GetNonSharedViewModel(ViewModelName) : 
                Router.ResolveViewModel<IViewModel>(true, ViewModelName);

            if (ipvt != null)
            {
                var view = ipvt.TargetObject as UserControl;

                if (view == null) return vm;

                var viewName = view.GetType().FullName;

                var baseViewModel = vm;

                if (!baseViewModel.RegisteredViews.Contains(viewName))
                {
                    baseViewModel.RegisterVisualState(viewName,
                                                      (state, transitions) =>
                                                      JounceHelper.ExecuteOnUI(
                                                          () => VisualStateManager.GoToState(view, state,
                                                                                             transitions)));
                    BindViewModel(view, baseViewModel);
                    baseViewModel.RegisteredViews.Add(viewName);
                }


                baseViewModel.Initialize();
                RoutedEventHandler loaded = null;
                loaded = (o, e) =>
                {
                    ((UserControl)o).Loaded -= loaded;
                    baseViewModel.Activate(viewName);
                };
                view.Loaded += loaded;

                if (CallDeactivateOnUnload)
                {
                    view.Unloaded += (o, e) => vm.Deactivate(viewName);
                }

            }

            return vm;
        }

        /// <summary>
        /// Method to perform the actual binding
        /// </summary>
        /// <remarks>
        /// Attempts first to bind to the layout root, otherwise binds to the view
        /// data context diretly
        /// </remarks>
        /// <param name="view">The view the markup extension is found in</param>
        /// <param name="viewModel">The view model instance</param>
        private static void BindViewModel(FrameworkElement view, IViewModel viewModel)
        {
            var root = view.FindName("LayoutRoot");
            if (root != null)
            {
                ((FrameworkElement)root).DataContext = viewModel;
            }
            else
            {
                view.Loaded += (o, e) => view.DataContext = viewModel;
            }
        }
    }   
}