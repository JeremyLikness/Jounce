using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.DomainServices.Client;
using Jounce.Core.Event;
using Jounce.Core.Workflow;
using Jounce.Framework;
using Jounce.Framework.Workflow;
using ToDoList.Contracts;
using ToDoList.Model;
using ToDoList.Silverlight.SterlingDatabase;
using ToDoList.Web;

namespace ToDoList.Repository
{
    [Export]
    public class WcfRiaRepository : IPartImportsSatisfiedNotification, IEventSink<MessageToDoItemComplete>
    {
        private readonly ToDoRiaContext _context = new ToDoRiaContext();
        private EntitySet<ToDoItemRia> _tasks;

        private readonly Mapper<ToDoItemRia, ToDoItem> _mapper =
            new Mapper<ToDoItemRia, ToDoItem>(
                new[] {"Id", "CompletedDate", "IsComplete"});

        private bool _tasksLoaded;
        private bool _syncRequired;
        
        [Import]
        public IServerStatus Status { get; set; }

        [Import]
        public ICacheContract Cache { get; set; }

        [Import]
        public ExportFactory<ToDoItemOverride> ToDoItemFactory { get; set; }

        [Import]
        public IEventAggregator Messaging { get; set; }

        public WcfRiaRepository()
        {
            _tasks = _context.ToDoItemRias;
        }

        /// <summary>
        /// Iterates to check whether the task list has changed
        /// </summary>
        /// <returns>A never-ending workflow to poll the server for changes</returns>
        private IEnumerable<IWorkflow> Synchronization()
        {
            var largeDelay = new WorkflowDelay(TimeSpan.FromSeconds(10));
            var smallDelay = new WorkflowDelay(TimeSpan.FromMilliseconds(100));

            // infinite loop            
            while (!GetConcurrencyIdentifier.Equals(Guid.Empty))
            {                
                // step one - get tasks from server
                while (!_tasksLoaded)
                {
                    Debug.WriteLine("*** Tasks not loaded.");
                    yield return LoadTasksAction();
                    Debug.WriteLine("*** Returned from load tasks.");
                    if (_tasksLoaded)
                    {
                        continue;
                    }
                    Status.Online = false;
                    Status.ItemsToSync = Cache.GetPendingTransactionCount();
                    Debug.WriteLine("*** Delay...");
                    yield return largeDelay;
                }

                Status.Online = true;
                Status.LastSyncDate = DateTime.Now;
                Status.ItemsToSync = Cache.GetPendingTransactionCount();

                yield return smallDelay;

                // step two - process the local transactions
                while (Cache.GetPendingTransactionCount() > 0)
                {
                    var transaction = Cache.GetNextTransaction();
                    Status.ItemsToSync = Cache.GetPendingTransactionCount();

                    Debug.WriteLine("*** Pending Transaction {0} {1}",
                                    transaction.Item.Id,
                                    transaction.Action);

                    _syncRequired = true;

                    var riaItem = GetById(transaction.Item.Id);

                    Debug.WriteLine("*** Exists on server: {0}",
                                    riaItem != null);

                    switch (transaction.Action)
                    {
                        case DatabaseAction.Delete:
                            if (riaItem != null)
                            {
                                _context.ToDoItemRias.Remove(riaItem);
                            }
                            break;
                        case DatabaseAction.MarkComplete:
                            {
                                while (_syncRequired)
                                {
                                    Debug.WriteLine("*** Mark {0} complete", transaction.Item.Id);
                                    yield return MarkCompleteAction(transaction.Item.Id);
                                    if (!_syncRequired)
                                    {
                                        continue;
                                    }
                                    Status.Online = false;
                                    Status.ItemsToSync = Cache.GetPendingTransactionCount();
                                    Debug.WriteLine("*** Delay...");
                                    yield return largeDelay;
                                }
                                break;
                            }
                        default:
                            if (riaItem == null)
                            {
                                _context.ToDoItemRias
                                    .Add(FromTask(transaction.Item));
                            }
                            else
                            {
                                FromTask(riaItem, transaction.Item);
                            }
                            break;
                    }

                    while (_syncRequired)
                    {
                        Debug.WriteLine("*** Sync required, submitting changes...");
                        yield return SubmitChangesAction();
                        if (!_syncRequired)
                        {
                            continue;
                        }
                        Status.Online = false;
                        Status.ItemsToSync = Cache.GetPendingTransactionCount();
                        Debug.WriteLine("*** Delay...");
                        yield return largeDelay;
                    }

                    // final step - purge the transaction
                    Debug.WriteLine("*** Marking transaction complete.");
                    Cache.TransactionComplete(transaction.Id);

                    yield return smallDelay;
                }

                Status.Online = true;
                Status.LastSyncDate = DateTime.Now;
                Status.ItemsToSync = Cache.GetPendingTransactionCount();

                yield return new WorkflowBackgroundWorker(bg => ProcessTransactions());

                // final step - check the concurrency
                Debug.WriteLine("*** Checking for concurrency...");
                yield return ConcurrencyAction();                

                Debug.WriteLine("*** Concurrency id: {0}", Cache.GetConcurrencyIdentifier);
                Debug.WriteLine("*** Delay...");
                yield return largeDelay;
            }
        }

