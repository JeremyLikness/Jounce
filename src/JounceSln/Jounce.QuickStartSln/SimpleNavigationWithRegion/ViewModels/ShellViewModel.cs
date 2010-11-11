using System.ComponentModel.Composition;
using Jounce.Core.View;
using Jounce.Core.ViewModel;

namespace SimpleNavigationWithRegion.ViewModels
{
    /// <summary>
    ///     The shell just hosts the navigation and the views. Normally I'd use regions for this but this
    ///     example doesn't include region manager and this is one way to abstract binding a control without
    ///     having a hard reference to the controls namespace
    /// </summary>
    [ExportAsViewModel("Shell")]
    public class ShellViewModel : BaseViewModel, IPartImportsSatisfiedNotification
    {       
        /// <summary>
        /// Called when a part's imports have been satisfied and it is safe to use.
        /// </summary>
        public void OnImportsSatisfied()
        {           
            // publish this so the binding happens
            EventAggregator.Publish(new ViewNavigationArgs("Navigation"));
        }               
    }
}