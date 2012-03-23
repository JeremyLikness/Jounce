using System.ComponentModel.Composition;
using ToDoList.ViewModels;
using ToDoList.Views;

namespace ToDoList
{
    [Export]
    public class Global
    {
        public static class Constants
        {
            public const string QUICK_ADD_REGION = "QuickAddRegion";
            public const string FILTER_REGION = "FilterRegion";
            public const string TEXT_FILTER_REGION = "TextFilterRegion";
            public const string SORT_REGION = "SortRegion";
            public const string STATUS_REGION = "StatusRegion";
            public const string EDIT_REGION = "EditRegion";
            public const string MAIN_TITLE = "To-Do List: Designing Silverlight Business Applications";
            public const string NEW_TITLE = "Add New To-Do List Item";
            public const string EDIT_TITLE = "Edit To-Do List Item: {0}";
            public const string PARM_STATE = "State";
            public const string PARM_ITEM = "Item";
            public const string STATE_CLOSED = "Closed";
            public const string STATE_NEW = "New";
            public const string STATE_EDIT = "Edit";
            public const string PARM_WINDOW = "ToDo.OpenWindow";
            public const string PARM_WIDTH = "Width";
            public const string PARM_HEIGHT = "Height";
            public const string PARM_TITLE = "Title";
            public const string XAP_PATH = "XAPPath";
            public const string REPORT_TASKGRID = "TaskGridReport";            
        }

        public readonly string StatusView = typeof (StatusView).FullName;
        public readonly string QuickAddView = typeof (QuickAddView).FullName;
        public readonly string FilterView = typeof (FilterView).FullName;
        public readonly string ToDoListView = typeof (ToDoListView).FullName;
        public readonly string TextFilterView = typeof (TextFilterView).FullName;
        public readonly string SortView = typeof (SortView).FullName;
        public readonly string EditView = typeof (EditView).FullName;
        public readonly string OobView = typeof (OutOfBrowser).FullName;
        public readonly string UpdateView = typeof (Update).FullName;
        public readonly string EventLogView = typeof (EventLogView).FullName;
        public readonly string ToDoListViewModel = typeof (ToDoListViewModel).FullName;
    }
}