using System.ComponentModel.Composition;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Regions.Core;
using ToDoList.ViewModels;

namespace ToDoList.Views
{
    [ExportAsView(typeof(SortView))]
    [ExportViewToRegion(typeof(SortView),Global.Constants.SORT_REGION)]
    public partial class SortView
    {
        public SortView()
        {
            InitializeComponent();
        }

        [Export]
        public ViewModelRoute Binding
        {
            get { return ViewModelRoute.Create<SortViewModel,SortView>(); }
        }
    }
}
