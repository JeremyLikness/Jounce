using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;
using Jounce.Core.Command;
using Jounce.Core.ViewModel;
using Jounce.Framework;
using Jounce.Framework.Command;
using Jounce.Framework.ViewModel;
using ToDoList.Behaviors;
using ToDoList.Contracts;
using ToDoList.Model;

namespace ToDoList.ViewModels
{
    [ExportAsViewModel(typeof(EditViewModel))]
    public class EditViewModel : BaseEntityViewModel, IEditViewModel  
    {
        private IToDoItem _item;
        private Guid _id;
        private string _title;
        private string _description;
        private DateTime _dueDate;
        private string _link;
        private bool _isNew;
        private bool _promptApp;
        
        [Import]
        public ExportFactory<ToDoItemOverride> ItemFactory { get; set; }
        
        [Import]
        public IRepository Repository { get; set; }
        
        [Import]
        public IToDoListApplicationContext AppContext { get; set; }

        public EditViewModel()
        {
            CancelCommand = new ActionCommand<object>(
                obj => Cancel(),
                obj => true);
        }

        private void Cancel()
        {
            if (!Committed)
            {
                var result = MessageBox.Show(
                    "If you cancel, you will lose all changes. Are you sure you wish to cancel? Press OK to continue or Cancel to return to editing.",
                    "Confirm Cancellatinon",
                    MessageBoxButton.OKCancel);
                if (result.Equals(MessageBoxResult.OK))
                {
                    Close();
                }
            }
            else
            {
                Close();
            }
        }

        private void Close()
        {
            AppContext.PromptBeforeClose = _promptApp;
            AppContext.Title = Global.Constants.MAIN_TITLE;
            GoToVisualState(Global.Constants.STATE_CLOSED, true);
            OverlayHelper.Hide();
        }

        public void NewItem()
        {
            AppContext.Title = Global.Constants.NEW_TITLE;
            _item = ItemFactory.CreateExport().Value;
            _item.DueDate = DateTime.Now.Date.AddDays(1);
            _item.Title = string.Empty;
            _item.Description = string.Empty;            
            _isNew = true;
            RefreshProperties();
            AppContext.PromptBeforeClose = false;
        }              

        public void EditItem(IToDoItem item)
        {            
            _item = item;
            AppContext.Title = string.Format(Global.Constants.EDIT_TITLE,
                                             _item.Title);
            _isNew = false;
            RefreshProperties();
            AppContext.PromptBeforeClose = false;
        }
        
        private void RefreshProperties()
        {                
            Id = _item.Id;
            Title = _item.Title;
            Description = _item.Description;
            DueDate = _item.DueDate;
            Link = _item.Link;
            _link = _item.Link == null ? string.Empty : _item.Link.ToString();
            LinkNoErrors = !string.IsNullOrEmpty(TextLink);
            RaisePropertyChanged(() => TextLink);
            RaisePropertyChanged(() => CompletedDate);
            RaisePropertyChanged(() => IsComplete);
            RaisePropertyChanged(() => IsDueNextWeek);
            RaisePropertyChanged(() => IsDueTomorrow);
            RaisePropertyChanged(() => IsInFuture);
            RaisePropertyChanged(() => IsPastDue);
            RaisePropertyChanged(() => LinkNoErrors);
            Committed = true;
            CancelCommand.RaiseCanExecuteChanged();            
        }

        private void CommitToItem()
        {
            _item.Title = Title;
            _item.Description = Description;
            _item.DueDate = DueDate;
            _item.Link = Link;
        }

        public Guid Id
        {
            get { return _id; }
            set
            {
                _id = value;
                RaisePropertyChanged(() => Id);
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                if (_title == value)
                {
                    return;
                }
                AppContext.PromptBeforeClose = true;
                _title = value;
                CancelCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(() => Title);
                ValidateTitle(value);
            }
        }
        
        public string Description
        {
            get { return _description; }
            set
            {
                if (_description == value)
                {
                    return;
                }
                AppContext.PromptBeforeClose = true;
                _description = value;
                CancelCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(()=>Description);
            }
        }

