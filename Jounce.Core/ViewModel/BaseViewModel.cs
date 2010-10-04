using System;
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
        /// <summary>
        ///     Binder to go to a visual state 
        /// </summary>
        public Action<string, bool> GoToVisualState { get; set; }

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
            _Initialize();
        }

        public virtual void _Initialize()
        {
            
        }

        /// <summary>
        ///     Called whenever the view model has a corresponding view come into focus
        /// </summary>
        public void Activate(string viewName)
        {
            Logger.LogFormat(LogSeverity.Information, GetType().FullName, "{0} [{1}]", MethodBase.GetCurrentMethod().Name, viewName);
            _Activate(viewName);
        }

        public virtual void _Activate(string viewName)
        {
            
        }

        /// <summary>
        ///     Called whenever a corresponding view goes out of focus
        /// </summary>
        public void Deactivate(string viewName)
        {
            Logger.LogFormat(LogSeverity.Information, GetType().FullName, "{0} [{1}]", MethodBase.GetCurrentMethod().Name, viewName);
            _Deactivate(viewName);
        }       

        public virtual void _Deactivate(string viewName)
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