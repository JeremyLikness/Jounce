using Jounce.Core.View;
using Jounce.Regions.Core;

namespace RegionManagementDynamic
{
    [ExportAsView("Dynamic")]
    [ExportViewToRegion("Dynamic","DynamicRegion")]
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
    }
}
