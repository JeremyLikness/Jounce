using System;
using System.ComponentModel;
using Jounce.Core.Workflow;

namespace Jounce.Framework.Workflow
{
    /// <summary>
    ///     Workflow for running a background process
    /// </summary>
    /// <remarks>
    /// Use this to create a workflow item that runs in the background
    /// </remarks>
    public class WorkflowBackgroundWorker : IWorkflow 
    {
        /// <summary>
        /// The instance of the <see cref="BackgroundWorker"/>
        /// </summary>
        private readonly BackgroundWorker _bg;

        /// <summary>
        /// The action used to perform work
        /// </summary>
        private readonly Action<BackgroundWorker> _doWork;

        /// <summary>
        /// The action called to report progress
        /// </summary>
        private readonly Action<BackgroundWorker, ProgressChangedEventArgs> _reportProgress;

        /// <summary>
        ///     No progress to report
        /// </summary>
        /// <param name="doWork">Worker</param>
        public WorkflowBackgroundWorker(Action<BackgroundWorker> doWork) : this(doWork, null)
        {            
        }

        /// <summary>
        ///     Spawn a workflow background
        /// </summary>
        /// <param name="doWork">Work to do</param>
        /// <param name="reportProgress">Progress to report</param>
        public WorkflowBackgroundWorker(Action<BackgroundWorker> doWork, Action<BackgroundWorker, ProgressChangedEventArgs> reportProgress)
        {
            _bg = new BackgroundWorker {WorkerReportsProgress = reportProgress != null, WorkerSupportsCancellation = false};
            _bg.DoWork += BgDoWork;
            _bg.RunWorkerCompleted += BgRunWorkerCompleted;
            if (reportProgress != null)
            {
                _reportProgress = reportProgress;
                _bg.ProgressChanged += BgProgressChanged;
            }
            _doWork = doWork;
        }

        /// <summary>
        ///     Supply your own BackgroundWorker.
        /// </summary>
        /// <param name="backgroundWorker">The background worker to use</param>
        public WorkflowBackgroundWorker(BackgroundWorker backgroundWorker)
        {
            _bg = backgroundWorker;
            _bg.RunWorkerCompleted += BgRunWorkerCompleted;
        }

        /// <summary>
        ///     Progress  change
        /// </summary>
        /// <param name="sender">The background worker</param>
        /// <param name="e">The args</param>
        void BgProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _reportProgress(_bg, e);
        }

        /// <summary>
        /// Called when work is complete
        /// </summary>
        /// <param name="sender">The background worker</param>
        /// <param name="e">The <see cref="RunWorkerCompletedEventArgs"/></param>
        void BgRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _bg.DoWork -= BgDoWork;
            _bg.RunWorkerCompleted -= BgRunWorkerCompleted;
            if (_reportProgress != null)
            {
                _bg.ProgressChanged -= BgProgressChanged;
            }
            Invoked();
        }

        /// <summary>
        /// Called to do work
        /// </summary>
        /// <param name="sender">The background worker</param>
        /// <param name="e">The arguments</param>
        void BgDoWork(object sender, DoWorkEventArgs e)
        {
            _doWork(_bg);
        }

        /// <summary>
        /// Invoke kicks off the process
        /// </summary>
        public void Invoke()
        {
            _bg.RunWorkerAsync();
        }

        /// <summary>
        /// This is wired by the controller
        /// </summary>
        public Action Invoked { get; set; }        
    }
}