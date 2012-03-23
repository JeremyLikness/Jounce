using System;
using System.Windows.Input;
using ToDoList.Contracts;
using ToDoList.Model;

namespace ToDoList.SampleData
{
#if DEBUG
    public class DesignToDoListItemViewModel : IToDoListItemViewModel 
    {
        public string ToolTip
        {
            get { return "Design-time task"; }
        }

        public string DateLabel
        {
            get { return "Due Date:"; }
        }

        public DateTime KeyDate
        {
            get { return DateTime.Now.Date.AddDays(1); }
        }

        public ICommand DeleteCommand { get; set; }
        public ICommand MarkCompleteCommand { get; set; }
        public ICommand EditCommand { get; set; }

        public IToDoItem Task
        {
            get
            {
                var item = new ToDoItem
                               {
                                   Description = "This is an example of a design-time task.",
                                   DueDate = DateTime.Now.Date.AddDays(1),
                                   Title = "Design-time Task"
                               };
                return item;
            }
        }
    }
#endif
}