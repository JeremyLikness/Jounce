using Jounce.Core.View;
using Jounce.Regions.Core;

namespace VSMAggregator.Views
{
    [ExportAsView(Globals.VIEW_RED)]
    [ExportViewToRegion(Globals.VIEW_RED, Globals.REGION_MAIN)]
    public partial class RedView
    {
        public RedView()
        {
            InitializeComponent();
        }
    }
}
