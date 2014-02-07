using System;
using System.Linq.Expressions;
using JounceDesktop.Core.Model;

namespace Jounce.Tests.Helpers
{
    /// <summary>
    /// Helper class to test the abstract <see cref="BaseNotify"/> class
    /// </summary>
    public class BaseNotifyTestHelper : BaseNotify
    {
        public const string PROPERTY_NAME = "TestProperty";

        public string TestProperty { get; set; }

        public static string StaticProperty { get; set; }

        public string Field;

        public new void RaisePropertyChanged(string propertyName)
        {
            base.RaisePropertyChanged(propertyName);
        }

        /// <summary>
        /// Raises this object's PropertyChanged event for each of the properties.
        /// </summary>
        /// <param name="propertyNames">The properties that have a new value.</param>
        public new void RaisePropertyChanged(params string[] propertyNames)
        {
            base.RaisePropertyChanged(propertyNames);
        }

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        public void RaisePropertyChangedWithExpression()
        {
            RaisePropertyChanged(() => TestProperty);
        }

        public void ExtractPropertyNameWithField()
        {
            ExtractPropertyName(() => Field);
        }

        public void ExtractPropertyNameWithStaticProperty()
        {
            ExtractPropertyName(() => StaticProperty);
        }

        /// <summary>
        /// Extracts the property name from the property expression
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="propertyExpression">An expression that evaluates to the property</param>
        /// <returns>The property name</returns>
        /// <remarks>
        /// Use this to take an expression like <code>() => MyProperty</code> and evaluate to the
        /// string "MyProperty"
        /// </remarks>
        public new string ExtractPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            return base.ExtractPropertyName(propertyExpression);
        }
    }
}