using Jounce.Core.View;
using Jounce.Regions.Core;

namespace VSMAggregator.Views
{
    [ExportAsView(Globals.VIEW_BLUE)]
    [ExportViewToRegion(Globals.VIEW_BLUE, Globals.REGION_MAIN)]
    public partial class BlueView
    {
        public BlueView()
        {
            InitializeComponent();
        }
    }
}
