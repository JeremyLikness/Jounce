using Jounce.Core.View;
using Jounce.Regions.Core;

namespace RegionManagement.Views
{
    /// <summary>
    ///     A green square
    /// </summary>
    [ExportAsView(SQUARE)]
    [ExportViewToRegion(SQUARE,LocalRegions.APP_REGION)]
    public partial class Square
    {
        public const string SQUARE = "Square";

        public Square()
        {
            InitializeComponent();
        }
    }
}
