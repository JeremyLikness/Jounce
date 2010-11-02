using System.ComponentModel.Composition;
using System.Windows.Controls;
using Jounce.Core.Event;

namespace SilverlightNavigation
{
    public partial class App : IEventSink<UnhandledExceptionEvent>
    {
        [Import]
        public IEventAggregator EventAggregator { get; set; }

        public App()
        {            
            InitializeComponent();
            Startup += (o, e) =>
                           {
                               CompositionInitializer.SatisfyImports(this);
                               EventAggregator.SubscribeOnDispatcher(this);
                           };
        }             

        public void HandleEvent(UnhandledExceptionEvent publishedEvent)
        {
            ChildWindow errorWin = new ErrorWindow(publishedEvent.UncaughtException);
            errorWin.Show();
        }
    }
}