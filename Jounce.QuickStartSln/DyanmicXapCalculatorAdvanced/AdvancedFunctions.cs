using System;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Jounce.Framework.Command;

namespace DyanmicXapCalculatorAdvanced
{
    /// <summary>
    ///     Adds multiplication and division
    /// </summary>
    public class AdvancedFunctions
    {        
        [Import]
        public Action<Func<int, int, int>> ExecuteCommandImport { get; set; }

        [Export]
        public Tuple<string, ICommand> AddCommand
        {
            get
            {
                return Tuple.Create("*",
                                    (ICommand)new ActionCommand<object>(
                                        obj => 
                                            ExecuteCommandImport((oldValue, newValue) => oldValue * newValue)));
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
                                                ExecuteCommandImport((oldValue, newValue) => newValue == 0 ? 0 : oldValue / newValue)));
            }
        }
    }
}