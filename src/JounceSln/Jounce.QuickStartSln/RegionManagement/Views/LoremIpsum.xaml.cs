using Jounce.Core.View;
using Jounce.Regions.Core;

namespace RegionManagement.Views
{
    /// <summary>
    ///     This is a tab with some text that gets dynamically added to the tab control
    /// </summary>
    [ExportAsView(LOREM,MenuName = "Lorem Ipsum")]
    [ExportViewToRegion(LOREM,LocalRegions.TAB_REGION)]
    public partial class LoremIpsum
    {
        public const string LOREM = "Lorem";

        public LoremIpsum()
        {
            InitializeComponent();
        }
    }
}
