using System.ComponentModel.Composition;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Regions.Core;
using ToDoList.ViewModels;

namespace ToDoList.Views
{
    [ExportAsView(typeof(ToDoListView))]
    [ExportViewToRegion(typeof(ToDoListView), "MainRegion")]
    public partial class ToDoListView
    {
        public ToDoListView()
        {
            InitializeComponent();
        }

        [Export]
        public ViewModelRoute Binding
        {
            get { return ViewModelRoute.Create<ToDoListViewModel, ToDoListView>(); }
        }
    }
}
