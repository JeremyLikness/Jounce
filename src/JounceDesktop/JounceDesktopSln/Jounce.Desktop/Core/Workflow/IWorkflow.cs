using System;

namespace Jounce.Desktop.Core.Workflow
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
        /// <summary>
        /// Invoke starts the sequence
        /// </summary>
        void Invoke();

        /// <summary>
        /// Invoked is called when the sequence ends (usually chained to the next instance)
        /// </summary>
        Action Invoked { get; set; }
    }
}