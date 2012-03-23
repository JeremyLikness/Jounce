using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;
using ToDoList.Contracts;

namespace ToDoList.Web
{
    public class ToDoItemRia : IToDoItem 
    {
        public ToDoItemRia()
        {
            Id = Guid.NewGuid();
        }

        public ToDoItemRia(IToDoItem item)
        {
            Id = item.Id;
            Title = item.Title;
            Description = item.Description;
            DueDate = item.DueDate;
            CompletedDate = item.CompletedDate;
            IsComplete = item.IsComplete;
            Link = item.Link;
        }
    
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [DisplayName("Due Date")]
        public DateTime DueDate { get; set; }

        [Editable(false)]
        [DisplayName("Completed")]
        public DateTime CompletedDate { get; private set; }

        [Editable(false)]
        [DisplayName("Is Complete")]
        public bool IsComplete { get; private set; }

        public Uri Link { get; set; }
        
        [Exclude]
        public bool IsPastDue { get; private set; }

        [Exclude]
        public bool IsDueTomorrow { get; private set; }

        [Exclude]
        public bool IsDueNextWeek { get; private set; }

        [Exclude]
        public bool IsInFuture { get; private set; }

        public IValidationResult ValidateDueDate(DateTime dueDate)
        {
            throw new NotImplementedException();
        }

        public IValidationResult ValidateTitle(string title)
        {
            throw new NotImplementedException();
        }
        
        public void MarkComplete()
        {
            throw new NotImplementedException();
        }

        public void CreateNew()
        {
            throw new NotImplementedException();
        }
    }
}