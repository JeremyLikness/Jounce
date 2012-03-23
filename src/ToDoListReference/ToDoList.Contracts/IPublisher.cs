namespace ToDoList.Contracts
{
    public interface IPublisher
    {
        void Publish<T>(T message);        
    }
}