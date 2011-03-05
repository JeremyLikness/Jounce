namespace Jounce.Core.View
{
    /// <summary>
    ///     The view locator element - maps views to dynamically loaded xaps
    /// </summary>
    public class ViewXapRoute
    {
        private ViewXapRoute()
        {
        }
        
        public static ViewXapRoute Create(string viewName, string viewXap)
        {
            return new ViewXapRoute {ViewName = viewName, ViewXap = viewXap};
        }

        /// <summary>
        ///     Name of the view
        /// </summary>
        public string ViewName { get; private set; }

        /// <summary>
        ///     The xap file the view lives in
        /// </summary>
        public string ViewXap { get; private set; }

        public override string ToString()
        {
            return string.Format(Resources.ViewXapRoute_ToString_View_Route_View, ViewName, ViewXap);
        }
    }
}