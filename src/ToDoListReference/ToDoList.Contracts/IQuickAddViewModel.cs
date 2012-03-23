using System;
using System.Windows.Input;

namespace ToDoList.Contracts
{
    public interface IQuickAddViewModel
    {
        string DueDate { get; }
        string Title { get; }
        ICommand CommitCommand { get; }        
    }
}