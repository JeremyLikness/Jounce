using ToDoList.Contracts;

namespace ToDoList.Model
{
    public class MessageToDoItemComplete
    {
        public IToDoItem Item { get; private set; }

        private MessageToDoItemComplete(IToDoItem item)
        {
            Item = item;
        }

        public static MessageToDoItemComplete Create(
            IToDoItem item)
        {
            return new MessageToDoItemComplete(item);
        }
    }
}