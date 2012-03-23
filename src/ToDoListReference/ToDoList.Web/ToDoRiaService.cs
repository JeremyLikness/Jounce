
using System.ComponentModel.Composition;
using ToDoList.Contracts;
using System.Linq;
using System.ServiceModel.DomainServices.Hosting;
using System.ServiceModel.DomainServices.Server;
using System;
using ToDoList.Model;

namespace ToDoList.Web
{    
    [EnableClientAccess]
    public class ToDoRiaService : DomainService
    {        
        private class PubSubMock : IPublisher
        {
            public void Publish<T>(T message)
            {
                return;
            }
        }

        [Import]
        public IRepository Repository { get; set; }

        public ToDoRiaService()
        {
            MEFBootstrapper.SatisfyImports(this);
        }

        [Invoke]
        public Guid GetConcurrencyId()
        {
            return Repository.GetConcurrencyIdentifier;
        }

        [Invoke]
        public void MarkComplete(Guid itemId)
        {
            var task = Repository.Query().FirstOrDefault(t => t.Id.Equals(itemId));
            if (task == null || task.IsComplete) return;
            ((ToDoItem)task).PubSub = new PubSubMock();
            task.MarkComplete();
            Repository.Save(task);
        }

        [Query]
        public IQueryable<ToDoItemRia> GetTasks()
        {
            var retVal = Repository.Query().Select(item => new ToDoItemRia(item));
            return retVal.AsQueryable();
        }

        [Insert]
        public void InsertTask(ToDoItemRia item)
        {
            Repository.Save(item);            
        }

        [Update]
        public void UpdateTask(ToDoItemRia item)
        {
            Repository.Save(item);
        }

        [Delete]
        public void DeleteTask(ToDoItemRia item)
        {
            Repository.Delete(item);
        }        
    }
}


