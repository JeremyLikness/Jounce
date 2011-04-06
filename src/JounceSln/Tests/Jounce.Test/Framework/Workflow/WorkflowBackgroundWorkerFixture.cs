using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using Jounce.Core.Workflow;
using Jounce.Framework.Workflow;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jounce.Test.Framework.Workflow
{
    [TestClass]
    public class WorkflowBackgroundWorkerFixture
    {
        [TestMethod]
        public void WorkflowBackgroundWorker_Invokes_doWork_Action()
        {
            WorkflowController.Begin(WorkflowBackgroundWorkerTest1());
        }

        private IEnumerable<IWorkflow> WorkflowBackgroundWorkerTest1()
        {
            int _index = 0;

            var bgWorkflow = new WorkflowBackgroundWorker(bg => _index++);
            yield return bgWorkflow;

            Assert.AreEqual(1, _index);

            yield return new WorkflowAction(() => _index++);
        }

        [TestMethod]
        public void WorkflowBackgroundWorker_With_Existing_Background_Worker_Invokes_doWork_Action()
        {
            WorkflowController.Begin(WorkflowBackgroundWorkerTest2());
        }

        private IEnumerable<IWorkflow> WorkflowBackgroundWorkerTest2()
        {
            int _index = 0;

            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += (s, e) => _index++;
            backgroundWorker.RunWorkerCompleted += (s, e) => _index++;

            var bgWorkflow = new WorkflowBackgroundWorker(backgroundWorker);
            yield return bgWorkflow;

            Assert.AreEqual(2, _index);

            yield return new WorkflowAction(() => _index++);
        }
    }
}