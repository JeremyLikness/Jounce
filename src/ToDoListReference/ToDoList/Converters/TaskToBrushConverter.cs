using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using ToDoList.Contracts;

namespace ToDoList.Converters
{
    public class TaskToBrushConverter : IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var retVal = new SolidColorBrush(Colors.Black);

            var task = value as IToDoItem;

            if (task != null)
            {
                if (task.IsComplete)
                {
                    retVal = new SolidColorBrush(Colors.Green);
                }
                else if (task.IsPastDue)
                {
                    retVal = new SolidColorBrush(Colors.Red);
                }
                else if (task.IsDueTomorrow)
                {
                    retVal = new SolidColorBrush(Colors.Orange);
                }
                else if (task.IsDueNextWeek)
                {
                    retVal = new SolidColorBrush(Colors.Yellow);
                }
                else
                {
                    retVal = new SolidColorBrush(Colors.Gray);
                }
            }

            return retVal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}