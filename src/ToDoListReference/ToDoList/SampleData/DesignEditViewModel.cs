using System;
using System.Windows.Input;
using Microsoft.Expression.Interactivity.Core;
using ToDoList.Contracts;

namespace ToDoList.SampleData
{
#if DEBUG
    public class DesignEditViewModel : IEditViewModel 
    {
        public DesignEditViewModel()
        {
            Id = Guid.NewGuid();
            Title = "Design Example";
            Description = "This is a design example of the task item and is for demonstration and design purposes only.";
            DueDate = DateTime.Now.AddDays(1);
            Link = new Uri("http://www.jeremylikness.com/");
            IsPastDue = false;
            IsDueTomorrow = true;
            IsDueNextWeek = false;
            IsInFuture = true;
            CommitCommand = new ActionCommand(() => { });
            CancelCommand = new ActionCommand(() => { });
        }
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CompletedDate { get; private set; }
        public bool IsComplete { get; private set; }
        public Uri Link { get; set; }
        public bool IsPastDue { get; private set; }
        public bool IsDueTomorrow { get; private set; }
        public bool IsDueNextWeek { get; private set; }
        public bool IsInFuture { get; private set; }
        public bool LinkNoErrors
        {
            get { return true; }
        }
        
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

        public string TextLink
        {
            get { return "http://www.jeremylikness.com/"; }
        }
        public ICommand CancelCommand { get; private set; }
        public ICommand CommitCommand { get; private set; }
    }
#endif 
}