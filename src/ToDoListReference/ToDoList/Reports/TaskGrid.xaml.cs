using Jounce.Core.View;

namespace ToDoList.Reports
{
    [ExportAsView(Global.Constants.REPORT_TASKGRID)]
    public partial class TaskGrid
    {
        public TaskGrid()
        {
            InitializeComponent();
        }        
    }
}
