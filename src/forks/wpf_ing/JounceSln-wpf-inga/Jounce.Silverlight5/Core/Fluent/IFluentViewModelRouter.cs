using System.Windows.Controls;
using Jounce.Core.ViewModel;

namespace Jounce.Core.Fluent
{
    /// <summary>
    ///     Fluently configure a view model/view relationship
    /// </summary>
    /// <remarks>
    /// Use this to configure view and view model routes at runtime
    /// </remarks>
    public interface IFluentViewModelRouter
    {
        /// <summary>
        /// Route a view and a view model, so that when a view is navigated to, the
        /// corresponding view model should be bound to it
        /// </summary>
        /// <param name="viewModel">The tag for the view model</param>
        /// <param name="view">The tag for the view</param>
        void RouteViewModelForView(string viewModel, string view);

        /// <summary>
        /// Route a view to a view model, so that when the view is navigated to, the
        /// corresponding view model should be bound to it
        /// </summary>
        /// <typeparam name="T">The type of the view model (fullname will be used as the tag)</typeparam>
        /// <typeparam name="TView">The tye of the view (full name will be used as the tag)</typeparam>
        void RouteViewModelForView<T,TView>() where T: IViewModel where TView: UserControl;
    }
}