using System.Collections.Generic;
using Jounce.Core.Command;
using ToDoList.Filters;

namespace ToDoList.Contracts
{
    /// <summary>
    /// Contract for the view model that exposes filters
    /// </summary>
    public interface IFilterViewModel
    {
        /// <summary>
        /// Command to change the <seealso cref="FilterBase"/>
        /// </summary>
        IActionCommand<FilterBase> FilterCommand { get; }

        /// <summary>
        /// List of <seealso cref="FilterBase"/> filters
        /// </summary>
        IEnumerable<FilterBase> Filters { get; } 
    }
}