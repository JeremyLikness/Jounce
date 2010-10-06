using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Jounce.Core.Command;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;

namespace EventAggregator.ViewModels
{
    [ExportAsViewModel(Constants.VM_SENDER)]
    public class SenderViewModel : BaseViewModel 
    {
        /// <summary>
        ///     Indexer of color to brush
        /// </summary>
        /// <param name="color">The color</param>
        /// <returns>The brush</returns>
        public SolidColorBrush this[string color]
        {
            get
            {
                return new SolidColorBrush(_colors[color]);
            }
        }

        private readonly Dictionary<string, Color> _colors =
            new Dictionary<string, Color>
                {
                    {"Red", Colors.Red},
                    {"Orange", Colors.Orange},
                    {"Yellow", Colors.Yellow},
                    {"Green", Colors.Green},
                    {"Blue", Colors.Blue}
                };

        public SenderViewModel()
        {
            Options = new ObservableCollection<string>(_colors.Keys);
            OptionChangedCommand = new
                ActionCommand<string>(option => EventAggregator.Publish(option),
                                      option => !string.IsNullOrEmpty(option));
            ThrowErrorCommand = new
                ActionCommand<object>(obj => { throw new Exception("My Error was thrown!"); });
        }

        public ObservableCollection<string> Options { get; private set; }

        public IActionCommand<string> OptionChangedCommand { get; private set; }

        public IActionCommand<object> ThrowErrorCommand { get; private set; }

        private string _currentOption;
        public string CurrentOption
        {
            get { return _currentOption; }
            set
            {
                _currentOption = value;
                RaisePropertyChanged(()=>CurrentOption);
            }
        }
    }
}