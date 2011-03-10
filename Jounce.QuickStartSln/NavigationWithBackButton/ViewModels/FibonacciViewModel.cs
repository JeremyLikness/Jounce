using System;
using System.Collections.Generic;
using System.Linq;
using Jounce.Core.ViewModel;
using Jounce.Core.Workflow;
using Jounce.Framework;
using Jounce.Framework.Workflow;

namespace NavigationWithBackButton.ViewModels
{
    [ExportAsViewModel("Fib")]
    public class FibonacciViewModel : ContentViewModel  
    {
        private int _lastNumber = 1;
        private int _currentNumber = 1;

        private readonly List<string> _sequence = new List<string>();
        
        public IEnumerable<string> Sequence
        {
            get { return from s in _sequence select s; }
        }

        public FibonacciViewModel()
        {
            if (InDesigner)
            {
                _sequence.AddRange(new[] { "1", "1", "2", "3", "5", "8", "13", "21", "34", "55"});
            }
            else
            {
                _sequence.Add(_lastNumber.ToString());
                _sequence.Add(_currentNumber.ToString());

                while (_sequence.Count < 9)
                {
                    _Iterate();
                }    
            }
        }

        public override void _Initialize()
        {
            base._Initialize();   
        
            WorkflowController.Begin(_GetSequence());
        }    
    
        private IEnumerable<IWorkflow> _GetSequence()
        {
            var workflowDelay = new WorkflowDelay(TimeSpan.FromSeconds(1));

            while (true)
            {
                yield return workflowDelay;
                _Iterate();
                JounceHelper.ExecuteOnUI(()=>RaisePropertyChanged(()=>Sequence));
            }
        }


        private void _Iterate()
        {
            var nextNumber = _lastNumber + _currentNumber;
            _lastNumber = _currentNumber;
            _currentNumber = nextNumber;
            _sequence.Add(nextNumber.ToString());
            if (_sequence.Count == 10)
            {
                _sequence.RemoveAt(0);
            }
        }
    }
}