using System.Windows;
using Jounce.Core.Event;

namespace VSMAggregator
{
    public partial class App : IEventSink<UnhandledExceptionEvent>
    {
        public App()
        {
            InitializeComponent();
        }

        public void HandleEvent(UnhandledExceptionEvent publishedEvent)
        {
            publishedEvent.Handled = true;
            MessageBox.Show(publishedEvent.UncaughtException.ToString());
        }
    }
}
