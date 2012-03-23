using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using Jounce.Core.Event;
using Jounce.Core.ViewModel;
using ToDoList.Model;

namespace ToDoList.ViewModels
{
    [ExportAsViewModel(typeof(EventLogViewModel))]
    public class EventLogViewModel : 
        BaseViewModel, 
        IEventSink<MessageToDoItemComplete>,
        IEventSink<MessageNewToDoItem>
    {        
        private const string MARK_TEMPLATE = 
            "{0} Mark Task Complete: {1}";

        private const string NEW_TEMPLATE =
            "{0} New Task: {1}";

        public ObservableCollection<string> Events { get; private set; }

        public EventLogViewModel()
        {
            Events = new ObservableCollection<string>();

            if (!InDesigner)
            {
                return;
            }

            Events.Add(string.Format(
                NEW_TEMPLATE, 
                DateTime.Now.AddMinutes(-5), 
                "New Task"));
            Events.Add(string.Format(
                MARK_TEMPLATE, 
                DateTime.Now, "Old Task"));
        }

        public void HandleEvent(MessageToDoItemComplete publishedEvent)
        {
            Events.Add(string.Format(MARK_TEMPLATE, 
                DateTime.Now, 
                publishedEvent.Item.Title));
        }

        public void HandleEvent(MessageNewToDoItem publishedEvent)
        {
            Events.Add(string.Format(NEW_TEMPLATE,
                DateTime.Now,
                publishedEvent.Item.Title));
        }

        protected override void ActivateView(string viewName, 
            System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            EventAggregator.SubscribeOnDispatcher<MessageNewToDoItem>(this);
            EventAggregator.SubscribeOnDispatcher<MessageToDoItemComplete>(this);
            base.ActivateView(viewName, viewParameters);
        }
    }
}