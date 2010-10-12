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

        /// <summary>
        ///     A "normal" request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            EventAggregator.Publish(new ViewNavigationArgs(Square.SQUARE));
        }

        /// <summary>
        ///     This request is for something not yet loaded - however, we give the
        ///     system a "hint" below so it can do what it needs to do
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonDynamic_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            EventAggregator.Publish(new ViewNavigationArgs("Dynamic"));
        }

        /// <summary>
        ///     Give Jounce a hint so when the view is requested, Jounce can automatically
        ///     load the needed Xap
        /// </summary>
        [Export]
        public ViewXapRoute DynamicRoute
        {
            get
            {
                return ViewXapRoute.Create("Dynamic", "RegionManagementDynamic.xap");
            }
        }
    }
}
