using System.ComponentModel.Composition;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Regions.Core;
using ToDoList.ViewModels;

namespace ToDoList.Views
{
    [ExportAsView(typeof(TextFilterView))]
    [ExportViewToRegion(typeof(TextFilterView), Global.Constants.TEXT_FILTER_REGION)]
    public partial class TextFilterView
    {
        public TextFilterView()
        {
            InitializeComponent();
        }

        [Export]
        public ViewModelRoute Binding
        {
            get { return ViewModelRoute.Create<TextFilterViewModel, TextFilterView>(); }
        }
    }
}
