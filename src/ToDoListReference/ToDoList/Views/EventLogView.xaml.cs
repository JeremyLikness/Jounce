using System.ComponentModel.Composition;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using ToDoList.ViewModels;

namespace ToDoList.Views
{
    [ExportAsView(typeof(EventLogView))]
    public partial class EventLogView
    {
        public EventLogView()
        {
            InitializeComponent();
        }

        [Export]
        public ViewModelRoute Binding
        {
            get { return ViewModelRoute.Create<EventLogViewModel, EventLogView>(); }
        }
    }
}
