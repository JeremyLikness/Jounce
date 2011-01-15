using System.Windows;
using Jounce.Core.Event;
using Jounce.Framework;

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
