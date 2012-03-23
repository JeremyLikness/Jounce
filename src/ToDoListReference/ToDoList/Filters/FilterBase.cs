using System;
using ToDoList.Contracts;

namespace ToDoList.Filters
{
    public class FilterBase
    {
        private FilterBase(string name, Predicate<IToDoItem> filter)
        {
            Name = name;
            Filter = filter;
        }

        public static FilterBase Create(string name, Predicate<IToDoItem> filter)
        {
            return new FilterBase(name, filter);
        }
        
        public string Name { get; private set; }
        public Predicate<IToDoItem> Filter { get; private set; }

        public override bool Equals(object obj)
        {
            return obj is FilterBase && ((FilterBase) obj).Name.Equals(Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}