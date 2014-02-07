using System;
using JounceDesktop.Core.Command;

namespace JounceDesktop.Framework.Command
{
    /// <summary>
    ///     Delegate command - does it all
    /// </summary>
    /// <remarks>
    /// Base for setting up commands
    /// </remarks>
    public class ActionCommand<T> : IActionCommand<T>
    {
        /// <summary>
        /// Action to perform (defaults to empty)
        /// </summary>
        private Action<T> _execute = obj => { };

        /// <summary>
        /// Function to determine if the action can execute
        /// </summary>
        private readonly Func<T,bool> _canExecute = obj => true;

        /// <summary>
        /// Set to true if the original action was overridden
        /// </summary>
        public bool Overridden { get; set; }

        /// <summary>
        /// Default constructor - do nothing
        /// </summary>
        public ActionCommand()
        {
            
        }

        /// <summary>
        ///     Override the action
        /// </summary>
        /// <param name="action">The action to override with</param>
        /// <remarks>
        /// Changes the function of the action after its initial definition
        /// </remarks>
        public void OverrideAction(Action<T> action)
        {
            _execute = action;
            Overridden = true;
        }

        /// <summary>
        ///     Constructor with action to perform
        /// </summary>
        /// <param name="execute">The action to execute</param>
        public ActionCommand(Action<T> execute)
        {
            _execute = execute;
        }

        /// <summary>
        ///     Constructor with action and condition
        /// </summary>
        /// <param name="execute">The action to execute</param>
        /// <param name="canExecute">A function to determine whether execution is allowed</param>
        public ActionCommand(Action<T> execute, Func<T,bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null. </param>
        public bool CanExecute(object parameter)
        {
            return _canExecute((T) parameter);
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null. </param>
        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                _execute((T) parameter);
            }
        }

        /// <summary>
        /// Use this to indicate the action should reevaluate whether or not it can execute
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raised when the state of execution has changed
        /// </summary>
        public event EventHandler CanExecuteChanged;
    }
}
