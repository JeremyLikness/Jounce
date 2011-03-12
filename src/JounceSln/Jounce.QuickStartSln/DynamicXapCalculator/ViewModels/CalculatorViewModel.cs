using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Jounce.Core.Application;
using Jounce.Core.Command;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;

namespace DynamicXapCalculator.ViewModels
{
    /// <summary>
    ///     Simple calculator
    /// </summary>
    [ExportAsViewModel("Calc")]
    public class CalculatorViewModel : BaseViewModel
    {
        /// <summary>
        ///     Old value on the "stack"
        /// </summary>
        private int _oldValue;

        /// <summary>
        ///     Need this for explicit module loading
        /// </summary>
        [Import]
        public IDeploymentService Deployment { get; set; }

        public CalculatorViewModel()
        {
            Value = "0";
            EqualCommand = new ActionCommand<object>(obj => ExecuteCommand((oldValue, newValue) => newValue),CanExecute);
            LoadCommand = new ActionCommand<object>(obj => _Load(), obj=>!_loaded);
        }

        private bool _loaded;

        /// <summary>
        ///     Load the dynamic XAP
        /// </summary>
        private void _Load()
        {
            // need to make sure this is mispelled correctly :)
            Deployment.RequestXap("DynamicXapCalculatorAdvanced.xap",ex=>
                                                                            {
                                                                                if (ex == null) return;
                                                                                _loaded = true;
                                                                                ((IActionCommand)LoadCommand).RaiseCanExecuteChanged();
                                                                            });

        }

        /// <summary>
        ///     List of commands to operate on
        /// </summary>
        [ImportMany(AllowRecomposition = true)]
        public ObservableCollection<Tuple<string,ICommand>> Commands { get; set; }

        [Export]
        public Action<Func<int,int,int>> ExecuteCommandExport
        {
            get { return ExecuteCommand; }
        }

        /// <summary>
        ///     Last command
        /// </summary>
        private Func<int, int, int> _lastCommand = (oldValue, newValue) => newValue;
        
        private int _lastValue;

        /// <summary>
        ///     Current value in the text box
        /// </summary>
        private string _value;
        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                RaisePropertyChanged(()=>Value);
                int.TryParse(value, out _lastValue);
            }
        }

        /// <summary>
        ///     True if the command can be executed (there is a valid integer)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool CanExecute(object obj)
        {
            int temp;
            return int.TryParse(Value, out temp);
        }

        /// <summary>
        ///     Perform the operation
        /// </summary>
        /// <param name="command">The command to perform</param>
        public void ExecuteCommand(Func<int, int, int> command)
        {
            if (!CanExecute(null)) return;

            var temp = _lastValue;
            Value = _lastCommand(_oldValue, _lastValue).ToString();
            _lastCommand = command;
            _oldValue = temp;
        }

        /// <summary>
        ///     Equal command - always the same
        /// </summary>
        public ICommand EqualCommand { get; private set; }

        /// <summary>
        ///     Load command - fetch the advanced features
        /// </summary>
        public ICommand LoadCommand { get; private set; }

        /// <summary>
        ///     The addition command - export it so it imports into the list above (the Commands list)
        /// </summary>
        [Export]
        public Tuple<string,ICommand> AddCommand
        {
            get
            {
                return Tuple.Create("+",
                                    (ICommand)new ActionCommand<object>(
                                        obj => ExecuteCommand((oldValue, newValue) => oldValue + newValue)));
            }
        }

        /// <summary>
        ///     Subtraction - export same as addition
        /// </summary>
        [Export]
        public Tuple<string, ICommand> SubstractCommand
        {
            get
            {
                return Tuple.Create("-",
                                    (ICommand)new ActionCommand<object>(
                                        obj => ExecuteCommand((oldValue, newValue) => oldValue - newValue)));
            }
        }
        
        
    }
}