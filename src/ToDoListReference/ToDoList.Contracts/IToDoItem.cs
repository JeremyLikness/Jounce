using System;

namespace ToDoList.Contracts
{
    public interface IToDoItem
    {
        Guid Id { get; set; }
        string Title { get; set; }
        string Description { get; set; }
        DateTime DueDate { get; set; }
        DateTime CompletedDate { get; }
        bool IsComplete { get; }
        Uri Link { get; set; }
        bool IsPastDue { get; }
        bool IsDueTomorrow { get; }
        bool IsDueNextWeek { get; }
        bool IsInFuture { get; }
        IValidationResult ValidateDueDate(DateTime dueDate);
        IValidationResult ValidateTitle(string title);
        void MarkComplete();
        void CreateNew();
    }
}