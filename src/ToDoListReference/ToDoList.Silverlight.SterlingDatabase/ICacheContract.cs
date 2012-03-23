using System;
using System.Collections.Generic;
using ToDoList.Contracts;

namespace ToDoList.Silverlight.SterlingDatabase
{
    public interface ICacheContract : IRepository
    {
        void SetConcurrency(Guid id);
        int GetPendingTransactionCount();
        DatabaseTransaction GetNextTransaction();
        void TransactionComplete(Guid id);
        void ProcessBatch(IEnumerable<DatabaseTransaction> actions);
        void MarkComplete(IToDoItem item);
    }
}