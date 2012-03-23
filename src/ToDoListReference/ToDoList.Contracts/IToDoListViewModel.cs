using System.Collections.Generic;

namespace ToDoList.Contracts
{
    public interface IToDoListViewModel
    {
        IEnumerable<IToDoItem> Tasks { get; }
    }
}