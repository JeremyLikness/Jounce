using System;
using System.ComponentModel.Composition;
using ToDoList.Contracts;

namespace ToDoList.Model
{    
    [Export(typeof(IToDoItem))]
    public class ToDoItem : IToDoItem
    {
        public ToDoItem()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }        
        public DateTime DueDate { get; set; }        
        public DateTime CompletedDate { get; protected set; }
        public bool IsComplete { get; protected set; }
        public Uri Link { get; set; }

        [Import]
        public IPublisher PubSub { get; set; }

        public bool IsPastDue
        {
            get { return !IsComplete && DateTime.Now.CompareTo(DueDate) > 0; }
        }

        public bool IsDueTomorrow
        {
            get
            {
                if (IsComplete)
                {
                    return false;
                }

                var span = DueDate.Date - DateTime.Now.Date;
                return Math.Floor(span.TotalDays).Equals(1);
            }
        }

        public bool IsDueNextWeek
        {
            get
            {
                if (IsComplete)
                {
                    return false;
                }

                var dayOfWeek = DateTime.Now.DayOfWeek;
                int daysUntilMonday;
                if (dayOfWeek.Equals(DayOfWeek.Sunday))
                {
                    daysUntilMonday = 1;
                }
                else
                {
                    daysUntilMonday = 8 - (int) dayOfWeek;
                }
                var monday = DateTime.Now.Date.AddDays(daysUntilMonday);
                var sunday = DateTime.Now.Date.AddDays(daysUntilMonday + 7);
                return monday.CompareTo(DueDate) < 0 && sunday.CompareTo(DueDate) > 0;
            }
        }

        public bool IsInFuture
        {
            get { return !IsComplete && DateTime.Now.CompareTo(DueDate) < 0; }
        }        

        public void CreateNew()
        {
            PubSub.Publish(MessageNewToDoItem.Create(this));                        
        }
        
        public void MarkComplete()
        {
            if (IsComplete)
            {
                throw new InvalidOperationException("Task is already marked complete.");
            }

            IsComplete = true;
            CompletedDate = DateTime.Now;
            PubSub.Publish(MessageToDoItemComplete.Create(this));
        }

        public IValidationResult ValidateDueDate(DateTime dueDate)
        {
            var errorText = DateTime.Now.CompareTo(dueDate) <= 0 
                ? string.Empty 
                : "Due date must be in the future.";
            return ValidationResult.Create(errorText);
        }

        public IValidationResult ValidateTitle(string title)
        {
            var titleCheck = title ?? string.Empty;
            var errorText = string.IsNullOrEmpty(titleCheck.Trim())
                        ? "The task must have a title."
                        : string.Empty;
            return ValidationResult.Create(errorText);
        }


    }
}
