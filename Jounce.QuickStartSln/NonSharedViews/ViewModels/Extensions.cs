using System;
using System.Collections.Generic;
using System.Linq;
using Jounce.Core.ViewModel;
using NonSharedViews.Models;

namespace NonSharedViews.ViewModels
{
    public static class Extensions
    {
        /// <summary>
        ///     Cast the model to the supporting view model
        /// </summary>
        /// <param name="contact">The contact</param>
        /// <param name="router">View Model Router</param>
        /// <returns>The supporting view model</returns>
        public static ContactViewModel ToViewModel(this Contact contact, IViewModelRouter router)
        {
            var vm = router.GetNonSharedViewModel<ContactViewModel>();

            if (vm == null)
            {
                throw new Exception("Couldn't create view model for contact.");
            }

            vm.SourceContact = contact;
            return vm;
        }

        /// <summary>
        ///     Convert a list
        /// </summary>
        /// <param name="contacts">The  contacts</param>
        /// <param name="router">The router</param>
        /// <returns>The list</returns>
        public static IEnumerable<ContactViewModel> ToViewModels(this IEnumerable<Contact> contacts, IViewModelRouter router)
        {
            return contacts.Select(contact => contact.ToViewModel(router)).ToList();
        }
    }
}