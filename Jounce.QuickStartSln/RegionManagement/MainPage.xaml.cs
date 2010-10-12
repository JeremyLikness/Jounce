using System.ComponentModel.Composition;
using Jounce.Core.Event;
using Jounce.Core.View;
using RegionManagement.Views;

namespace RegionManagement
{
    /// <summary>
    ///     Main view
    /// </summary>
    [ExportAsView(MAIN,IsShell=true)]
    public partial class MainPage : IPartImportsSatisfiedNotification
    {
        public const string MAIN = "Main";

        [Import]
        public IEventAggregator EventAggregator { get; set; }        

        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called when a part's imports have been satisfied and it is safe to use.
        /// </summary>
        public void OnImportsSatisfied()
        {
            // activate debug view
            EventAggregator.Publish(new ViewNavigationArgs(DebugView.DEBUG));

            // these are targetted to a tab control and only get wired in when activated, so we'll
            // pre-load them in the order we want
            EventAggregator.Publish(new ViewNavigationArgs(RequestSquare.REQUEST_SQUARE));
            EventAggregator.Publish(new ViewNavigationArgs(RequestCircle.REQCIRCLE));
            
            // now let's activate the square again (shift the focus of the tab)
            EventAggregator.Publish(new ViewNavigationArgs(RequestSquare.REQUEST_SQUARE));
        }
    }
}
