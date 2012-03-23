using System;

namespace ToDoList.SterlingDatabase
{
    public class Concurrency
    {        
        public Guid Status { get; set; } 
   
        public static Concurrency Refresh()
        {
            return new Concurrency {Status = Guid.NewGuid()};
        }
    }
}