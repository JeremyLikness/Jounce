using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ToDoList.Converters
{
    public class BooleanToStatusBrushConverter : IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (DesignerProperties.IsInDesignTool)
            {
                return new SolidColorBrush(Colors.Magenta);
            }
            var defaultValue = Application.Current.Resources["HighlightBrushOrange"];
            if (value is bool && (bool)value)
            {
                defaultValue = Application.Current.Resources["HighlightBrushGreen"];
            }
            return defaultValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}