using System;
using Jounce.Core.Workflow;

namespace Jounce.Framework.Workflow
{
    /// <summary>
    ///     Generic workflow action
    /// </summary>
    public class WorkflowAction : IWorkflow
    {
        private readonly bool _immediate;

        public Action Execute { get; set; }

        public WorkflowAction()
        {
        }

        public WorkflowAction(bool immediate)
        {
            _immediate = immediate;
        }

        public WorkflowAction(Action action)
        {
            _immediate = false;
            Execute = action;
        }

        public WorkflowAction(Action action, bool immediate)
        {
            _immediate = immediate;
            Execute = action;
        }

        public void Invoke()
        {
            Execute();
            if (_immediate)
            {
                Invoked();
            }
        }

        public Action Invoked { get; set; }
    }
}