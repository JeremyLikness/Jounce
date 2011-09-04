using System;
using System.ComponentModel;
using Jounce.Core.Workflow;

namespace Jounce.Framework.Workflow
{
    /// <summary>
    ///     Workflow for running a background process
    /// </summary>
    public class WorkflowBackgroundWorker : IWorkflow 
    {
        private readonly BackgroundWorker _bg;
        private readonly Action<BackgroundWorker> _doWork;
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
        /// <param name="backgroundWorker"></param>
        public WorkflowBackgroundWorker(BackgroundWorker backgroundWorker)
        {
            _bg = backgroundWorker;
            _bg.RunWorkerCompleted += BgRunWorkerCompleted;
        }

        /// <summary>
        ///     Progress  change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BgProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _reportProgress(_bg, e);
        }

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

        void BgDoWork(object sender, DoWorkEventArgs e)
        {
            _doWork(_bg);
        }

        public void Invoke()
        {
            _bg.RunWorkerAsync();
        }

        public Action Invoked { get; set; }        
    }
}