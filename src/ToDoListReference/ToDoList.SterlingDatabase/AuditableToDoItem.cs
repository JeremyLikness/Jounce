using System;
using ToDoList.Contracts;
using ToDoList.Model;

namespace ToDoList.SterlingDatabase
{
    public class AuditableToDoItem : ToDoItem
    {
        public AuditableToDoItem()
        {            
        }

        public AuditableToDoItem(IToDoItem item)
        {
            Id = item.Id;
            CompletedDate = item.CompletedDate;
            Description = item.Description;
            DueDate = item.DueDate;
            IsComplete = item.IsComplete;
            Link = item.Link;
            Title = item.Title;
            Created = DateTime.Now;
        }

        public new bool IsComplete
        {
            get { return base.IsComplete; }
            set { base.IsComplete = value; }
        }
        public new DateTime CompletedDate
        {
            get { return base.CompletedDate; }
            set { base.CompletedDate = value; }
        }
        public string User { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}