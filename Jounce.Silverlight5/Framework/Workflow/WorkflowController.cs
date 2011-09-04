using System;
using System.Collections.Generic;
using Jounce.Core.Workflow;

namespace Jounce.Framework.Workflow
{
    /// <summary>
    ///     Controller for sequential asynchronous workflows
    /// </summary>
    public class WorkflowController
    {
        /// <summary>
        ///     Callback for exception
        /// </summary>
        private readonly Action<Exception> _exceptionCallback;

        /// <summary>
        ///     Enumerator
        /// </summary>
        private readonly IEnumerator<IWorkflow> _enumerator;

        /// <summary>
        ///     Constructor starts the process
        /// </summary>
        /// <param name="workflow">The workflow to start</param>
        /// <param name="exceptionCallback">Callback for when exceptions occur</param>
        public WorkflowController(IEnumerable<IWorkflow> workflow, Action<Exception> exceptionCallback)
        {
            _enumerator = workflow.GetEnumerator();
            _exceptionCallback = exceptionCallback;
        }

        /// <summary>
        ///     Invoked - moves to next item
        /// </summary>
        private void Invoked()
        {
            if (!_enumerator.MoveNext())
                return;

            var next = _enumerator.Current;
            next.Invoked = Invoked;

            // call it
            try
            {
                next.Invoke();
            }
            catch(Exception ex)
            {
                _enumerator.Dispose();
                _exceptionCallback(ex);                
            }
        }

        /// <summary>
        ///     Begins a workflow
        /// </summary>
        /// <param name="workflow">The workflow item</param>
        /// <param name="exceptionCallback">Callback for exceptions</param>
        public static void Begin(object workflow, Action<Exception> exceptionCallback)
        {
            // start with a unit
            if (workflow is IWorkflow)
            {
                workflow = new[] {workflow as IWorkflow};
            }

            // start with a list
            if (workflow is IEnumerable<IWorkflow>)
            {
                new WorkflowController(workflow as IEnumerable<IWorkflow>, exceptionCallback).Invoked();
            }
        }

        /// <summary>
        ///     Begin workflow (no exception handling)
        /// </summary>
        /// <param name="workflow">The workflow seed</param>
        public static void Begin(object workflow)
        {
            Begin(workflow, ex=>{});
        }
    }
}