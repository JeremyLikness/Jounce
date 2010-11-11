using Jounce.Core.View;
using Jounce.Regions.Core;

namespace SimpleNavigationWithRegion.Views
{
    [ExportAsView("RedSquare",Category="Navigation",MenuName = "Square",ToolTip = "Click to view a red square.")]
    [ExportViewToRegion("RedSquare", "ShapeRegion")]
    public partial class RedSquare
    {
        public RedSquare()
        {
            InitializeComponent();
        }
    }
}
