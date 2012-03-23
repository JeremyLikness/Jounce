using System;
using System.ComponentModel.Composition;
using ToDoList.Contracts;

namespace ToDoList.Exports
{    
    public class FactoryExports
    {
        [Import]
        public ExportFactory<IToDoItem> ToDoItemFactory { get; set; }       
 
        [Export(typeof(Func<IToDoItem>))]
        public Func<IToDoItem> ToDoItemFactoryExport
        {
            get { return () => ToDoItemFactory.CreateExport().Value; }
        }
    }
}