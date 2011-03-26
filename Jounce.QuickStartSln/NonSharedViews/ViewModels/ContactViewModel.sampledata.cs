using System.Diagnostics;
using NonSharedViews.Models;

namespace NonSharedViews.ViewModels
{
    public partial class ContactViewModel
    {
        [Conditional("DESIGN")]
        protected void SetDesignerData()
        {
            var contact = new Contact
                              {
                                  FirstName = "Jeremy",
                                  LastName = "Likness",
                                  Address = "1212 Hollywood Blvd",
                                  City = "Hollywood",
                                  State = "California"
                              };
            SourceContact = contact;
        }
    }
}