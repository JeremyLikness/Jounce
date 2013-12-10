using System;
using System.Windows.Threading;
using Jounce.Core.Workflow;

namespace Jounce.Framework.Workflow
{
    /// <summary>
    ///     Injects a delay into the workflow
    /// </summary>
    /// <remarks>
    /// Yield return an instance to introduce a delay
    /// </remarks>
    public class WorkflowDelay : IWorkflow 
    {
        /// <summary>
        /// The timer
        /// </summary>
        private readonly DispatcherTimer _timer;

        /// <summary>
        /// Constructor with the delay 
        /// </summary>
        /// <param name="interval">The delay</param>
        public WorkflowDelay(TimeSpan interval)
        {
            if (interval <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException("interval");
            }

            _timer = new DispatcherTimer {Interval = interval};
        }

        /// <summary>
        /// Method to call when fired, will stop the sequence and move to the next step
        /// </summary>
        /// <param name="sender">The timer</param>
        /// <param name="e">Parameters</param>
        void TimerTick(object sender, EventArgs e)
        {
            _timer.Stop();
            _timer.Tick -= TimerTick;
            Invoked();
        }

        /// <summary>
        /// Invoke will kick off the timer
        /// </summary>
        public void Invoke()
        {
            _timer.Tick += TimerTick;            
            _timer.Start();
        }

        /// <summary>
        /// This is wired by the controller
        /// </summary>
        public Action Invoked { get; set; }        
    }
}