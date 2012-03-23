namespace ToDoList.Contracts
{
    public interface IValidationResult
    {
        bool IsValid { get; }
        string ErrorText { get; }
    }
}