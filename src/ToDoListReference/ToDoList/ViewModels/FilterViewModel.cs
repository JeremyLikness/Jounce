using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using Jounce.Core.Command;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using ToDoList.Contracts;
using ToDoList.Filters;

namespace ToDoList.ViewModels
{
    [ExportAsViewModel(typeof(FilterViewModel))]
    public class FilterViewModel : BaseViewModel, IFilterViewModel
    {
        private FilterBase _currentFilter;

        [Import]
        public IToDoListApplicationContext ApplicationContext { get; set; }

        public FilterViewModel()
        {
            FilterCommand = new ActionCommand<FilterBase>(
                filter =>
                    {
                        EventAggregator.Publish(filter);
                        _currentFilter = filter;
                        ApplicationContext.FilterName = filter.Name;
                        FilterCommand.RaiseCanExecuteChanged();
                    },
                filter => !filter.Equals(_currentFilter));
        }

        public IActionCommand<FilterBase> FilterCommand { get; private set; }
        
        [ImportMany]
        public ObservableCollection<FilterBase> Filters { get; set; }

        IEnumerable<FilterBase> IFilterViewModel.Filters
        {
            get { return Filters; }
        }

        protected override void InitializeVm()
        {
            var filter = (from f in Filters
                            where f.Name.Equals(ApplicationContext.FilterName)
                            select f).FirstOrDefault()
                            ?? Filters.First();
            FilterCommand.Execute(filter);            
        }
    }
}