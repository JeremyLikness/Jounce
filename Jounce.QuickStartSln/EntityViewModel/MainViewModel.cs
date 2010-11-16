using System.Text.RegularExpressions;
using System.Windows;
using Jounce.Core.Command;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using Jounce.Framework.ViewModels;

namespace EntityViewModel
{
    [ExportAsViewModel("MainViewModel")]
    public class MainViewModel : BaseEntityViewModel
    {
        public MainViewModel()
        {
            CancelCommand = new ActionCommand<object>(obj => _Confirm(), obj => !Committed);
            var committedProp = ExtractPropertyName(() => Committed);

            if (InDesigner)
            {
                FirstName = "Jeremy";
                LastName = "Likness";
                PhoneNumber = "+1 404-555-1212";
                Email = "jeremy@jeremylikness.com";
            }
            else
            {
                // anytime the committed status changes, we should re-evaluate the cancel button
                PropertyChanged += (o, e) =>
                                        {
                                            if (e.PropertyName.Equals(committedProp))
                                            {
                                                CancelCommand.RaiseCanExecuteChanged();
                                            }
                                        };
            }
        }

        private void _Confirm()
        {
            var result = MessageBox.Show("Are you sure?", "Confirm Cancel", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                _Reset();
            }
        }

        public IActionCommand CancelCommand { get; private set; }

        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                RaisePropertyChanged(() => FirstName);
                _ValidateName(ExtractPropertyName(() => FirstName), value);
            }
        }

        private string _lastName;

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                RaisePropertyChanged(() => LastName);
                _ValidateName(ExtractPropertyName(() => LastName), value);
            }
        }

        private void _ValidateName(string prop, string value)
        {
            ClearErrors(prop);

            if (string.IsNullOrEmpty(value))
            {
                SetError(prop, "The field is required.");
            }
        }

        private string _phoneNumber;

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = value;
                RaisePropertyChanged(() => PhoneNumber);
                _ValidatePhoneNumber();
            }
        }

        private void _ValidatePhoneNumber()
        {
            var prop = ExtractPropertyName(() => PhoneNumber);
            ClearErrors(prop);
            if (string.IsNullOrEmpty(_phoneNumber) || !Regex.IsMatch(_phoneNumber,
                                                                        @"^((\+\d{1,3}(-| )?\(?\d\)?(-| )?\d{1,5})|(\(?\d{2,6}\)?))(-| )?(\d{3,4})(-| )?(\d{4})(( x| ext)\d{1,5}){0,1}$"))
            {
                SetError(prop, "Field should be a valid international phone number such as +1 404-555-1212");
            }
        }

        private string _email;

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                RaisePropertyChanged(() => Email);
                _ValidateEmail();
            }
        }

        private void _ValidateEmail()
        {
            var prop = ExtractPropertyName(() => Email);
            ClearErrors(prop);
            if (string.IsNullOrEmpty(_email) ||
                !Regex.IsMatch(_email, @"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$", RegexOptions.IgnoreCase))
            {
                SetError(prop, "Field should be a valid email address.");
            }
        }

        protected override void _ValidateAll()
        {
            _ValidateName(ExtractPropertyName(() => FirstName), _firstName);
            _ValidateName(ExtractPropertyName(() => LastName), _lastName);
            _ValidatePhoneNumber();
            _ValidateEmail();
        }

        protected override void _OnCommitted()
        {
            MessageBox.Show("Record was saved.");
            _Reset();
        }

        private void _Reset()
        {
            PhoneNumber = string.Empty;
            Email = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            Committed = true;
        }
    }
}