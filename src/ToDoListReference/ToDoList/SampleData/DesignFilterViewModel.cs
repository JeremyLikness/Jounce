using System.Collections.Generic;
using Jounce.Core.Command;
using Jounce.Framework.Command;
using ToDoList.Contracts;
using ToDoList.Filters;

namespace ToDoList.SampleData
{
#if DEBUG
    public class DesignFilterViewModel : IFilterViewModel
    {
        public IActionCommand<FilterBase> FilterCommand
        {
            get { return new ActionCommand<FilterBase>(); }
        }

        public IEnumerable<FilterBase> Filters
        {
            get { return new[] {FilterBase.Create("All", t => true), FilterBase.Create("Complete", t => true)}; }
        }
    }
#endif
}