using System.Windows.Controls;

namespace Jounce.Core.View
{
    /// <summary>
    ///     The view locator element - maps views to dynamically loaded xaps
    /// </summary>
    /// <remarks>
    /// With this route, Jounce is informed that a view lives in another XAP file and 
    /// can automatically download the XAP prior to navigation
    /// </remarks>
    public class ViewXapRoute
    {
        /// <summary>
        /// Supress public creation
        /// </summary>
        private ViewXapRoute()
        {
        }

        /// <summary>
        /// Create a route using a view tag and the name of the XAP
        /// </summary>
        /// <param name="viewName">The tag for the view</param>
        /// <param name="viewXap">The name of the XAP file</param>
        /// <returns>A new instance of the route</returns>
        public static ViewXapRoute Create(string viewName, string viewXap)
        {
            return new ViewXapRoute {ViewName = viewName, ViewXap = viewXap};
        }

        /// <summary>
        /// Create a route using the full name of the type for the view as the tag and the name of the XAP
        /// </summary>
        /// <typeparam name="T">The type of the view</typeparam>
        /// <param name="viewXap">The name of the XAP file</param>
        /// <returns>A new instance of the route</returns>
        public static ViewXapRoute Create<T>(string viewXap) where T: UserControl 
        {
            return new ViewXapRoute { ViewName = typeof(T).FullName, ViewXap = viewXap };
        }
        
        /// <summary>
        /// Tag for the view
        /// </summary>
        public string ViewName { get; private set; }

        /// <summary>
        /// The xap file the view lives in
        /// </summary>
        public string ViewXap { get; private set; }

        /// <summary>
        /// String representation
        /// </summary>
        /// <returns>The string representation</returns>
        public override string ToString()
        {
            return string.Format(Resources.ViewXapRoute_ToString_View_Route_View, ViewName, ViewXap);
        }
    }
}