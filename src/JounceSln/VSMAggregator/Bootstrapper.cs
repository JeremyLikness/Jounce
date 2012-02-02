using System.ComponentModel.Composition;
using System.Windows;
using Jounce.Core.Application;
using Jounce.Core.Event;
using Jounce.Core.Fluent;

namespace VSMAggregator
{
    [Export(typeof(IModuleInitializer))]
    public class Bootstrapper : IModuleInitializer 
    {
        [Import]
        public IEventAggregator EventAggregator { get; set; }

        [Import]
        public IFluentViewModelRouter ViewModelRouter { get; set; }

        public bool Initialized { get; private set; }
        
        public void Initialize()
        {
            // set up App to handle exceptions
            EventAggregator.SubscribeOnDispatcher((App) Application.Current);

            // route the view models
            ViewModelRouter.RouteViewModelForView(Globals.VIEWMODEL_MAIN, Globals.VIEW_MAINPAGE);
            ViewModelRouter.RouteViewModelForView(Globals.VIEWMODEL_RED, Globals.VIEW_RED);
            ViewModelRouter.RouteViewModelForView(Globals.VIEWMODEL_BLUE, Globals.VIEW_BLUE);

            Initialized = true;
        }
    }
}