using System.Windows.Controls;

namespace Jounce.Core.Fluent
{
    /// <summary>
    ///     Configure a view/XAP route at runtime
    /// </summary>
    public interface IFluentViewXapRouter
    {
        void RouteViewInXap(string view, string xap);
        void RouteViewInXap<T>(string xap) where T : UserControl; 
    }
}