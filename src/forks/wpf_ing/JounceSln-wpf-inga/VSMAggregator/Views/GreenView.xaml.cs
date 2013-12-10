using Jounce.Core.View;
using Jounce.Regions.Core;

namespace VSMAggregator.Views
{
    [ExportAsView(Globals.VIEW_GREEN)]
    [ExportViewToRegion(Globals.VIEW_GREEN, Globals.REGION_MAIN)]
    public partial class GreenView
    {
        public GreenView()
        {
            InitializeComponent();
        }
    }
}
