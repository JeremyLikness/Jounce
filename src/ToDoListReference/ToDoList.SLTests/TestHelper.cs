using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace ToDoList.SLTests
{
    public static class TestHelper
    {
        /// <summary>
        ///     Returns true if dates are within 1 second of each other
        /// </summary>
        /// <param name="date">Source date</param>
        /// <param name="compareDate">Date to compare to</param>
        /// <returns>True if they are within 1 second of each other</returns>
        public static bool IsCloseTo(this DateTime date, DateTime compareDate)
        {
            var diff = Math.Abs((compareDate - date).TotalMilliseconds);
            return diff <= 1000;
        }

        public static IEnumerable<DependencyObject> FullTree(DependencyObject parent)
        {
            yield return parent;

            var childrenCount = VisualTreeHelper
                .GetChildrenCount(parent);
            for (var x = 0; x < childrenCount; x++)
            {
                var child = VisualTreeHelper
                    .GetChild(parent, x);
                foreach (var grandChild 
                    in FullTree(child))
                {
                    yield return grandChild;
                }
            }
        }

        public static IEnumerable<T> ElementsOfType<T>(
            DependencyObject parent) 
            where T : DependencyObject
        {
            return from element in FullTree(parent)
                    where element is T
                    select element as T;
        }

        public static T FindInChildrenByName<T>
            (DependencyObject parent, 
            string name) 
            where T : FrameworkElement
        {
            return (from element in ElementsOfType<T>(parent)
                    where element.Name.Equals(name, 
                    StringComparison.InvariantCultureIgnoreCase)
                    select element).First();
        }
    }
}