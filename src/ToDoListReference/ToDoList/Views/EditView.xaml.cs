using System.ComponentModel.Composition;
using System.Windows;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Regions.Core;
using ToDoList.ViewModels;

namespace ToDoList.Views
{
    [ExportAsView(typeof(EditView))]
    [ExportViewToRegion(typeof(EditView), Global.Constants.EDIT_REGION)]
    public partial class EditView
    {
        public EditView()
        {
            InitializeComponent();
            VisualStateManager.GoToState(this, "Closed", false);            
        }

        [Export]
        public ViewModelRoute Binding
        {
            get { return ViewModelRoute.Create<EditViewModel, EditView>(); }
        }
    }
}
