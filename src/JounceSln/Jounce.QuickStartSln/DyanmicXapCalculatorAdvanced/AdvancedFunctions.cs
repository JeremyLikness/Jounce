using System;
using System.ComponentModel.Composition;
using System.Windows.Input;
using DynamicXapCalculator.ViewModels;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;

namespace DyanmicXapCalculatorAdvanced
{
    /// <summary>
    ///     Adds multiplication and division
    /// </summary>
    public class AdvancedFunctions
    {
        /// <summary>
        ///     Use this to get access back to the view model we are injecting to
        /// </summary>
        [Import]
        public IViewModelRouter Router { get; set; }

        [Export]
        public Tuple<string, ICommand> AddCommand
        {
            get
            {
                return Tuple.Create("*",
                                    (ICommand)new ActionCommand<object>(
                                        obj => 
                                            Router.ResolveViewModel<CalculatorViewModel>("Calc"). 
                                            ExecuteCommand((oldValue, newValue) => oldValue * newValue)));
            }
        }

        [Export]
        public Tuple<string, ICommand> SubstractCommand
        {
            get
            {
                return Tuple.Create("/",
                                    (ICommand)new ActionCommand<object>(
                                        obj =>
                                             Router.ResolveViewModel<CalculatorViewModel>("Calc").
                                            ExecuteCommand((oldValue, newValue) => newValue == 0 ? 0 : oldValue / newValue)));
            }
        }
    }
}