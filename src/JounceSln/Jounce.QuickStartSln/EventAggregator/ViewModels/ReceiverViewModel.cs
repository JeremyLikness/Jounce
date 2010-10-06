using System.ComponentModel.Composition;
using System.Windows.Media;
using Jounce.Core.Event;
using Jounce.Core.ViewModel;

namespace EventAggregator.ViewModels
{
    [ExportAsViewModel(Constants.VM_RECEIVER)]
    public class ReceiverViewModel : BaseViewModel, IEventSink<string>, IPartImportsSatisfiedNotification 
    {
        public ReceiverViewModel()
        {
            if (InDesigner)
            {
                Brush = new SolidColorBrush(Colors.Yellow);
            }
        }

        private SenderViewModel _sender;

        private SolidColorBrush _brush;
        public SolidColorBrush Brush
        {
            get { return _brush; }
            set
            {
                _brush = value;
                RaisePropertyChanged(()=>Brush);
            }
        }

        /// <summary>
        ///     Handle the event
        /// </summary>
        /// <param name="color">The color to process</param>
        public void HandleEvent(string color)
        {
            Brush = _sender[color];            
        }

        /// <summary>
        /// Called when a part's imports have been satisfied and it is safe to use.
        /// </summary>
        public void OnImportsSatisfied()
        {
            _sender = Router.ResolveViewModel<SenderViewModel>(Constants.VIEW_SENDER);
            EventAggregator.SubscribeOnDispatcher(this);
        }
    }
}