        private void ProcessTransactions()
        {
         
            // step three - update the local content from the database 
            var serverTransactions =
                (from task in _tasks
                 let localCopy = task
                 where !Cache.Query()
                            .Where(
                                c =>
                                c.Id == localCopy.Id)
                            .Any()
                 select DatabaseTransaction
                     .Generate(FromRia(task),
                               DatabaseAction.Insert))
                    .ToList();

            if (serverTransactions.Count <= 0) return;

            Debug.WriteLine(
                "*** {0} objects on server not in local cache.",
                serverTransactions.Count);
            Cache.ProcessBatch(serverTransactions);
        }

        private IWorkflow MarkCompleteAction(Guid id)
        {
            IWorkflow markCompleteAction = null;
            Action markComplete =
                () =>
                _context.MarkComplete(
                    id,
                    result =>
                        {
                            if (result.HasError)
                            {
                                result.MarkErrorAsHandled();
                            }
                            else
                            {
                                _syncRequired = false;
                            }
                            markCompleteAction.Invoked();
                        }, null);
            markCompleteAction = new WorkflowAction(markComplete, false);
            return markCompleteAction;
        }

        private IWorkflow SubmitChangesAction()
        {
            IWorkflow submitChangesAction = null;
            Action submitChanges =
                () =>
                _context.SubmitChanges(
                    so =>
                        {
                            if (so.HasError)
                            {
                                so.MarkErrorAsHandled();
                            }
                            else
                            {
                                _syncRequired = false;
                            }
                            submitChangesAction.Invoked();
                        }, null);
            submitChangesAction = new WorkflowAction(
                submitChanges, false);
            return submitChangesAction;
        }

        private IWorkflow LoadTasksAction()
        {
            IWorkflow loadTasksAction = null;
            Action loadTasks =
                () =>
                _context.Load(_context.GetTasksQuery(),
                              LoadBehavior.RefreshCurrent,
                              result =>
                                  {
                                      if (result.HasError)
                                      {
                                          result.MarkErrorAsHandled();
                                      }
                                      else
                                      {
                                          _tasks = _context.ToDoItemRias;
                                          _tasksLoaded = true;
                                      }
                                      loadTasksAction.Invoked();
                                  }, null);
            loadTasksAction = new WorkflowAction(loadTasks, false);
            return loadTasksAction;
        }

        private IWorkflow ConcurrencyAction()
        {
            IWorkflow checkConcurrencyAction = null;
            Action checkConcurrency =
                () =>
                _context.GetConcurrencyId(
                    invokeOp =>
                        {
                            if (!invokeOp.HasError)
                            {
                                CheckConcurrencyAction(invokeOp.Value);
                            }
                            else
                            {
                                Status.Online = false;
                                invokeOp.MarkErrorAsHandled();
                            }
                            checkConcurrencyAction.Invoked();
                        }, null);

            checkConcurrencyAction = new WorkflowAction(checkConcurrency, false);
            return checkConcurrencyAction;
        }

        private void CheckConcurrencyAction(Guid value)
        {
            if (value.Equals(Cache.GetConcurrencyIdentifier)) return;
            Cache.SetConcurrency(value);
            _syncRequired = true;
            _tasksLoaded = false;
        }

        public Guid GetConcurrencyIdentifier
        {
            get { return Cache.GetConcurrencyIdentifier; }
        }

        private ToDoItemRia GetById(Guid id)
        {
            return (from t in _tasks
                    where t.Id.Equals(id)
                    select t).FirstOrDefault();
        }

        private IToDoItem FromRia(ToDoItemRia task)
        {
            var newTask = ToDoItemFactory.CreateExport().Value;
            _mapper.MapSourceToTarget(task, newTask);
            newTask.Id = task.Id;
            newTask.IsComplete = task.IsComplete;
            newTask.CompletedDate = task.CompletedDate;
            return newTask;
        }

        private void FromTask(ToDoItemRia riaItem, IToDoItem task)
        {
            _mapper.MapTargetToSource((ToDoItem) task, riaItem);            
        }

        private ToDoItemRia FromTask(IToDoItem task)
        {
            var newTask = new ToDoItemRia {Id = task.Id};
            FromTask(newTask, task);
            return newTask;
        }

        public void OnImportsSatisfied()
        {
            Messaging.Subscribe(this);
            JounceHelper.ExecuteOnUI(() =>
                                        WorkflowController.Begin(Synchronization()));
        }

        public void HandleEvent(MessageToDoItemComplete publishedEvent)
        {
            var todo = publishedEvent.Item;
            Cache.MarkComplete(todo);
        }
    }
}