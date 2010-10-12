using System.ComponentModel.Composition;
using System.Windows.Controls;
using Jounce.Core.Event;
using Jounce.Core.View;
using Jounce.Regions.Core;

namespace RegionManagement.Views
{
    /// <summary>
    ///     This view is used to click and request the circle view to display
    /// </summary>
    [ExportAsView(REQCIRCLE,MenuName = "Request a Circle")]
    [ExportViewToRegion(REQCIRCLE,LocalRegions.TAB_REGION)]
    public partial class RequestCircle
    {
        public const string REQCIRCLE = "RequestCircle";

        [Import]
        public IEventAggregator EventAggregator { get; set; }

        public RequestCircle()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // show the circle
            EventAggregator.Publish(new ViewNavigationArgs(Circle.CIRCLE));
        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            // dynamically add the lorem tab and then disable the button
            EventAggregator.Publish(new ViewNavigationArgs(LoremIpsum.LOREM));
            ((Button) sender).IsEnabled = false;
        }
    }
}
