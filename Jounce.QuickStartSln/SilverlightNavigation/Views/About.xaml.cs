using Jounce.Core.View;
using Jounce.Regions.Core;

namespace SilverlightNavigation.Views
{
    [ExportAsView("About")]
    [ExportViewToRegion("About", "MainContainer")]
    public partial class About 
    {
        public About()
        {
            InitializeComponent();
        }       
    }
}