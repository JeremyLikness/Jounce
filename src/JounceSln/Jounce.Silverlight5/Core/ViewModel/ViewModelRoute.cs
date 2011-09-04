using System.Windows.Controls;

namespace Jounce.Core.ViewModel
{
    /// <summary>
    ///     A route binds a view to a view model
    /// </summary>
    public class ViewModelRoute
    {
        private ViewModelRoute()
        {
        }

        /// <summary>
        ///     Safely typed creation
        /// </summary>
        /// <typeparam name="TViewModel">The view model</typeparam>
        /// <typeparam name="TView">The view</typeparam>
        /// <returns>The route</returns>
        public static ViewModelRoute Create<TViewModel, TView>() where TViewModel : IViewModel where TView : UserControl
        {
            return new ViewModelRoute {ViewModelType = typeof (TViewModel).FullName, ViewType = typeof (TView).FullName};
        }

        public static ViewModelRoute Create(string viewModelType, string viewType)            
        {
            return new ViewModelRoute { ViewModelType = viewModelType, ViewType = viewType };
        }

        public string ViewModelType { get; private set; }

        public string ViewType { get; private set; }

        public override string ToString()
        {
            return string.Format(Resources.ViewModelRoute_ToString_ViewModelRoute, ViewModelType, ViewType);
        }
    }
}