using System.ComponentModel.Composition;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Regions.Core;
using ToDoList.ViewModels;

namespace ToDoList.Views
{
    [ExportAsView(typeof(QuickAddView))]
    [ExportViewToRegion(typeof(QuickAddView),
        Global.Constants.QUICK_ADD_REGION)]
    public partial class QuickAddView
    {
        public QuickAddView()
        {
            InitializeComponent();
        }

        [Export]
        public ViewModelRoute Binding
        {
            get { return ViewModelRoute
                .Create<QuickAddViewModel, QuickAddView>(); }
        }
    }
}
