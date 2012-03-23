using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Threading;
using ToDoList.Contracts;
using ToDoList.Model;
using ToDoList.SterlingDatabase;

namespace ToDoList.Web
{
    public static class MEFBootstrapper
    {
        private static readonly CompositionContainer Container;
        
        static MEFBootstrapper()
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(typeof(MEFBootstrapper).AsCatalogForAssembly());
            catalog.Catalogs.Add(typeof(IToDoItem).AsCatalogForAssembly());
            catalog.Catalogs.Add(typeof(ToDoItem).AsCatalogForAssembly());
            catalog.Catalogs.Add(typeof(ToDoDatabase).AsCatalogForAssembly());
            
            Container = new CompositionContainer(catalog);        
        }

        public static void SatisfyImports(object target)
        {
            Container.ComposeParts(target);
        }        

        public static AssemblyCatalog AsCatalogForAssembly(this Type type)
        {
            return new AssemblyCatalog(type.Assembly);       
        }
    }    
}