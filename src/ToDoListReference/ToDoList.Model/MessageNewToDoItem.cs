using ToDoList.Contracts;

namespace ToDoList.Model
{
    public class MessageNewToDoItem
    {
        private MessageNewToDoItem(IToDoItem item)
        {
            Item = item;
        }

        public IToDoItem Item { get; private set; } 

        public static MessageNewToDoItem Create(IToDoItem item)
        {
            return new MessageNewToDoItem(item);
        }
    }
}