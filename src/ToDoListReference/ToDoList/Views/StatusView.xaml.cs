using System.ComponentModel.Composition;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Regions.Core;
using ToDoList.ViewModels;

namespace ToDoList.Views
{
    [ExportAsView(typeof(StatusView))]
    [ExportViewToRegion(typeof(StatusView),Global.Constants.STATUS_REGION)]
    public partial class StatusView
    {
        public StatusView()
        {
            InitializeComponent();
        }

        [Export]
        public ViewModelRoute Binding
        {
            get { return ViewModelRoute.Create<ServerStatusViewModel, StatusView>(); }
        }
    }
}
