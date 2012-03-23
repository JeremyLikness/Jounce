using System.ComponentModel.Composition;
using System.Linq;

namespace ToDoList.Sorts
{
    public class SortList
    {
        [Export]
        public SortBase DueDate
        {
            get
            {
                return SortBase.Create("Due Date",
                                       list => list.OrderBy(t => t.IsComplete)
                                       .ThenBy(t => t.DueDate)
                                       .ThenBy(t => t.CompletedDate));
            }
        }

        [Export]
        public SortBase Title
        {
            get
            {
                return SortBase.Create("Title",
                                       list => list.OrderBy(t => t.Title));                
            }
        }

        [Export]
        public SortBase CompletedDate
        {
            get
            {
                return SortBase.Create("Completed Date",
                                       list => list.OrderByDescending(t => t.IsComplete)
                                                   .ThenBy(t => t.CompletedDate)
                                                   .ThenBy(t => t.DueDate));
            }
        }
    }
}