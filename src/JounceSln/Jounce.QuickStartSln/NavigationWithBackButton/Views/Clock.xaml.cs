using Jounce.Core.View;
using Jounce.Regions.Core;

namespace NavigationWithBackButton.Views
{
    [ExportAsView("Clock", Category="Content", MenuName = "Big Clock")]
    [ExportViewToRegion("Clock", "ContentRegion")]
    public partial class Clock
    {
        public Clock()
        {
            InitializeComponent();
        }
    }
}
