using System;
using System.ComponentModel.Composition;
using System.Windows;
using Jounce.Core.Event;

namespace EventAggregator.Views
{
    /// <summary>
    ///     Handles exceptions
    /// </summary>
    /// <remarks>
    ///     This isn't a user control/view so we can't export as view. Instead, we export as an object and then
    ///     let the main view model import based on the contract name. This will wire us up and start us listening
    ///     for error events, and the rest is, as they say, history.
    /// </remarks>
    [Export(Constants.VIEW_EXCEPTION,typeof(object))]
    public partial class UnhandledException : IPartImportsSatisfiedNotification, IEventSink<UnhandledExceptionEvent>
    {
        [Import]
        public IEventAggregator EventAggregator { get; set; }

        private Exception _lastException; 

        public UnhandledException()
        {
            InitializeComponent();
            Closed += UnhandledException_Closed;
        }

        void UnhandledException_Closed(object sender, EventArgs e)
        {
            if (!DialogResult != true)
            {
            }
            else
            {
                EventAggregator.Unsubscribe(this);
                throw _lastException;
            }
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;            
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;           
        }

        /// <summary>
        /// Called when a part's imports have been satisfied and it is safe to use.
        /// </summary>
        public void OnImportsSatisfied()
        {
            EventAggregator.SubscribeOnDispatcher(this);
        }

        public void HandleEvent(UnhandledExceptionEvent publishedEvent)
        {
            ErrorText.Text = publishedEvent.UncaughtException.Message;
            _lastException = publishedEvent.UncaughtException;
            publishedEvent.Handled = true;
            Show();
        }
    }
}

