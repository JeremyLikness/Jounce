using System.Collections.Generic;
using ToDoList.Contracts;

namespace ToDoList.Sorts
{
    public class SortBase
    {
        public delegate IEnumerable<IToDoItem> 
            SortDelegate(IEnumerable<IToDoItem> list);

        private SortBase(string name, SortDelegate sort)
        {
            Name = name;
            Sort = sort;
        }

        public static SortBase Create(string name, SortDelegate sort)
        {
            return new SortBase(name, sort);
        }

        public string Name { get; private set; }
        public SortDelegate Sort { get; private set; }

        public override bool Equals(object obj)
        {
            return obj is SortBase && ((SortBase)obj).Name.Equals(Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}