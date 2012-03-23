using System.Windows.Input;

namespace ToDoList.Contracts
{
    public interface IEditViewModel : IToDoItem
    {
        string TextLink { get; }
        bool LinkNoErrors { get; }
        ICommand CancelCommand { get; }
        ICommand CommitCommand { get; }
    }
}