using System;
using System.Windows.Threading;
using Jounce.Core.Workflow;

namespace Jounce.Framework.Workflow
{
    /// <summary>
    ///     Injects a delay into the workflow
    /// </summary>
    public class WorkflowDelay : IWorkflow 
    {
        private readonly DispatcherTimer _timer;

        public WorkflowDelay(TimeSpan interval)
        {
            if (interval <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException("interval");
            }

            _timer = new DispatcherTimer();
            _timer.Tick += TimerTick;
            _timer.Interval = interval;
        }

        void TimerTick(object sender, EventArgs e)
        {
            _timer.Stop();
            _timer.Tick -= TimerTick;
            Invoked();
        }

        public void Invoke()
        {
            _timer.Start();
        }

        public Action Invoked { get; set; }        
    }
}