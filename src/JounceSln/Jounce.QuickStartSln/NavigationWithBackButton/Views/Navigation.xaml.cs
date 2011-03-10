using Jounce.Core.View;
using Jounce.Regions.Core;

namespace NavigationWithBackButton.Views
{
    [ExportAsView("Navigation")]
    [ExportViewToRegion("Navigation", "NavigationRegion")]
    public partial class Navigation
    {
        public Navigation()
        {
            InitializeComponent();
        }
    }
}
