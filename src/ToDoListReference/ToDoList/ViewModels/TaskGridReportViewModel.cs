using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Controls;
using System.Windows.Printing;
using Jounce.Core.ViewModel;
using ToDoList.Contracts;

namespace ToDoList.ViewModels
{    
    [Export(typeof(ITaskGridReport))]
    public class TaskGridReportViewModel : BaseViewModel, 
        ITaskGridReportViewModel, ITaskGridReport
    {
        private const int TASKS_PER_PAGE = 48;

        [Import]
        public Global Globals { get; set; }

        [Import]
        public IToDoListApplicationContext AppContext { get; set; }

        public DateTime PrintDate { get; set; }
        public string UserName { get; set; }
        public int TotalPages { get; set;  }
        public int Page { get; set; }
        public IEnumerable<IToDoItem> Tasks { get; set; }        
        
        private int _taskPosition;
        private List<IToDoItem> _tasks;
        private UserControl _view;
        
        public void ProcessReport()
        {
            ProcessUserName();            
            PrintDate = DateTime.Now;                        
            var tasksViewModel = Router.ResolveViewModel(Globals.ToDoListViewModel) as IToDoListViewModel;
            if (tasksViewModel == null) return;
            _tasks = tasksViewModel.Tasks.ToList();
            _taskPosition = 0;
            Page = 1;
            TotalPages = (_tasks.Count()/TASKS_PER_PAGE) + 1;                
            var printDoc = new PrintDocument();
            printDoc.PrintPage += PrintDocPrintPage;
            printDoc.Print("Task List");
        }

        void PrintDocPrintPage(object sender, PrintPageEventArgs e)
        {
            if (_view == null)
            {
                _view = Router.GetNonSharedView(Global.Constants.REPORT_TASKGRID, this);
            }

            var tasks = new List<IToDoItem>();
            for (var x = 0; x < TASKS_PER_PAGE && _taskPosition < _tasks.Count; x++)
            {
                tasks.Add(_tasks[_taskPosition++]);
            }
            Tasks = tasks;
            RaisePropertyChanged(()=>Page);
            RaisePropertyChanged(()=>Tasks);
            e.PageVisual = _view;
            if (_taskPosition < _tasks.Count)
            {
                Page++;
                e.HasMorePages = true;
            }
            else
            {
                _tasks.Clear();        
            }
        }

        private void ProcessUserName()
        {
            if (AppContext.IsRunningOutOfBrowser)
            {
                var size = 64;
                var sb = new StringBuilder(64);
                if (GetUserName(sb, ref size))
                {
                    UserName = sb.ToString();
                }
            }
            else
            {
                UserName = "Web User";
            }
        }

        [DllImport("Advapi32.dll")]
        private static extern bool GetUserName(StringBuilder lpBuffer, ref int nSize);

    }
}