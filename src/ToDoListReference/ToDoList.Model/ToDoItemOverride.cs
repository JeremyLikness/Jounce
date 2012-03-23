using System;
using System.ComponentModel.Composition;

namespace ToDoList.Model
{
    [Export]
    public class ToDoItemOverride : ToDoItem 
    {        
        public new bool IsComplete
        {
            get { return base.IsComplete; }
            set { base.IsComplete = value; }
        }
        public new DateTime CompletedDate
        {
            get { return base.CompletedDate; }
            set { base.CompletedDate = value; }
        }
     }
}