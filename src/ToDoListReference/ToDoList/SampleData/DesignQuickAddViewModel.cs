using System;
using System.Windows.Input;
using ToDoList.Contracts;

namespace ToDoList.SampleData
{
#if DEBUG 
    public class DesignQuickAddViewModel : IQuickAddViewModel 
    {
        public string DueDate
        {
            get { return DateTime.Now.ToString(); }
        }

        public string Title
        {
            get { return "Design-time Task"; }
        }

        public ICommand CommitCommand
        {
            get { return null; }
        }
    }
#endif 
}