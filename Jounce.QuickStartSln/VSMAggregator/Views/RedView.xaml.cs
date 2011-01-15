using System.Linq;
using System.Windows;
using Jounce.Core.View;
using Jounce.Framework;
using Jounce.Regions.Core;

namespace VSMAggregator.Views
{
    [ExportAsView(Globals.VIEW_RED)]
    [ExportViewToRegion(Globals.VIEW_RED, Globals.REGION_MAIN)]
    public partial class RedView
    {
        public RedView()
        {
            InitializeComponent();
            Loaded += RedView_Loaded;            
        }

        void RedView_Loaded(object sender, RoutedEventArgs e)
        {
            var groups = VisualStateManager.GetVisualStateGroups(LayoutRoot);
            foreach(var group in groups.Cast<VisualStateGroup>().Where(g=>g.Name.Equals("NavigationStates")))
            {
                group.CurrentStateChanged += GroupCurrentStateChanged;                
            }
        }

        static void GroupCurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            JounceHelper.ExecuteOnUI(() => MessageBox.Show(string.Format("Transition {0}=>{1}", e.OldState.Name, e.NewState.Name)));
        }

        
    }
}
