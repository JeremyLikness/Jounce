using Jounce.Core.View;
using Jounce.Regions.Core;

namespace SimpleNavigationWithRegion.Views
{
    [ExportAsView("TextView",Category="Navigation",MenuName = "Text", ToolTip = "Click to view some text.")]
    [ExportViewToRegion("TextView","ShapeRegion")]
    public partial class TextView
    {
        public TextView()
        {
            InitializeComponent();
        }
    }
}
