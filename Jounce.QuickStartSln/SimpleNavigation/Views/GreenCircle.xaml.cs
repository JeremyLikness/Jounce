using Jounce.Core.View;

namespace SimpleNavigation.Views
{
    [ExportAsView("GreenCircle",Category="Navigation",CommandName="Circle",ToolTip = "Click to view a green circle.")]
    public partial class GreenCircle
    {
        public GreenCircle()
        {
            InitializeComponent();
        }
    }
}
