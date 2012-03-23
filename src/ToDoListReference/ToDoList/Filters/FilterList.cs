using System.ComponentModel.Composition;

namespace ToDoList.Filters
{
    public class FilterList
    {
        [Export]
        public FilterBase All
        {
            get { return FilterBase.Create("All", t => true); }
        }

        [Export]
        public FilterBase Future
        {
            get { return FilterBase.Create("Future", t => t.IsInFuture); }
        }

        [Export]
        public FilterBase PastDue
        {
            get { return FilterBase.Create("Past Due", t => t.IsPastDue); }
        }

        [Export]
        public FilterBase DueTomorrow
        {
            get { return FilterBase.Create("Due Tomorrow", t => t.IsDueTomorrow); }
        }

        [Export]
        public FilterBase DueNextWeek
        {
            get { return FilterBase.Create("Due Next Week", t => t.IsDueNextWeek); }
        }

        [Export]
        public FilterBase Complete
        {
            get { return FilterBase.Create("Complete", t => t.IsComplete); }
        }
    }
}