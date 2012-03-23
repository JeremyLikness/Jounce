using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;
using Jounce.Core.Command;
using Jounce.Core.Event;
using Jounce.Core.ViewModel;
using Jounce.Framework;
using Jounce.Framework.Command;
using ToDoList.Contracts;
using ToDoList.Model;

namespace ToDoList.ViewModels
{
    [ExportAsViewModel(typeof(ToDoListItemViewModel))]
    public class ToDoListItemViewModel : BaseViewModel, IToDoListItemViewModel, IEventSink<MessageToDoItemComplete>, IPartImportsSatisfiedNotification
    {
        [Import]
        public IRepository Repository { get; set; }        

        [Import]
        public Global Globals { get; set; }

        public ToDoListItemViewModel()
        {
            DeleteCommand = new ActionCommand<object>(
                obj=>Delete());
            EditCommand = new ActionCommand<object>(
                obj => Edit());
            MarkCompleteCommand = new ActionCommand<object>(
                obj => Task.MarkComplete(), obj => !Task.IsComplete);
        }

        private void Edit()
        {
            EventAggregator.Publish(Globals.EditView.AsViewNavigationArgs()
                .AddNamedParameter(Global.Constants.PARM_STATE,Global.Constants.STATE_EDIT)
                .AddNamedParameter(Global.Constants.PARM_ITEM, _task));
        }

        private void Delete()
        {
            var result =
                MessageBox.Show(
                    "Are you sure you wish to delete this task? Click OK to delete and Cancel to leave the task.",
                    "Confirm Delete", MessageBoxButton.OKCancel);
            if (result.Equals(MessageBoxResult.OK))
            {
                Repository.Delete(_task);
            }
        }

        private IToDoItem _task;

        public IToDoItem Task
        {
            get { return _task; }
            set 
            { 
                _task = value;
                RaisePropertyChanged(() => Task);
                RaisePropertyChanged(() => ToolTip);
                RaisePropertyChanged(() => DateLabel);
                RaisePropertyChanged(() => KeyDate);

            }
        }

        public DateTime KeyDate
        {
            get
            {
                return _task.IsComplete
                           ? _task.CompletedDate
                           : _task.DueDate;
            }
        }

        public string DateLabel
        {
            get
            {
                return _task.IsComplete
                           ? "Completed:"
                           : "Due:";
            }
        }

        public string ToolTip
        {
            get
            {
                if (Task == null)
                {
                    return string.Empty;
                }

                return string.IsNullOrEmpty(Task.Description)
                           ? Task.Title
                           : Task.Description;
            }
        }

        public ICommand DeleteCommand { get; private set; }
        
        public IActionCommand MarkCompleteCommand { get; private set; }

        public IActionCommand EditCommand { get; private set; }

        ICommand IToDoListItemViewModel.EditCommand
        {
            get { return EditCommand; }
        }

        ICommand IToDoListItemViewModel.MarkCompleteCommand
        {
            get { return MarkCompleteCommand; }
        }

        public void HandleEvent(MessageToDoItemComplete publishedEvent)
        {
            if (!publishedEvent.Item.Id.Equals(Task.Id))
            {
                return;
            }

            MarkCompleteCommand.RaiseCanExecuteChanged();
            RaisePropertyChanged(() => KeyDate);
            RaisePropertyChanged(() => DateLabel);
        }

        public void OnImportsSatisfied()
        {
            EventAggregator.Subscribe(this);                        
            Repository.AggregateRootChanged += RepositoryAggregateRootChanged;
        }

        void RepositoryAggregateRootChanged(object sender, EventArgs e)
        {
            Repository.AggregateRootChanged -= RepositoryAggregateRootChanged;
            EventAggregator.Unsubscribe(this);
        }
    }
}