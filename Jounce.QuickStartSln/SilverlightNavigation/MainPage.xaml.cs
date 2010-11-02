using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Jounce.Core.View;

namespace SilverlightNavigation
{
    [ExportAsView("MainPage", IsShell=true)]
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // After the Frame navigates, ensure the HyperlinkButton representing the current page is selected
        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            foreach (var hb in
                LinksStackPanel.Children.Select(child => child as HyperlinkButton).Where(hb => hb != null && hb.NavigateUri != null))
            {
                VisualStateManager.GoToState(hb,
                                             hb.NavigateUri.ToString().Equals(e.Uri.ToString())
                                                 ? "ActiveLink"
                                                 : "InactiveLink", true);
            }
        }

        // If an error occurs during navigation, show an error window
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;
            ChildWindow errorWin = new ErrorWindow(e.Uri);
            errorWin.Show();
        }
    }
}