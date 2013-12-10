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
    /// <remarks>
    /// This is the main router to locate and parse views.
    /// </remarks>
    [Export]
    [Export(typeof(IFluentViewXapRouter))]
    public class ViewRouter : IFluentViewXapRouter, IPartImportsSatisfiedNotification, IEventSink<ViewNavigationArgs>
    {
        private bool _initialized;

        /// <summary>
        /// The deployment service reference to <see cref="IDeploymentService"/>
        /// </summary>
        [Import]
        public IDeploymentService DeploymentService { get; set; }

        /// <summary>
        /// The instance of the <see cref="IViewModelRouter"/>
        /// </summary>
        [Import]
        public IViewModelRouter ViewModelRouter { get; set; }
        
        /// <summary>
        ///  A list of view locations using the <see cref="ViewXapRoute"/>
        /// </summary>
        [ImportMany(AllowRecomposition = true)]
        public ViewXapRoute[] ViewLocations { get; set; }

        /// <summary>
        /// List of fluently configured <see cref="ViewXapRoute"/>
        /// </summary>
        private readonly List<ViewXapRoute> _fluentRoutes = new List<ViewXapRoute>();

        /// <summary>
        /// Event aggregator instance that implements <see cref="IEventAggregator"/>
        /// </summary>
        [Import]
        public IEventAggregator EventAggregator { get; set; }

        /// <summary>
        /// Instance of the <see cref="ILogger"/>
        /// </summary>
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

        /// <summary>
        /// Use to fluently route a view to a xap file
        /// </summary>
        /// <param name="view">The tag for the view</param>
        /// <param name="xap">The name of the XAP</param>
        public void RouteViewInXap(string view, string xap)
        {
            _fluentRoutes.Add(ViewXapRoute.Create(view, xap));
        }

        /// <summary>
        /// Use to fluently route a view type to a xap file
        /// </summary>
        /// <typeparam name="T">The type of the view (uses the full name for the tag)</typeparam>
        /// <param name="xap">The name of the XAP file</param>
        public void RouteViewInXap<T>(string xap) where T : UserControl
        {
            _fluentRoutes.Add(ViewXapRoute.Create<T>(xap));
        }
    }
}