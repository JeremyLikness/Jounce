using System;

namespace Jounce.Core.Workflow
{
    /// <summary>
    ///     Interface for a sequential asynchronous workflow item
    /// </summary>
    public interface IWorkflow
    {
        void Invoke();
        Action Invoked { get; set; }
    }
}