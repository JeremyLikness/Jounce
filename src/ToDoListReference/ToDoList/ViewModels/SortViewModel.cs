using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using Jounce.Core.Command;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using ToDoList.Contracts;
using ToDoList.Sorts;

namespace ToDoList.ViewModels
{
    [ExportAsViewModel(typeof(SortViewModel))]
    public class SortViewModel : BaseViewModel, ISortViewModel
    {
        private SortBase _currentSort;

        [Import]
        public IToDoListApplicationContext ApplicationContext { get; set; }

        public SortViewModel()
        {
            SortCommand = new ActionCommand<SortBase>(
                sort =>
                {
                    EventAggregator.Publish(sort);
                    _currentSort = sort;
                    ApplicationContext.SortName = sort.Name;
                    SortCommand.RaiseCanExecuteChanged();
                },
                sort => !sort.Equals(_currentSort));
        }

        public IActionCommand<SortBase> SortCommand { get; private set; }

        [ImportMany]
        public ObservableCollection<SortBase> Sorts { get; set; }

        IEnumerable<SortBase> ISortViewModel.Sorts
        {
            get { return Sorts; }
        }

        protected override void InitializeVm()
        {
            var sort = (from s in Sorts
                        where s.Name.Equals(ApplicationContext.SortName)
                        select s).FirstOrDefault()
                       ?? Sorts.First();
            SortCommand.Execute(sort);
        }
    }
}