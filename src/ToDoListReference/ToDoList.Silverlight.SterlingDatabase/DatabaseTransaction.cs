using System;
using ToDoList.Contracts;
using ToDoList.Model;

namespace ToDoList.Silverlight.SterlingDatabase
{
    public class DatabaseTransaction
    {     
        public class ToDoItemReference : ToDoItemOverride
        {
            public ToDoItemReference(IToDoItem item)
            {
                CompletedDate = item.CompletedDate;
                Description = item.Description;
                DueDate = item.DueDate;
                Id = item.Id;
                IsComplete = item.IsComplete;
                Link = item.Link;
                Title = item.Title;
            }

            public ToDoItemReference()
            {
                
            }
        }

        public Guid Id { get; set; }
        public ToDoItemReference Item { get; set; }
        public long TransactionTime { get; set; }
        public DatabaseAction Action { get; set; }

        public static DatabaseTransaction Generate(
            IToDoItem item, DatabaseAction action)
        {
            return new DatabaseTransaction
                        {
                            Id = Guid.NewGuid(),
                            Item = new ToDoItemReference(item),
                            TransactionTime = DateTime.Now.Ticks,
                            Action = action
                        };
        }
    }
}