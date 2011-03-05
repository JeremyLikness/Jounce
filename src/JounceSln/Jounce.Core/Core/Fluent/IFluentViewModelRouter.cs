namespace Jounce.Core.Fluent
{
    /// <summary>
    ///     Fluently configure a view model/view relationship
    /// </summary>
    public interface IFluentViewModelRouter
    {
        void RouteViewModelForView(string viewModel, string view);
    }
}