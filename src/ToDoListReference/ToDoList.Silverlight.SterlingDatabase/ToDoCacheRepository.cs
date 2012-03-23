using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using ToDoList.Contracts;
using ToDoList.Model;
using Wintellect.Sterling;
using Wintellect.Sterling.IsolatedStorage;

namespace ToDoList.Silverlight.SterlingDatabase
{
    public class ToDoCacheRepository : ICacheContract
    {
        private readonly SterlingEngine _engine;
        private readonly ISterlingDatabaseInstance _database;
                
        public ToDoCacheRepository()
        {
            _engine = new SterlingEngine();
            new SterlingDefaultLogger(SterlingLogLevel.Verbose);
            _engine.Activate();
            _database = _engine.SterlingDatabase
                .RegisterDatabase<ToDoListCache>(new IsolatedStorageDriver());
        }

        public IQueryable<IToDoItem> Query()
        {
            return _database.Query<ToDoItemOverride, Guid>()
                .Select(item => item.LazyValue.Value)
                .Cast<IToDoItem>().AsQueryable();
        }

        public void Save(IToDoItem item)
        {
            Debug.WriteLine("Client Save GUID {0}", item.Id);
            var existing = (from t in _database.Query<ToDoItemOverride, Guid>()
                            where t.Key == item.Id
                            select t.Key).Count() > 0;

            var dbItem = FromInterface(item);

            _database.Save(dbItem);

            var transaction = DatabaseTransaction
                .Generate(dbItem, existing
                                        ? DatabaseAction.Update
                                        : DatabaseAction.Insert);

            _database.Save(transaction);
            _database.Flush();
            if (!existing)
            {
                RaiseAggregateRootChanged();
            }
        }

        private static ToDoItemOverride FromInterface(IToDoItem item)
        {
            return new ToDoItemOverride
                       {
                           CompletedDate = item.CompletedDate,
                           Description = item.Description,
                           DueDate = item.DueDate,
                           Id = item.Id,
                           IsComplete = item.IsComplete,
                           Link = item.Link,
                           Title = item.Title
                       };
        }

        public void Delete(IToDoItem item)
        {
            _database.Delete(typeof (ToDoItemOverride), item.Id);

            var transaction = DatabaseTransaction
                .Generate(item, DatabaseAction.Delete);

            _database.Save(transaction);

            _database.Flush();
            RaiseAggregateRootChanged();
        }

        private const string SYNC_GUID = "SyncGuid";
                
        public Guid GetConcurrencyIdentifier
        {
            get
            {
                var settings = IsolatedStorageSettings.ApplicationSettings;
                return settings.Contains(SYNC_GUID)
                            ? (Guid) settings[SYNC_GUID]
                            : Guid.NewGuid();
            }            
        }

        public void SetConcurrency(Guid id)
        {
            IsolatedStorageSettings.ApplicationSettings[SYNC_GUID] = id;
        }
        
        protected void RaiseAggregateRootChanged()
        {
            var handler = AggregateRootChanged;
            if (handler != null)
            {
                Action action = () => handler(this, EventArgs.Empty);
                var dispatcher = Deployment.Current.Dispatcher;
                if (dispatcher.CheckAccess())
                {
                    action();
                }
                else
                {
                    dispatcher.BeginInvoke(action);
                }
            }
        }

        public event EventHandler AggregateRootChanged;
        
        public int GetPendingTransactionCount()
        {
            var count = _database.Query<DatabaseTransaction, Guid>()
                .Count();
            return count;
        }

        public DatabaseTransaction GetNextTransaction()
        {
            var oldest =
                (from d in
                        _database.Query<DatabaseTransaction, long, Guid>(
                            ToDoListCache.TRANSACTION_IDX)
                    orderby d.Index
                    select d).FirstOrDefault();
            if (oldest == null)
            {
                return null;
            }
            return oldest.LazyValue.Value;
        }

        public void TransactionComplete(Guid id)
        {
            _database.Delete(typeof(DatabaseTransaction), id);
            _database.Flush();
        }

        public void ProcessBatch(IEnumerable<DatabaseTransaction> actions)
        {
            foreach (var action in actions)
            {
                switch(action.Action)
                {
                    case DatabaseAction.Delete:
                        _database.Delete(typeof(ToDoItemOverride), action.Item.Id);
                        break;
                    default:
                        _database.Save(FromInterface(action.Item));
                        break;                    
                }
            }

            _database.Flush();
            RaiseAggregateRootChanged();
        }

        public void MarkComplete(IToDoItem item)
        {
            var existing = _database.Load<ToDoItemOverride>(item.Id);
            if (existing != null)
            {
                existing.CompletedDate = item.CompletedDate;
                existing.IsComplete = item.IsComplete;
                _database.Save(existing);
                _database.Save(DatabaseTransaction.Generate(existing, DatabaseAction.MarkComplete));
                _database.Flush();
            }
        }
    }
}