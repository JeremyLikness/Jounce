using System.Collections.Generic;
using System.Collections.ObjectModel;
using Jounce.Core.ViewModel;
using NonSharedViews.Models;

namespace NonSharedViews.ViewModels
{
    [ExportAsViewModel(typeof(MainViewModel))]
    public class MainViewModel : BaseViewModel
    {
        private readonly List<Contact> _sampleData = new List<Contact>
                                                {
                                                    new Contact
                                                        {
                                                            FirstName = "Jeremy",
                                                            LastName = "Likness",
                                                            Address = "1212 Hollywood Blvd",
                                                            City = "Hollywood",
                                                            State = "California"
                                                        },
                                                    new Contact
                                                        {
                                                            FirstName = "John",
                                                            LastName = "Doe",
                                                            Address = "12 Driving Parkway",
                                                            City = "St. Petersburg",
                                                            State = "Florida"
                                                        },
                                                    new Contact
                                                        {
                                                            FirstName = "Jane",
                                                            LastName = "Doe",
                                                            Address = "1414 Disk Drive",
                                                            City = "Lead",
                                                            State = "South Dakota"
                                                        },
                                                    new Contact
                                                        {
                                                            FirstName = "Sam",
                                                            LastName = "Iam",
                                                            Address = "12 Many Terrace",
                                                            City = "Figment",
                                                            State = "Imagination"
                                                        },

                                                };

        public MainViewModel()
        {
            Contacts = new ObservableCollection<Contact>(_sampleData);
            Contacts.CollectionChanged += (o, e) => RaisePropertyChanged(() => ViewModels);            
        }

        public ObservableCollection<Contact> Contacts { get; private set; }

        public IEnumerable<ContactViewModel> ViewModels
        {
            get { return Contacts.ToViewModels(Router); }
        }
    }
}