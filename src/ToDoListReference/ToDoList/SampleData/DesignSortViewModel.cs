using System.Collections.Generic;
using Jounce.Core.Command;
using ToDoList.Contracts;
using ToDoList.Sorts;

namespace ToDoList.SampleData
{
#if DEBUG
    public class DesignSortViewModel : ISortViewModel 
    {
        public IActionCommand<SortBase> SortCommand { get; set; }

        public IEnumerable<SortBase> Sorts
        {
            get
            {
                return new[]
                           {
                               SortBase.Create("Date", l => l),
                               SortBase.Create("Title", l => l)
                           };
            }
        }
    }
#endif
}