using ToDoList.Contracts;

namespace ToDoList.Model
{
    public class ValidationResult : IValidationResult
    {
        public bool IsValid { get; set; }
        public string ErrorText { get; set; }
        public static IValidationResult Create(string message)
        {
            return new ValidationResult
                       {
                           IsValid = string.IsNullOrEmpty(message),
                           ErrorText = message
                       };
        }
    }
}