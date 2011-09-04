using System.Windows.Controls;
using Jounce.Core.ViewModel;

namespace Jounce.Core.Fluent
{
    /// <summary>
    ///     Fluently configure a view model/view relationship
    /// </summary>
    public interface IFluentViewModelRouter
    {
        void RouteViewModelForView(string viewModel, string view);
        void RouteViewModelForView<T,TView>() where T: IViewModel where TView: UserControl;
    }
}