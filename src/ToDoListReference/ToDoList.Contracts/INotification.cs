using System;

namespace ToDoList.Contracts
{
    public interface INotification
    {
        void Notify(
            string caption, 
            string text, 
            TimeSpan time);
    }
}