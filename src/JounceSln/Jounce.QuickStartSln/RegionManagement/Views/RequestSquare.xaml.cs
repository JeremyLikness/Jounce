using System.ComponentModel.Composition;
using Jounce.Core.Event;
using Jounce.Core.View;
using Jounce.Regions.Core;

namespace RegionManagement.Views
{    
    /// <summary>
    ///     Request a square
    /// </summary>
    [ExportAsView(REQUEST_SQUARE, MenuName = "Request a Square")]
    [ExportViewToRegion(REQUEST_SQUARE,LocalRegions.TAB_REGION)]
    public partial class RequestSquare
    {
        public const string REQUEST_SQUARE = "RequestSquare";

        [Import]
        public IEventAggregator EventAggregator { get; set; }

        public RequestSquare()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            EventAggregator.Publish(new ViewNavigationArgs(Square.SQUARE));
        }
    }
}
