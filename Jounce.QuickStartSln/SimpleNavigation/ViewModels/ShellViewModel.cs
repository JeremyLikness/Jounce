using System.ComponentModel.Composition;
using Jounce.Core.Event;
using Jounce.Core.View;
using Jounce.Core.ViewModel;

namespace SimpleNavigation.ViewModels
{
    /// <summary>
    ///     The shell just hosts the navigation and the views. Normally I'd use regions for this but this
    ///     example doesn't include region manager and this is one way to abstract binding a control without
    ///     having a hard reference to the controls namespace
    /// </summary>
    [ExportAsViewModel("Shell")]
    public class ShellViewModel : BaseViewModel, IPartImportsSatisfiedNotification, IEventSink<ViewNavigationArgs>
    {
        private object _navigation;
        public object Navigation
        {
            get { return _navigation; }
            set
            {
                _navigation = value;
                RaisePropertyChanged(()=>Navigation);
            }
        }

        private object _currentView;

        /// <summary>
        ///     The current view - this is where we'll set the control
        /// </summary>
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                RaisePropertyChanged(()=>CurrentView);
            }
        }

        /// <summary>
        /// Called when a part's imports have been satisfied and it is safe to use.
        /// </summary>
        public void OnImportsSatisfied()
        {
            EventAggregator.SubscribeOnDispatcher(this);  
         
            // ask the router for the navigation control
            Navigation = Router["Navigation"];

            // publish this so the binding happens
            EventAggregator.Publish(new ViewNavigationArgs("Navigation"));
        }
        
        /// <summary>
        ///     Our event sink is the raised event for a navigation
        /// </summary>
        /// <param name="publishedEvent"></param>
        public void HandleEvent(ViewNavigationArgs publishedEvent)
        {
            //  we'll use the router to reference the view based on the passed type
            if (!publishedEvent.ViewType.Equals("Navigation"))
            {
                CurrentView = Router[publishedEvent.ViewType];
            }
        }
    }
}