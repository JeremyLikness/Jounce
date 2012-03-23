using System.ComponentModel.Composition;
using Jounce.Core.Event;
using ToDoList.Contracts;

namespace ToDoList.Model
{
    /// <summary>
    /// Bridge between event aggregator implementation and more generic contract 
    /// </summary>
    [Export(typeof(IPublisher))]
    public class PubSub : IPublisher 
    {
        [Import]
        public IEventAggregator EventAggregator { get; set; }

        public void Publish<T>(T message)
        {
            EventAggregator.Publish(message);
        }        
    }
}