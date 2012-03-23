using Jounce.Core.View;
using Jounce.Regions.Core;

namespace ToDoList.Views
{
    [ExportAsView(typeof(Update))]
    [ExportViewToRegion(typeof(Update), Global.Constants.EDIT_REGION)]
    public partial class Update
    {
        public Update()
        {
            InitializeComponent();
        }
    }
}
