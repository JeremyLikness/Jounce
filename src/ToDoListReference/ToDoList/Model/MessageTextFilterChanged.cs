namespace ToDoList.Model
{
    public class MessageTextFilterChanged
    {
        public string Text { get; set; } 

        public static MessageTextFilterChanged Create(string text)
        {
            return new MessageTextFilterChanged {Text = text};
        }
    }
}