using System;

namespace Jounce.Core.Workflow
{
    /// <summary>
    ///     Interface for a sequential asynchronous workflow item
    /// </summary>
    /// <remarks>
    /// A long-running operation is launched using the <see cref="Invoke"/> method. The controller
    /// will wait for the operation to end and then continue the workflow by calling 
    /// <see cref="Invoked"/>
    /// </remarks>
    public interface IWorkflow
    {
        void Invoke();
        Action Invoked { get; set; }
    }
}