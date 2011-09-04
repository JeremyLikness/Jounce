using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Controls;
using Jounce.Core.Application;
using Jounce.Core.Event;
using Jounce.Core.Fluent;
using Jounce.Core.View;
using Jounce.Core.ViewModel;

namespace Jounce.Framework.View
{
    /// <summary>
    ///     The view router is responsible for on-demand loading
    ///     It listens to view events, asks the deployment service to load, then
    ///     activates the view
    /// </summary>
    [Export]
    [Export(typeof(IFluentViewXapRouter))]
    public class ViewRouter : IFluentViewXapRouter, IPartImportsSatisfiedNotification, IEventSink<ViewNavigationArgs>
    {
        private bool _initialized;

        /// <summary>
        ///     The deployment service
        /// </summary>
        [Import]
        public IDeploymentService DeploymentService { get; set; }

        /// <summary>
        ///     The router
        /// </summary>
        [Import]
        public IViewModelRouter ViewModelRouter { get; set; }
        
        /// <summary>
        ///     View locations
        /// </summary>
        [ImportMany(AllowRecomposition = true)]
        public ViewXapRoute[] ViewLocations { get; set; }

        private readonly List<ViewXapRoute> _fluentRoutes = new List<ViewXapRoute>();

        /// <summary>
        ///     Event aggregator
        /// </summary>
        [Import]
        public IEventAggregator EventAggregator { get; set; }

        [Import(AllowDefault = true,AllowRecomposition = true)]
        public ILogger Logger { get; set; }

        /// <summary>
        /// Called when a part's imports have been satisfied and it is safe to use.
        /// </summary>
        public void OnImportsSatisfied()
        {
            if (_initialized)
            {
                return;
            }

            _initialized = true;

            EventAggregator.SubscribeOnDispatcher(this);
        }

        /// <summary>
        ///     Hook into navigation event
        /// </summary>
        /// <param name="e">View navigation args</param>
        public void HandleEvent(ViewNavigationArgs e)
        {
            if (e.Deactivate)
            {
                ViewModelRouter.DeactivateView(e.ViewType);
                EventAggregator.Publish(new ViewNavigatedArgs(e.ViewType){Deactivate = true});
                return;
            }

            // does a view location exist?
            var viewLocation =
                (from location in _fluentRoutes
                 where location.ViewName.Equals(e.ViewType, StringComparison.InvariantCultureIgnoreCase)
                 select location).FirstOrDefault() ??
                (from location in ViewLocations
                                where location.ViewName.Equals(e.ViewType, StringComparison.InvariantCultureIgnoreCase)
                                select location).FirstOrDefault();

            // if so, try to load the xap, then activate the view
            if (viewLocation != null)
            {
                DeploymentService.RequestXap(viewLocation.ViewXap,
                                             exception =>
                                                 {
                                                     if (exception != null)
                                                     {
                                                         throw exception;
                                                     }
                                                     _ActivateView(e.ViewType, e.ViewParameters);
                                                 });
            }
            else
            {
                // just activate the view directly
                _ActivateView(e.ViewType, e.ViewParameters);
            }
        }       

        /// <summary>
        ///     Activate the view
        /// </summary>
        /// <param name="viewName">The name of the view</param>
        /// <param name="parameters">Parameters for the view</param>
        private void _ActivateView(string viewName, IDictionary<string, object> parameters)
        {            
            ViewModelRouter.ActivateView(viewName, parameters);
            EventAggregator.Publish(new ViewNavigatedArgs(viewName));
        }

        public void RouteViewInXap(string view, string xap)
        {
            _fluentRoutes.Add(ViewXapRoute.Create(view, xap));
        }        
    }
}