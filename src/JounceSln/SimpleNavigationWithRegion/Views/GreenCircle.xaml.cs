using Jounce.Core.View;
using Jounce.Regions.Core;

namespace SimpleNavigationWithRegion.Views
{
    [ExportAsView("GreenCircle",Category="Navigation",MenuName="Circle",ToolTip = "Click to view a green circle.")]
    [ExportViewToRegion("GreenCircle","ShapeRegion")]
    public partial class GreenCircle
    {
        public GreenCircle()
        {
            InitializeComponent();
        }
    }
}
