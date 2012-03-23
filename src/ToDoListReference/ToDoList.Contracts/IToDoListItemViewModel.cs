using System;
using System.Windows.Input;

namespace ToDoList.Contracts
{
    public interface IToDoListItemViewModel
    {
        IToDoItem Task { get; }
        string ToolTip { get; }
        string DateLabel { get; }
        DateTime KeyDate { get; }
        ICommand DeleteCommand { get; }
        ICommand EditCommand { get; }
        ICommand MarkCompleteCommand { get; }
    }
}