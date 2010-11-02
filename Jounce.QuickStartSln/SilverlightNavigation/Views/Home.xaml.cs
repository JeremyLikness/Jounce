using Jounce.Core.View;
using Jounce.Regions.Core;

namespace SilverlightNavigation.Views
{
    [ExportAsView("Home")]
    [ExportViewToRegion("Home","MainContainer")]
    public partial class Home
    {
        public Home()
        {
            InitializeComponent();
        }      
    }
}