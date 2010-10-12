using System.ComponentModel.Composition;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Regions.Core;
using RegionManagement.ViewModels;

namespace RegionManagement.Views
{
    /// <summary>
    ///     Shows the debug messages from the logger
    /// </summary>
    [ExportAsView(DEBUG)]
    [ExportViewToRegion(DEBUG, LocalRegions.APP_REGION)]    
    public partial class DebugView
    {
        public const string DEBUG = "Debug";
        public DebugView()
        {
            InitializeComponent();
        }

        [Export]
        public ViewModelRoute Binding
        {
            get
            {
                return ViewModelRoute.Create(DebugViewModel.DEBUG_VM, DEBUG);
            }
        }
    }
}
