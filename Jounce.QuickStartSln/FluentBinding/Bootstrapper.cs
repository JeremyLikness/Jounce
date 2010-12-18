using System.ComponentModel.Composition;
using Jounce.Core.Application;
using Jounce.Core.Fluent;

namespace FluentBinding
{
    /// <summary>
    ///     Module initializer is called at the start of the application and whenever a XAP is dynamically
    ///     loaded that exports this. In our case, we are simply going to set up the route for the
    ///     view and view model here using the fluent interface.
    /// </summary>
    [Export(typeof(IModuleInitializer))]
    public class Bootstrapper : IModuleInitializer 
    {
        public bool Initialized { get; set; }

        [Import]
        public IFluentViewModelRouter Router { get; set; }

        public void Initialize()
        {
            Router.RouteViewModelForView("MainViewModel","MainView");
        }
    }
}