﻿using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Jounce.Core.Model
{
    /// <summary>
    ///     Base class for items that implement property changed
    /// </summary>
    public abstract class BaseNotify : INotifyPropertyChanged
    {
        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Raises this object's PropertyChanged event for each of the properties.
        /// </summary>
        /// <param name="propertyNames">The properties that have a new value.</param>
        protected void RaisePropertyChanged(params string[] propertyNames)
        {
            foreach (var name in propertyNames)
            {
                RaisePropertyChanged(name);
            }
        }

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <typeparam name="T">The type of the property that has a new value</typeparam>
        /// <param name="propertyExpression">A Lambda expression representing the property that has a new value.</param>
        protected void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            var propertyName = ExtractPropertyName(propertyExpression);
            RaisePropertyChanged(propertyName);
        }

        protected string ExtractPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException("propertyExpression");
            }

            var memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException(Resources.BaseNotify_ExtractPropertyName_NotAMember, "propertyExpression");
            }

            var property = memberExpression.Member as PropertyInfo;
            if (property == null)
            {
                throw new ArgumentException(Resources.BaseNotify_ExtractPropertyName_NotAProperty, "propertyExpression");
            }
            
            var getMethod = property.GetGetMethod(true);

            if (getMethod == null)
            {
                // this shouldn't happen - the expression would reject the property before reaching this far
                throw new ArgumentException(Resources.BaseNotify_ExtractPropertyName_NoGetter, "propertyExpression");
            }

            if (getMethod.IsStatic)
            {
                throw new ArgumentException(Resources.BaseNotify_ExtractPropertyName_Static, "propertyExpression");
            }

            return memberExpression.Member.Name;
        }
    }
}