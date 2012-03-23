using System;
using System.Linq;

namespace ToDoList.Contracts
{
    public interface IRepository
    {
        IQueryable<IToDoItem> Query();
        void Save(IToDoItem item);
        void Delete(IToDoItem item);        
        event EventHandler AggregateRootChanged;
        Guid GetConcurrencyIdentifier { get; }
    }
}