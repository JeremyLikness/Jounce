using System.Collections.Generic;
using System.Linq;
using Jounce.Core.Command;
using Jounce.Core.Event;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework;
using Jounce.Framework.Command;
using Jounce.Framework.ViewModels;
using NavigationWithBackButton.Messages;

namespace NavigationWithBackButton.ViewModels
{
    [ExportAsViewModel("Main")]
    public class MainViewModel : BaseViewModel, IEventSink<ViewNavigationArgs>, IEventSink<GoBack>
    {
        private string _currentView = string.Empty;

        private bool _goingBack; 

        private readonly Stack<string> _history = new Stack<string>();

        public MainViewModel()
        {
            BackCommand = new ActionCommand<object>(o => EventAggregator.Publish(new GoBack()),
                o => _history.Count > 0);
        }

        public override void _Initialize()
        {
            EventAggregator.Subscribe<ViewNavigationArgs>(this);
            EventAggregator.Subscribe<GoBack>(this);
            base._Initialize();
            EventAggregator.Publish("Navigation".AsViewNavigationArgs());
        }

        public IActionCommand BackCommand { get; private set; }

        public void HandleEvent(ViewNavigationArgs publishedEvent)
        {
            if (publishedEvent.Deactivate)
            {
                return;
            }

            var viewInfo = (from vi in ((ViewModelRouter) Router).Views
                            where vi.Metadata.ExportedViewType.Equals(publishedEvent.ViewType)
                            select vi.Metadata)
                            .FirstOrDefault();

            if (viewInfo == null || !viewInfo.Category.Equals("Content")) return;

            if (publishedEvent.ViewType.Equals(_currentView)) return;

            if (!string.IsNullOrEmpty(_currentView))
            {
                EventAggregator.Publish(new ViewNavigationArgs(_currentView) {Deactivate = true});

                if (_goingBack)
                {
                    _goingBack = false;
                }
                else
                {
                    _history.Push(_currentView);
                }
            }

            BackCommand.RaiseCanExecuteChanged();

            _currentView = publishedEvent.ViewType;
        }

        public void HandleEvent(GoBack publishedEvent)
        {
            if (_history.Count < 1)
            {
                return; 
            }

            var view = _history.Pop();

            _goingBack = true;

            EventAggregator.Publish(view.AsViewNavigationArgs());
        }
    }
}