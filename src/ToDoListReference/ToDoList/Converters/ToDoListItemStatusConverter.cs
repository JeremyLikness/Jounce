using System;
using System.Globalization;
using System.Windows.Data;
using ToDoList.Contracts;

namespace ToDoList.Converters
{
    public class ToDoListItemStatusConverter : IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var task = value as IToDoItem;
            if (task == null)
            {
                return string.Empty;
            }
            var template = task.IsComplete ? 
                "Completed: {0}" : "Due: {0}";
            var date = task.IsComplete ? 
                task.CompletedDate.ToShortDateString() : 
                task.DueDate.ToShortDateString();
            var status = string.Format(template, date);
            return status;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}