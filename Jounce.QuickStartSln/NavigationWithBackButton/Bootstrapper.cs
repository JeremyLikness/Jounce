using System.ComponentModel.Composition;
using Jounce.Core.Application;
using Jounce.Core.Fluent;

namespace NavigationWithBackButton
{
    [Export(typeof(IModuleInitializer))]
    public class Bootstrapper : IModuleInitializer
    {
        [Import]
        public IFluentViewModelRouter Router { get; set; }  

        public bool Initialized { get; private set; }

        public void Initialize()
        {
            Initialized = true;

            Router.RouteViewModelForView("Main", "Main");
            Router.RouteViewModelForView("Clock", "Clock");
            Router.RouteViewModelForView("Navigation", "Navigation");
            Router.RouteViewModelForView("Fib", "Fib");
            Router.RouteViewModelForView("Lorem", "Lorem");
        }
    }
}