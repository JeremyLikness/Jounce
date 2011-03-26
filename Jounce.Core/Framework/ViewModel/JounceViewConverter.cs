using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Jounce.Core.ViewModel;

namespace Jounce.Framework.ViewModel
{
    /// <summary>
    ///     Jounce view converter
    /// </summary>
    public class JounceViewConverter : IValueConverter 
    {
        /// <summary>
        ///     Single instance for composition 
        /// </summary>
        private static readonly JounceViewConverter _composedConverter = new JounceViewConverter();

        /// <summary>
        ///     The view model router
        /// </summary>
        [Import]
        public IViewModelRouter Router { get; set; }
        
        /// <summary>
        /// Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        /// <returns>
        /// The value to be passed to the target dependency property.
        /// </returns>
        /// <param name="value">The source data being passed to the target.</param><param name="targetType">The <see cref="T:System.Type"/> of data expected by the target dependency property.</param><param name="parameter">An optional parameter to be used in the converter logic.</param><param name="culture">The culture of the conversion.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (DesignerProperties.IsInDesignTool)
            {
                return value;
            }

            lock (_composedConverter)
            {
                if (_composedConverter.Router == null)
                {
                    CompositionInitializer.SatisfyImports(_composedConverter);
                }
            }

            // if the value is a view model and a view tag was passed, resolve them
            if (value is IViewModel)
            {
                // first get the view model tag 
                var exportAttribute =
                    (from c in value.GetType().GetCustomAttributes(true) where c is ExportAsViewModelAttribute select (ExportAsViewModelAttribute)c)
                        .FirstOrDefault();

                if (exportAttribute == null)
                {
                    return value;
                }

                var viewModelTag = exportAttribute.ViewModelType;
                var views = _composedConverter.Router.GetViewTagsForViewModel(viewModelTag);

                // we always assume the first view
                if (views.Length > 0)
                {
                    return _composedConverter.Router.GetNonSharedView(views[0], value);
                }
            }

            return value;
        }

        /// <summary>
        /// Modifies the target data before passing it to the source object.  This method is called only in <see cref="F:System.Windows.Data.BindingMode.TwoWay"/> bindings.
        /// </summary>
        /// <returns>
        /// The value to be passed to the source object.
        /// </returns>
        /// <param name="value">The target data being passed to the source.</param><param name="targetType">The <see cref="T:System.Type"/> of data expected by the source object.</param><param name="parameter">An optional parameter to be used in the converter logic.</param><param name="culture">The culture of the conversion.</param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}