using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Windows.Data;
using Jounce.Core.ViewModel;
using ToDoList.Contracts;
using ToDoList.ViewModels;

namespace ToDoList.Converters
{
    public class ToDoListItemToViewModelConverter : IValueConverter
    {
        private static IViewModelRouter _router;

        [Import]
        public IViewModelRouter Router { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is IToDoItem) || DesignerProperties.IsInDesignTool)
            {
                return null;
            }

            if (_router == null)
            {
                CompositionInitializer.SatisfyImports(this);
                _router = Router;
            }            
            
            var vm = _router.GetNonSharedViewModel<ToDoListItemViewModel>();            
            vm.Task = value as IToDoItem;
            return vm;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}