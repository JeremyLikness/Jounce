using Jounce.Core.View;
using Jounce.Regions.Core;

namespace NavigationWithBackButton.Views
{
    [ExportAsView("Lorem", Category = "Content", MenuName = "Lorem Ipsum")]
    [ExportViewToRegion("Lorem", "ContentRegion")]
    public partial class TextView
    {
        public TextView()
        {
            InitializeComponent();
        }
    }
}
