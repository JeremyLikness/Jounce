using System.ComponentModel.Composition;
using System.Windows;
using Jounce.Core.Event;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using RegionManagement.ViewModels;

namespace RegionManagement.Views
{
    /// <summary>
    ///     Just a circle
    /// </summary>
    [ExportAsView(CIRCLE)]
    public partial class Circle
    {
        public const string CIRCLE = "Circle";

        [Import]
        public IEventAggregator EventAggregator { get; set; }

        public Circle()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EventAggregator.Publish(new ViewNavigationArgs(CIRCLE) { Deactivate = true});
        }

        [Export]
        public ViewModelRoute Binding
        {
            get
            {
                return ViewModelRoute.Create(CircleViewModel.CIRCLE_VM, CIRCLE);
            }
        }
    }
}
