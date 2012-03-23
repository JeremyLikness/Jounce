using System.ComponentModel.Composition;
using ToDoList.Contracts;
using ToDoList.Silverlight.SterlingDatabase;

namespace ToDoList.Exports
{
    public class RepositoryExport
    {
        private ICacheContract _cache;

        [Export]
        [Export(typeof(IRepository))]
        public ICacheContract Cache
        {
            get { return _cache ?? 
                (_cache = new ToDoCacheRepository()); }
        }        
    }   
}