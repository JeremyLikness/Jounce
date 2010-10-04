using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;

namespace Jounce.Framework.ViewModels
{
    /// <summary>
    ///     Base class for view models that maintain "dirty" state (commit) and/or require validation
    /// </summary>
    /// <remarks>
    ///     This class provides basic support for implementing the <see cref="INotifyPropertyChanged"/> and 
    /// <see cref="INotifyDataErrorInfo"/> interfaces.
    /// </remarks>
    public abstract class BaseEntityViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        protected BaseEntityViewModel()
        {            
            // can only commit if no errors
            CommitCommand = new ActionCommand<object>(
                obj =>
                {
                    _ValidateAll();

                    if (HasErrors) return;
                    
                    _OnCommitted();
                    
                    Committed = true;
                },
                obj => !HasErrors && !Committed);

            // change validity for commit whenever the error condition changes
            ErrorsChanged += (o, e) => CommitCommand.RaiseCanExecuteChanged();

            // any time a property changes that is not the committed flag, reset committed
            PropertyChanged += (o, e) =>
            {
                if (Committed && !e.PropertyName.Equals(ExtractPropertyName(() => Committed)))
                {
                    Committed = false;
                }
            };
        }

        /// <summary>
        ///     Use this to validate any late-bound fields before committing
        /// </summary>
        protected virtual void _ValidateAll()
        {
            
        }

        /// <summary>
        ///     Use this to perform whatever action is needed when committed
        /// </summary>
        protected virtual void _OnCommitted()
        {
            
        }

        /// <summary>
        ///     True when changes have been committed
        /// </summary>
        private bool _committed = true;

        /// <summary>
        ///     This value will raise property changed when committed without errors
        /// </summary>
        public bool Committed
        {
            get { return _committed; }
            set
            {
                _committed = value;
                CommitCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(() => Committed);
            }
        }

        /// <summary>
        ///     Commit changes
        /// </summary>
        public ActionCommand<object> CommitCommand { get; private set; }

        /// <summary>
        ///     Internal list of errors
        /// </summary>
        private readonly Dictionary<string, IEnumerable<string>> _errors = new Dictionary<string, IEnumerable<string>>();

        /// <summary>
        ///     Collction of errors changed
        /// </summary>
        private event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;        

        /// <summary>
        /// Occurs when the validation errors have changed for a property or for the entire object.
        /// </summary>
        event EventHandler<DataErrorsChangedEventArgs> INotifyDataErrorInfo.ErrorsChanged
        {
            [method:
                SuppressMessage("Microsoft.Design", "CA1033", Justification = "Doesn't need to access event methods.")]
            add { ErrorsChanged += value; }
            [method:
                SuppressMessage("Microsoft.Design", "CA1033", Justification = "Doesn't need to access event methods.")]
            remove { ErrorsChanged -= value; }
        }

        /// <summary>
        ///     True if errors exist
        /// </summary>
        bool INotifyDataErrorInfo.HasErrors
        {
            get { return HasErrors; }
        }

        /// <summary>
        ///     True if errors exist
        /// </summary>
        protected bool HasErrors
        {
            get { return _errors.Any(); }
        }

        /// <summary>
        ///     Get errors for a property
        /// </summary>
        /// <param name="propertyName">The property</param>
        /// <returns>The list of errors</returns>
        IEnumerable INotifyDataErrorInfo.GetErrors(string propertyName)
        {
            return GetErrors(propertyName);
        }

        /// <summary>
        ///     Get errors for a property
        /// </summary>
        /// <param name="propertyName">The name of the property</param>
        /// <returns></returns>
        protected virtual IEnumerable GetErrors(string propertyName)
        {
            IEnumerable<string> error;
            return _errors.TryGetValue(propertyName ?? string.Empty, out error) ? error : null;
        }

        /// <summary>
        ///     Set an error for a property
        /// </summary>
        /// <param name="propertyName">The name of the property</param>
        /// <param name="error">The error</param>
        protected virtual void SetError(string propertyName, string error)
        {
            SetErrors(propertyName, new List<string> {error});
        }

