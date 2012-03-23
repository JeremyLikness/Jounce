using System;
using System.Collections.Generic;
using Wintellect.Sterling.Database;

namespace ToDoList.SterlingDatabase
{
    public class ToDoDatabase : BaseDatabaseInstance
    {
        public const string INDEX_BY_USER = "IndexByUser";        

        protected override List<ITableDefinition> RegisterTables()
        {
            return
                new List<ITableDefinition>
                    {
                        CreateTableDefinition<AuditableToDoItem, 
                        Guid>(
                            todo => todo.Id)
                            .WithIndex<AuditableToDoItem, string, 
                            Guid>(
                                INDEX_BY_USER, todo => todo.User),
                        CreateTableDefinition<Concurrency,bool>(c=>true)
                    };
        }        
    }
}