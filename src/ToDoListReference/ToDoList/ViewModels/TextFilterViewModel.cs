using Jounce.Core.ViewModel;
using ToDoList.Contracts;
using ToDoList.Model;

namespace ToDoList.ViewModels
{
    [ExportAsViewModel(typeof(TextFilterViewModel))]
    public class TextFilterViewModel : BaseViewModel, ITextFilterViewModel 
    {
        private string _textFilter;
        private string _lastFilter = string.Empty;

        public string TextFilter
        {
            get { return _textFilter; }
            set
            {
                _textFilter = value;
                RaisePropertyChanged(() => TextFilter);
                ProcessFilterMessage();
            }
        }

        private void ProcessFilterMessage()
        {
            var text = _textFilter ?? string.Empty;
            if (!text.Equals(_lastFilter))
            {
                _lastFilter = text; 
                EventAggregator.Publish(MessageTextFilterChanged.Create(text));
            }
        }
    }
}