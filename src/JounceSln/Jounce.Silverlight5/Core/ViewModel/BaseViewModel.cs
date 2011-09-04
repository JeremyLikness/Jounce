using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Reflection;
using Jounce.Core.Application;
using Jounce.Core.Event;
using Jounce.Core.Model;

namespace Jounce.Core.ViewModel
{
    /// <summary>
    ///     Base view model for data-binding
    /// </summary>
    public abstract class BaseViewModel : BaseNotify, IViewModel
    {
        protected BaseViewModel()
        {
            RegisteredViews = new List<string>();
        }        

        /// <summary>
        ///     Views that are registered with this view model
        /// </summary>
        public List<string> RegisteredViews { get; private set; }

        /// <summary>
        ///     Handle all visual states
        /// </summary>
        private readonly Dictionary<string,Action<string,bool>> _visualStates 
            = new Dictionary<string, Action<string, bool>>();

        /// <summary>
        ///     Binder to go to a visual state 
        /// </summary>
        public Action<string, bool> GoToVisualState { get; set; }

        /// <summary>
        ///     Allow registration to the visual state
        /// </summary>
        /// <param name="view">The view being registered</param>
        /// <param name="action">The visual state action</param>       
        public void RegisterVisualState(string view, Action<string,bool> action)
        {
            _visualStates.Add(view, action);
            if (GoToVisualState == null)
            {
                GoToVisualState = action;
            }
        }

        /// <summary>
        ///     Transition to the visual state for a view
        /// </summary>
        /// <param name="view">The view name</param>
        /// <param name="state">The state</param>
        /// <param name="useTransitions">True to use transitions</param>
        /// <returns>True if the view transition is found</returns>
        public bool GoToVisualStateForView(string view, string state, bool useTransitions)
        {
            if (_visualStates.ContainsKey(view))
            {
                _visualStates[view](state, useTransitions);
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Event aggregator
        /// </summary>
        [Import]
        public IEventAggregator EventAggregator { get; set; }

        /// <summary>
        ///     Router
        /// </summary>
        [Import]
        public IViewModelRouter Router { get; set; }

        /// <summary>
        ///     Logger
        /// </summary>
        [Import(AllowDefault = true,AllowRecomposition = true)]
        public ILogger Logger { get; set; }

        /// <summary>
        ///     True if in the designer
        /// </summary>
        protected bool InDesigner
        {
            get
            {
                return DesignerProperties.IsInDesignTool;
            }
        }

        /// <summary>
        ///     Called first time the view model is created
        /// </summary>
        public void Initialize()
        {            
            Logger.Log(LogSeverity.Information, GetType().FullName, MethodBase.GetCurrentMethod().Name);
            InitializeVm();
        }

        /// <summary>
        ///     Called the very first time the view model is created
        /// </summary>
        protected virtual void InitializeVm()
        {
            
        }

        /// <summary>
        ///     Called whenever the view model has a corresponding view come into focus
        /// </summary>
        public void Activate(string viewName)
        {
            Activate(viewName, new Dictionary<string, object>());
        }

        /// <summary>
        ///     Called whenever the view model has a corresponding view come into focus
        /// </summary>
        public void Activate(string viewName, IDictionary<string, object> parameters)
        {
            Logger.LogFormat(LogSeverity.Information, GetType().FullName, "{0} [{1}]", MethodBase.GetCurrentMethod().Name, viewName);
            ActivateView(viewName, parameters);
        }

        /// <summary>
        ///     Called when a view is activated through navigation
        /// </summary>
        /// <param name="viewName">The tag for the view</param>
        /// <param name="viewParameters">Parameters requested for the activation</param>
        protected virtual void ActivateView(string viewName, IDictionary<string, object> viewParameters)
        {
            
        }

        /// <summary>
        ///     Called whenever a corresponding view goes out of focus
        /// </summary>
        public void Deactivate(string viewName)
        {
            Logger.LogFormat(LogSeverity.Information, GetType().FullName, "{0} [{1}]", MethodBase.GetCurrentMethod().Name, viewName);
            DeactivateView(viewName);
        }       

        /// <summary>
        ///     Called when a view is deactivated through navigation
        /// </summary>
        /// <param name="viewName">The tag for the view</param>
        protected virtual void DeactivateView(string viewName)
        {
            
        }

        /// <summary>
        ///     Force the to-string implementation for easier debugging
        /// </summary>
        /// <returns>The overridden to string method</returns>
        public override string ToString()
        {
            return string.Format(Resources.BaseViewModel_ToString_ViewModel, GetType().Name);
        }
    }
}