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
        
        public WorkflowBackgroundWorker(Action<BackgroundWorker> doWork, bool reportsProgress)
        {
            _bg = new BackgroundWorker {WorkerReportsProgress = reportsProgress, WorkerSupportsCancellation = false};
            _bg.DoWork += BgDoWork;
            _bg.RunWorkerCompleted += BgRunWorkerCompleted;
            _doWork = doWork;
        }

        void BgRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _bg.DoWork -= BgDoWork;
            _bg.RunWorkerCompleted -= BgRunWorkerCompleted;
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