        /// <summary>
        ///     Overload for expression
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="propertyExpresssion">An expression that points to the property</param>
        /// <param name="error">The error</param>
        protected virtual void SetError<T>(Expression<Func<T>> propertyExpresssion, string error)
        {
            var propertyName = ExtractPropertyName(propertyExpresssion);
            SetError(propertyName, error);
        }       

        /// <summary>
        ///     Clears the errors
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void ClearErrors(string propertyName)
        {
            SetErrors(propertyName, new List<string>());
        }

        /// <summary>
        ///     Clear all errors for a property
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="propertyExpresssion">The expression that points to the property</param>
        protected virtual void ClearErrors<T>(Expression<Func<T>> propertyExpresssion)
        {
            var propertyName = ExtractPropertyName(propertyExpresssion);
            ClearErrors(propertyName);
        }

        /// <summary>
        ///     Set errors for a property
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="propertyExpresssion">The expression for the property</param>
        /// <param name="propertyErrors">The collection of errors</param>
        protected virtual void SetErrors<T>(Expression<Func<T>> propertyExpresssion, IEnumerable<string> propertyErrors)
        {
            var propertyName = ExtractPropertyName(propertyExpresssion);
            SetErrors(propertyName, propertyErrors);
        }

        /// <summary>
        ///     Set errors for a property
        /// </summary>
        /// <param name="propertyName">The name of the property</param>
        /// <param name="propertyErrors">The collection of errors</param>
        protected virtual void SetErrors(string propertyName, IEnumerable<string> propertyErrors)
        {
            if (propertyErrors.Any(error => error == null))
            {
                throw new ArgumentException(Core.Resources.BaseViewModel_SetErrors_NoNullErrors, "propertyErrors");
            }

            var propertyNameKey = propertyName ?? string.Empty;

            IEnumerable<string> currentPropertyErrors;
            if (_errors.TryGetValue(propertyNameKey, out currentPropertyErrors))
            {
                if (!AreErrorCollectionsEqual(currentPropertyErrors, propertyErrors))
                {
                    if (propertyErrors.Any())
                    {
                        _errors[propertyNameKey] = propertyErrors;
                    }
                    else
                    {
                        _errors.Remove(propertyNameKey);
                    }

                    RaiseErrorsChanged(propertyNameKey);
                }
            }
            else
            {
                if (propertyErrors.Any())
                {
                    _errors[propertyNameKey] = propertyErrors;
                    RaiseErrorsChanged(propertyNameKey);
                }
            }
        }

        /// <summary>
        ///     Raises this object's ErrorsChangedChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has new errors.</param>
        protected virtual void RaiseErrorsChanged(string propertyName)
        {
            var handler = ErrorsChanged;
            if (handler != null)
            {
                handler(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        ///     Raises this object's ErrorsChangedChanged event.
        /// </summary>
        /// <typeparam name="T">The type of the property that has errors</typeparam>
        /// <param name="propertyExpresssion">A Lambda expression representing the property that has new errors.</param>
        protected virtual void RaiseErrorsChanged<T>(Expression<Func<T>> propertyExpresssion)
        {
            var propertyName = ExtractPropertyName(propertyExpresssion);
            RaiseErrorsChanged(propertyName);
        }

        /// <summary>
        ///     Compares error collections
        /// </summary>
        /// <param name="propertyErrors">The property errors</param>
        /// <param name="currentPropertyErrors">The current</param>
        /// <returns>True if there are/aren't equal</returns>
        private static bool AreErrorCollectionsEqual(IEnumerable<string> propertyErrors,
                                                     IEnumerable<string> currentPropertyErrors)
        {
            var equals = currentPropertyErrors.Zip(propertyErrors, (current, newError) => current == newError);
            return propertyErrors.Count() == currentPropertyErrors.Count() && equals.All(b => b);
        }
    }
}