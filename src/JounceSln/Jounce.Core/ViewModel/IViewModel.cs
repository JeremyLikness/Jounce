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