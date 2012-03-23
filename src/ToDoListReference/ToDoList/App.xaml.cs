using System.Globalization;
using System.Threading;

namespace ToDoList
{
    public partial class App
    {
        public App()
        {            
            InitializeComponent();      
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("es");
        }        
    }
}
