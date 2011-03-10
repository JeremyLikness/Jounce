using Jounce.Core.View;
using Jounce.Regions.Core;

namespace NavigationWithBackButton.Views
{
    [ExportAsView("Fib", Category="Content", MenuName="Fibonacci Sequence")]
    [ExportViewToRegion("Fib", "ContentRegion")]
    public partial class Fibonacci
    {
        public Fibonacci()
        {
            InitializeComponent();
        }
    }
}
