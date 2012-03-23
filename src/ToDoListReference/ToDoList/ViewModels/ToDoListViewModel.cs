using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using Jounce.Core.Event;
using Jounce.Core.ViewModel;
using ToDoList.Contracts;
using ToDoList.Filters;
using ToDoList.Model;
using ToDoList.Sorts;

namespace ToDoList.ViewModels
{
    [ExportAsViewModel(typeof (ToDoListViewModel))]
    public class ToDoListViewModel : BaseViewModel, IToDoListViewModel, IEventSink<FilterBase>,
        IEventSink<SortBase>, IEventSink<MessageTextFilterChanged>, IEventSink<MessageNewToDoItem>, 
        IEventSink<MessageToDoItemComplete>
    {
        private readonly ObservableCollection<IToDoItem> _tasks =
            new ObservableCollection<IToDoItem>();

        private FilterBase _filter = FilterBase.Create("Default", t => true);
        private SortBase _sort = SortBase.Create("Default", t => t);

        private string _textFilter = string.Empty;
        
        [Import]
        public ExportFactory<IPublisher> PubSub { get; set; }

        [Import]
        public IRepository Repository { get; set; }               

        [Import]
        public Global Globals { get; set; }
        
        public IEnumerable<IToDoItem> Tasks
        {
            get
            {
                var query = from t in _tasks
                            where _filter.Filter(t)
                            select t; 
                if (!string.IsNullOrEmpty(_textFilter))
                {
                    query = query.Where(t => ContainsFilter(t.Title) ||
                                                ContainsFilter(t.Description));
                }
                return _sort.Sort(query);
            }
        }        

        private bool ContainsFilter(string source)
        {
            return !string.IsNullOrEmpty(source) && source.ToLower().Contains(_textFilter);
        }

        protected override void ActivateView(string viewName, IDictionary<string, object> viewParameters)
        {
            _tasks.Clear();

            foreach (var task in Repository.Query())
            {
                ((ToDoItemOverride)task).PubSub =
                                PubSub.CreateExport().Value;
                _tasks.Add(task);
            }
            RaisePropertyChanged(() => Tasks);

            Repository.AggregateRootChanged +=
                (o, e) =>
                    {                        
                        _tasks.Clear();
                        foreach (var task in Repository.Query())
                        {
                            ((ToDoItemOverride) task).PubSub =
                                PubSub.CreateExport().Value;
                            _tasks.Add(task);
                        }
                        RaisePropertyChanged(() => Tasks);
                    };

            base.ActivateView(viewName, viewParameters);
        }

        protected override void InitializeVm()
        {            
            EventAggregator.Subscribe<FilterBase>(this);
            EventAggregator.Subscribe<SortBase>(this);
            EventAggregator.Subscribe<MessageNewToDoItem>(this);
            EventAggregator.Subscribe<MessageTextFilterChanged>(this);
            EventAggregator.Subscribe<MessageToDoItemComplete>(this);
            base.InitializeVm();
        }
        
        public void HandleEvent(FilterBase publishedEvent)
        {
            _filter = publishedEvent;
            RaisePropertyChanged(() => Tasks);
        }

        public void HandleEvent(MessageNewToDoItem publishedEvent)
        {
            Repository.Save(publishedEvent.Item);
        }

        public void HandleEvent(MessageTextFilterChanged publishedEvent)
        {
            _textFilter = publishedEvent.Text.ToLower().Trim();
            RaisePropertyChanged(() => Tasks);
        }

        public void HandleEvent(SortBase publishedEvent)
        {
            _sort = publishedEvent;
            RaisePropertyChanged(() => Tasks);
        }

        public void HandleEvent(MessageToDoItemComplete publishedEvent)
        {
            var item = (from t in _tasks
                        where publishedEvent.Item.Id == t.Id
                        select t).FirstOrDefault();

            if (item == null) return;
            ((ToDoItemOverride)item).IsComplete = true;
            ((ToDoItemOverride) item).CompletedDate = publishedEvent.Item.CompletedDate;
        }
    }
}