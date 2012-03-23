using System;
using System.Collections.Generic;
using ToDoList.Contracts;
using ToDoList.Model;

namespace ToDoList.SampleData
{
#if DEBUG
    public class DesignToDoListViewModel : IToDoListViewModel 
    {
        private readonly List<IToDoItem> _items = new List<IToDoItem>();

        public DesignToDoListViewModel()
        {            
            for(var x = 0; x < 20; x++)
            {
                var todo = new ToDoItem
                               {
                                   Description = string.Format("Some crazy little task {0}", x+1),
                                   Title = string.Format("Task Number {0}", x+1),
                                   DueDate = DateTime.Now.Date.AddDays(x - 10)
                               };
                _items.Add(todo);
            }
        }
        
        public IEnumerable<IToDoItem> Tasks
        {
            get { return _items; }
        }
    }
#endif
}