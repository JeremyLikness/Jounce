using System.ComponentModel.Composition;
using System.Windows.Navigation;
using Jounce.Core.Event;
using Jounce.Core.View;
using Jounce.Framework;

namespace SilverlightNavigation.Views
{
    /// <summary>
    ///     Main frame for navigation
    /// </summary>
    public partial class JounceNavigation
    {
        /// <summary>
        ///     Event aggregator
        /// </summary>
        [Import]
        public IEventAggregator EventAggregator { get; set; }

        /// <summary>
        ///     Navigation container (holds the region for target views)
        /// </summary>
        [Import]
        public NavigationContainer NavContainer { get; set; }

        private static string _lastView = string.Empty;
                
        public JounceNavigation()
        {
            InitializeComponent();
            CompositionInitializer.SatisfyImports(this);
            LayoutRoot.Children.Add(NavContainer);
            if (!string.IsNullOrEmpty(_lastView)) return;
            EventAggregator.Publish("Home".AsViewNavigationArgs());
            _lastView = "Home";
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("view"))
            {
                var newView = NavigationContext.QueryString["view"];
                _lastView = newView;
                EventAggregator.Publish(_lastView.AsViewNavigationArgs()); 
                EventAggregator.Publish(NavigationContext);
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (!string.IsNullOrEmpty(_lastView))
            {
                EventAggregator.Publish(new ViewNavigationArgs(_lastView) {Deactivate = true});
            }
            LayoutRoot.Children.Remove(NavContainer);
        }

    }
}
