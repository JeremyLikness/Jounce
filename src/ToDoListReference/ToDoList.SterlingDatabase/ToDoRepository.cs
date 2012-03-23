using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using ToDoList.Contracts;
using Wintellect.Sterling;
using Wintellect.Sterling.Server.FileSystem;

namespace ToDoList.SterlingDatabase
{
    [Export(typeof(IRepository))]
    public class ToDoRepository : IRepository
    {
        private readonly SterlingEngine _engine;
        private readonly ISterlingDatabaseInstance _database;        

        public ToDoRepository()
        {
            _engine = new SterlingEngine();
            new SterlingDefaultLogger(SterlingLogLevel.Verbose);
            _engine.Activate();
            _database = _engine.SterlingDatabase
                .RegisterDatabase<ToDoDatabase>(
                new FileSystemDriver("ToDoList/"));    
            _database.RegisterTrigger(new ConcurrencyTrigger(_database));

            // set initial concurrency value
            if (_database.Load<Concurrency>(true) == null)
            {
                _database.Save(Concurrency.Refresh());
            }
        }

        private static string ResolveUser()
        {
            return Thread.CurrentPrincipal.Identity.Name;
        }

        public IQueryable<IToDoItem> Query()
        {
            var user = ResolveUser();
            var list = _database.Query<AuditableToDoItem, string, Guid>(ToDoDatabase.INDEX_BY_USER)
                .Where(index => index.Index.Equals(user,StringComparison.InvariantCultureIgnoreCase));
            return list
                .Select(item => item.LazyValue.Value)
                .Cast<IToDoItem>().AsQueryable();            
        }

        public void Save(IToDoItem item)
        {
            Debug.WriteLine(string.Format("Server Save GUID {0}", item.Id));
            var insert = true;
            var toSave = new AuditableToDoItem(item);
            var existing = _database.Load<AuditableToDoItem>(item.Id);
            if (existing != null)
            {
                toSave.Created = existing.Created;
                insert = false;
            }
            toSave.User = ResolveUser();
            toSave.Modified = DateTime.Now;
            toSave.User = ResolveUser();
            _database.Save(toSave);      
            _database.Flush();
            if (insert)
            {
                RaiseAggregateRootChanged();
            }
        }

        public void Delete(IToDoItem item)
        {
            _database.Delete(typeof(AuditableToDoItem), item.Id);
            _database.Flush();
            RaiseAggregateRootChanged();
        }

        public Guid GetConcurrencyIdentifier
        {
            get { return _database.Load<Concurrency>(true).Status; }
        }

        protected void RaiseAggregateRootChanged()
        {
            var handler = AggregateRootChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public event EventHandler AggregateRootChanged;                
    }
}