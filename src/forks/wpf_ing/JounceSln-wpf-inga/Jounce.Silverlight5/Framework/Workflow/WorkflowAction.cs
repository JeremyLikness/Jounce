using System;
using Jounce.Core.Workflow;

namespace Jounce.Framework.Workflow
{
    /// <summary>
    ///     Generic workflow action
    /// </summary>
    /// <remarks>
    /// Generic form of encapsulating actions
    /// </remarks>
    public class WorkflowAction : IWorkflow
    {
        /// <summary>
        /// True if action should be called immediately (won't be invoked by user)
        /// </summary>
        private readonly bool _immediate;

        /// <summary>
        /// Execute command
        /// </summary>
        public Action Execute { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public WorkflowAction()
        {
        }

        /// <summary>
        /// Constructor with immediate
        /// </summary>
        /// <param name="immediate">True to call immediately</param>
        public WorkflowAction(bool immediate)
        {
            _immediate = immediate;
        }

        /// <summary>
        /// Constructor with the action
        /// </summary>
        /// <param name="action">Action to kick off</param>
        public WorkflowAction(Action action)
        {
            _immediate = false;
            Execute = action;
        }

        /// <summary>
        /// Constructor with action and immediate flag
        /// </summary>
        /// <param name="action">Action to call</param>
        /// <param name="immediate">True to call immediately</param>
        public WorkflowAction(Action action, bool immediate)
        {
            _immediate = immediate;
            Execute = action;
        }

        /// <summary>
        /// Invoke command - if the action is not asynchronous, will immediately
        /// call the invoked command
        /// </summary>
        public void Invoke()
        {
            Execute();
            if (_immediate)
            {
                Invoked();
            }
        }

        /// <summary>
        /// Invoked command wired by the controller
        /// </summary>
        public Action Invoked { get; set; }
    }
}