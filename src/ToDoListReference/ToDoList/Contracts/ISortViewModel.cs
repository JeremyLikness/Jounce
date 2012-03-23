using System.Collections.Generic;
using Jounce.Core.Command;
using ToDoList.Sorts;

namespace ToDoList.Contracts
{
    public interface ISortViewModel
    {
        IActionCommand<SortBase> SortCommand { get; }
        IEnumerable<SortBase> Sorts { get; }
    }
}