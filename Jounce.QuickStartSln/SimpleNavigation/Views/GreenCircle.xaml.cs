using System.ComponentModel.Composition;
using Jounce.Core.View;
using Jounce.Core.ViewModel;

namespace SimpleNavigation.Views
{
    [ExportAsView("GreenCircle",Category="Navigation",MenuName="Circle",ToolTip = "Click to view a green circle.")]
    public partial class GreenCircle
    {
        public GreenCircle()
        {
            InitializeComponent();
        }

        [Export]
        public ViewModelRoute Binding
        {
            get { return ViewModelRoute.Create("GreenCircle", "GreenCircle"); }
        }
    }    
}
