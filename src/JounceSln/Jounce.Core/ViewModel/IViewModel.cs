using System;
using System.ComponentModel;

namespace Jounce.Core.ViewModel
{
    /// <summary>
    ///     View model interface
    /// </summary>
    public interface IViewModel : INotifyPropertyChanged
    {
        /// <summary>
        ///     Go to visual state action
        /// </summary>
        Action<string,bool> GoToVisualState { get; set; }

        /// <summary>
        ///     Visual state for a specific view
        /// </summary>
        /// <param name="view">The view</param>
        /// <param name="state">The state</param>
        /// <param name="useTransitions">Use transitions?</param>
        /// <returns>True if the view is registered</returns>
        bool GoToVisualStateForView(string view, string state, bool useTransitions);

        /// <summary>
        ///     Called first time the view model is created
        /// </summary>
        void Initialize();

        /// <summary>
        ///     Called whenever the view model has a corresponding view come into focus
        /// </summary>
        void Activate(string viewName);

        /// <summary>
        ///     Called whenever a corresponding view goes out of focus
        /// </summary>
        void Deactivate(string viewName);        
    }
}