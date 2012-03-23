using System.Windows.Controls;
using Jounce.Core.View;
using Jounce.Regions.Core;

namespace ToDoList.Views
{
    [ExportAsView(typeof(OutOfBrowser))]
    [ExportViewToRegion(typeof(OutOfBrowser),
        Global.Constants.EDIT_REGION)]
    public partial class OutOfBrowser
    {
        public OutOfBrowser()
        {
            InitializeComponent();
        }
    }
}
