using System.Collections.Generic;

namespace ToDoList.Contracts
{
    public interface IExportAs
    {
        void Export(IEnumerable<IToDoItem> items);
    }
}