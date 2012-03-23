using System.ComponentModel.Composition;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Regions.Core;
using ToDoList.ViewModels;

namespace ToDoList.Views
{
    [ExportAsView(typeof(FilterView))]
    [ExportViewToRegion(typeof(FilterView), Global.Constants.FILTER_REGION)]        
    public partial class FilterView
    {
        public FilterView()
        {
            InitializeComponent();            
        }

        [Export]
        public ViewModelRoute Binding
        {
            get { return ViewModelRoute.Create<FilterViewModel, FilterView>(); }
        }
    }
}