        public DateTime DueDate
        {
            get { return _dueDate; }
            set
            {
                if (_dueDate == value)
                {
                    return;
                }
                AppContext.PromptBeforeClose = true;                
                _dueDate = value;
                RaisePropertyChanged(() => DueDate);
                CancelCommand.RaiseCanExecuteChanged();
                if (IsComplete)
                {
                    return;
                }
                ValidateDueDate(value);
            }
        }

        public DateTime CompletedDate
        {
            get { return _item.CompletedDate; }
        }

        public bool IsComplete
        {
            get { return _item.IsComplete; }
        }

        private Uri _internalLink;
        public Uri Link
        {
            get { return _internalLink; }
            set 
            { 
                _internalLink = value;
                RaisePropertyChanged(() => Link);
            }
        }
        
        public string TextLink
        {
            get { return _link; }
            set
            {
                if (_link == value)
                {
                    return;
                }
                AppContext.PromptBeforeClose = true;                
                _link = value;
                RaisePropertyChanged(() => Link);                
                CancelCommand.RaiseCanExecuteChanged();
                ValidateLink(value);
                RaisePropertyChanged(() => LinkNoErrors);
            }
        }

        public bool LinkNoErrors { get; private set; }        

        public bool IsPastDue
        {
            get { return _item.IsPastDue; }
        }

        public bool IsDueTomorrow
        {
            get { return _item.IsDueTomorrow; }
        }

        public bool IsDueNextWeek
        {
            get { return _item.IsDueNextWeek; }
        }

        public bool IsInFuture
        {
            get { return _item.IsInFuture; }
        }

        public IValidationResult ValidateDueDate(DateTime dueDate)
        {
            ClearErrors(() => DueDate);
            IValidationResult result = ValidationResult.Create(string.Empty);
            if (!dueDate.Equals(_item.DueDate))
            {
                result = _item.ValidateDueDate(dueDate);
                if (!result.IsValid)
                {
                    SetError(() => DueDate, result.ErrorText);
                }
            }
            return result;
        }

        public IValidationResult ValidateTitle(string title)
        {
            ClearErrors(() => Title);
            var result = _item.ValidateTitle(title);
            if(!result.IsValid)
            {
                SetError(() => Title, result.ErrorText);
            }
            return result;
        }

        public void MarkComplete()
        {
            throw new NotImplementedException();
        }

        private void ValidateLink(string link)
        {
            ClearErrors(()=>TextLink);
            LinkNoErrors = true;
            Uri uri;
            var result = ValidationResult.Create(string.Empty);
            if (string.IsNullOrEmpty(link))
            {
                Link = null;
                LinkNoErrors = false;
            }
            else if (Uri.TryCreate(link, UriKind.Absolute, out uri))
            {
                Link = uri;                
            }
            else
            {
                result = ValidationResult.Create("Link must be a valid URL.");
            }
            if (!result.IsValid)
            {
                LinkNoErrors = false;
                SetError(()=>TextLink, result.ErrorText);
            }
            return;
        }
        
        public IActionCommand CancelCommand { get; private set; }
                
        ICommand IEditViewModel.CancelCommand
        {
            get { return CancelCommand; }
        }

        ICommand IEditViewModel.CommitCommand
        {
            get { return CommitCommand; }
        }
        
        public void CreateNew()
        {
            _item.CreateNew();            
        }

        protected override void ValidateAll()
        {
            ValidateTitle(_title);
            if (!IsComplete)
            {
                ValidateDueDate(_dueDate);
            }
            ValidateLink(_link);
        }

        protected override void OnCommitted()
        {
            CommitToItem();
            if (_isNew)
            {
                _item.CreateNew();
            }
            else
            {
                Repository.Save(_item);
            }
            base.OnCommitted();    
            Close();
        }

        protected override void ActivateView(string viewName, 
            IDictionary<string, object> viewParameters)
        {
            _promptApp = AppContext.PromptBeforeClose;
            if (!viewParameters.ContainsKey(Global.Constants.PARM_STATE)) return;

            var state = viewParameters.ParameterValue<string>(Global.Constants.PARM_STATE);
            GoToVisualState(state, true);

            OverlayHelper.Show();         

            switch (state)
            {
                case Global.Constants.STATE_NEW:
                    NewItem();
                    break;
                case Global.Constants.STATE_EDIT:
                    EditItem(viewParameters.ParameterValue<IToDoItem>(
                        Global.Constants.PARM_ITEM));
                    break;
            }
        }       
    }
}