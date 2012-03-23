using System;
using Wintellect.Sterling;
using Wintellect.Sterling.Database;

namespace ToDoList.SterlingDatabase
{
    public class ConcurrencyTrigger : BaseSterlingTrigger<AuditableToDoItem,Guid>
    {
        private readonly ISterlingDatabaseInstance _database;

        public ConcurrencyTrigger(ISterlingDatabaseInstance database)
        {
            _database = database;
        }

        public override bool BeforeSave(AuditableToDoItem instance)
        {
            return true;
        }

        public override void AfterSave(AuditableToDoItem instance)
        {
            _database.Save(Concurrency.Refresh());
        }

        public override bool BeforeDelete(Guid key)
        {
            _database.Save(Concurrency.Refresh());
            return true;
        }
    }
}