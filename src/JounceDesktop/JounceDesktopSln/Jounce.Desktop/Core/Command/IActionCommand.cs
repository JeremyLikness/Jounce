using System;
using System.Windows.Input;

namespace JounceDesktop.Core.Command
{
    /// <summary>
    ///  Typed action command
    /// </summary>
    /// <typeparam name="T">The type to act against</typeparam>
    public interface IActionCommand<T> : IActionCommand
    {        
        /// <summary>
        ///     Override the action
        /// </summary>
        /// <param name="action"></param>
        void OverrideAction(Action<T> action);            
    }

    /// <summary>
    ///     Basic action command
    /// </summary>
    public interface IActionCommand : ICommand 
    {
        /// <summary>
        ///     True if the original command has been overridden
        /// </summary>
        bool Overridden { get; set; }

        /// <summary>
        ///     Raise the execute changed event
        /// </summary>
        void RaiseCanExecuteChanged();
    }
}