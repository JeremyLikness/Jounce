using System;
using System.Collections.Generic;
using ToDoList.Model;
using Wintellect.Sterling.Database;

namespace ToDoList.Silverlight.SterlingDatabase
{
    public class ToDoListCache : BaseDatabaseInstance
    {
        public const string TRANSACTION_IDX = "TransactionIndex";

        protected override List<ITableDefinition> RegisterTables()
        {
            return new List<ITableDefinition>
                        {
                            CreateTableDefinition<ToDoItemOverride, Guid>(t => t.Id),
                            CreateTableDefinition<DatabaseTransaction, Guid>(d => d.Id)
                                .WithIndex<DatabaseTransaction, long, Guid>(
                                    TRANSACTION_IDX, d => d.TransactionTime)
                        };
        }
    }
}