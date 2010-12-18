namespace Jounce.Core.Fluent
{
    /// <summary>
    ///     Configure a view/XAP route at runtime
    /// </summary>
    public interface IFluentViewXapRouter
    {
        void RouteViewInXap(string view, string xap);
    }
}