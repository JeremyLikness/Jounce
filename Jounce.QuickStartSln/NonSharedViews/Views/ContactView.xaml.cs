using System.ComponentModel.Composition;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using NonSharedViews.ViewModels;

namespace NonSharedViews.Views
{
    [ExportAsView(typeof(ContactView))]
    public partial class ContactView
    {
        public ContactView()
        {
            InitializeComponent();
        }

        [Export]
        public ViewModelRoute Binding
        {
            get { return ViewModelRoute.Create<ContactViewModel,ContactView>(); }
        }
    }
}
