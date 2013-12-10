using System.Windows.Controls;

namespace Jounce.Core.Fluent
{
    /// <summary>
    ///     Configure a view/XAP route at runtime
    /// </summary>
    /// <remarks>
    /// Use this to dynamically inform Jounce that a view with a tag lives in a separate Xap file
    /// </remarks>
    public interface IFluentViewXapRouter
    {
        /// <summary>
        /// Route a view to a XAP file
        /// </summary>
        /// <param name="view">The tag for the view</param>
        /// <param name="xap">The name of the XAP file</param>
        void RouteViewInXap(string view, string xap);

        /// <summary>
        /// Route a view to a XAP file
        /// </summary>
        /// <typeparam name="T">The type of the view (full name will be used for the tag)</typeparam>
        /// <param name="xap">The name of the XAP file</param>
        void RouteViewInXap<T>(string xap) where T : UserControl;
    }
}