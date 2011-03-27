using System.ComponentModel;
using Jounce.Core.ViewModel;
using NonSharedViews.Models;

namespace NonSharedViews.ViewModels
{
    [ExportAsViewModel(typeof(ContactViewModel))]
    public partial class ContactViewModel : BaseViewModel
    {
        public ContactViewModel()
        {
            if (DesignerProperties.IsInDesignTool)
            {
                SetDesignerData();
            }
        }

        public Contact SourceContact { get; set; }
        
        public string FirstName
        {
            get { return SourceContact.FirstName; }
            set
            {
                SourceContact.FirstName = value;
                RaisePropertyChanged(()=>FirstName);
            }
        }

        public string LastName
        {
            get { return SourceContact.LastName; }
            set
            {
                SourceContact.LastName = value;
                RaisePropertyChanged(()=>LastName);
            }
        }

        public string Address
        {
            get { return SourceContact.Address; }
            set
            {
                SourceContact.Address = value;
                RaisePropertyChanged(()=>Address);
            }
        }

        public string City
        {
            get { return SourceContact.City; }
            set
            {
                SourceContact.City = value;
                RaisePropertyChanged(()=>City);
            }
        }

        public string State
        {
            get { return SourceContact.State; }
            set
            {
                SourceContact.State = value;
                RaisePropertyChanged(()=>State);
            }
        }
    }
}