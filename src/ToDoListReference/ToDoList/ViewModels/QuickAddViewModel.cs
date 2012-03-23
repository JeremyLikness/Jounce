using System;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Jounce.Core.ViewModel;
using Jounce.Framework.ViewModel;
using ToDoList.Contracts;
using ToDoList.Model;

namespace ToDoList.ViewModels
{
    [ExportAsViewModel(typeof(QuickAddViewModel))]
    public class QuickAddViewModel 
        : BaseEntityViewModel, IQuickAddViewModel
    {
        private IToDoItem _referenceItem;

        public QuickAddViewModel()
        {
            GetToDoItem = () => new ToDoItem();
            DueDate = DateTime.Now.Date.AddDays(1).ToString();
            _referenceItem = GetToDoItem();
        }

        [Import]
        public IToDoListApplicationContext ApplicationContext { get; set; }

        private Func<IToDoItem> _getToDoItem;

        [Import(typeof(Func<IToDoItem>), AllowDefault = true, AllowRecomposition = true)]
        public Func<IToDoItem> GetToDoItem
        {
            get { return _getToDoItem; }
            set 
            { 
                _getToDoItem = value;
                _referenceItem = value();
            }
        }
        
        private DateTime _dueDate;
        private string _dueDateDisplay;
        
        public string DueDate
        {
            get { return _dueDateDisplay; }
            set
            {
                ClearErrors(() => DueDate);
                _dueDateDisplay = value;
                    
                if (DateTime.TryParse(value, out _dueDate))
                {
                    if (_referenceItem != null)
                    {
                        var result = _referenceItem.ValidateDueDate(_dueDate);
                        if (!result.IsValid)
                        {
                            SetError(() => DueDate, result.ErrorText);
                        }
                    }
                    RaisePropertyChanged(() => DueDate);
                }
                else
                {
                   SetError(() => DueDate, "Date must be in proper format.");
                }
            }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                ClearErrors(() => Title);
                if (_referenceItem != null)
                {
                    var result = _referenceItem.ValidateTitle(value);
                    if (!result.IsValid)
                    {
                        SetError(() => Title, result.ErrorText);
                    }
                }
                RaisePropertyChanged(() => Title);
                ApplicationContext.PromptBeforeClose = !string.IsNullOrEmpty(_title);
            }
        }              
        
        ICommand IQuickAddViewModel.CommitCommand
        {
            get { return CommitCommand; }
        }

        protected override void OnCommitted()
        {
            _referenceItem.Title = Title;
            _referenceItem.Description = Title;
            _referenceItem.DueDate = _dueDate;
            _referenceItem.CreateNew();
            Reset();            
        }

        protected override void ValidateAll()
        {
            ClearErrors(() => DueDate);
            ClearErrors(() => Title);
            var result = _referenceItem.ValidateDueDate(_dueDate);
            if (!result.IsValid)
            {
                SetError(() => DueDate, result.ErrorText);
            }
            result = _referenceItem.ValidateTitle(_title);
            if (!result.IsValid)
            {
                SetError(() => Title, result.ErrorText);
            }
        }
        
        private void Reset()
        {
            _title = string.Empty;
            DueDate = DateTime.Now.Date.AddDays(1).ToString();
            _referenceItem = GetToDoItem();
            RaisePropertyChanged(() => Title);
            RaisePropertyChanged(() => DueDate);
        }

        protected override void InitializeVm()
        {
            _referenceItem = GetToDoItem();
        }
    }
}