using System.ComponentModel.Composition;
using Jounce.Core.Event;
using Jounce.Core.View;
using Jounce.Core.ViewModel;

namespace DynamicXapCalculator.Views
{
    [ExportAsView("Shell",IsShell = true)]
    public partial class Shell 
    {
        [Import]
        public IEventAggregator EventAggregator { get; set; }

        public Shell()
        {
            InitializeComponent();
        }

        [Export]
        public ViewModelRoute Binding
        {
            get
            {
                return ViewModelRoute.Create("Calc", "Shell");
            }
        }        
    }

}
