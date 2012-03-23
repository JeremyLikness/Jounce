namespace ToDoList.Contracts
{
    public interface IToDoListApplicationContext
    {
        string Title { get; set; }
        bool PromptBeforeClose { get; set; }
        string FilterName { get; set; }
        string SortName { get; set; }
        bool IsInstalled { get; }
        bool IsRunningOutOfBrowser { get; }
        string XapSource { get; }
    }
}