using Jounce.Core.View;

namespace SimpleNavigation.Views
{
    [ExportAsView("RedSquare",Category="Navigation",CommandName = "Square",ToolTip = "Click to view a red square.")]
    public partial class RedSquare
    {
        public RedSquare()
        {
            InitializeComponent();
        }
    }
}
