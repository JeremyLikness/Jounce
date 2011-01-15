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
                foreach (var state in
                    group.States.Cast<VisualState>().Where(state => state.Name.Equals("HideState")))
                {
                    state.Storyboard.Completed += Storyboard_Completed;
                }
            }
        }

        static void Storyboard_Completed(object sender, System.EventArgs e)
        {
            JounceHelper.ExecuteOnUI(()=>MessageBox.Show("Red transition completed."));
        }
    }
}
