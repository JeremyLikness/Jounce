using Jounce.Core.View;

namespace SimpleNavigation.Views
{
    [ExportAsView("TextView",Category="Navigation",CommandName = "Text", ToolTip = "Click to view some text.")]
    public partial class TextView
    {
        public TextView()
        {
            InitializeComponent();
        }
    }
}
