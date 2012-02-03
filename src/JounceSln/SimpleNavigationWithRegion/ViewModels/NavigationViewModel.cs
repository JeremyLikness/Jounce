using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Controls;
using Jounce.Core.View;
using Jounce.Core.ViewModel;

namespace SimpleNavigationWithRegion.ViewModels
{
    /// <summary>
    ///     Handles navigation - extracts the meta data for the views and raises navigation events
    ///     Uses "current view" to synchronize the buttons (button for current view will be disabled)
    /// </summary>
    [ExportAsViewModel("Navigation")]
    public class NavigationViewModel : BaseViewModel
    {
        public NavigationViewModel()
        {
            // this is just an easy way to pull out the pieces we want/need, normally we'd
            // probably type this more strongly
            _buttonInfo = new ObservableCollection<Tuple<string, string, string>>();

            // give the designer something
            if (DesignerProperties.IsInDesignTool)
            {
                ButtonInfo.Add(Tuple.Create("Test", "Test", "This is a test instance."));
            }
        }

        /// <summary>
        ///     Grab the full list of views
        /// </summary>
        [ImportMany(AllowRecomposition = true)]
        public Lazy<UserControl, IExportAsViewMetadata>[] Views { get; set; }

        /// <summary>
        ///     We could wire everything on imports satisfied, this is just an example of doing
        ///     it "lazy" and waiting until the list is requested
        /// </summary>
        private readonly ObservableCollection<Tuple<string, string, string>> _buttonInfo;

        public ObservableCollection<Tuple<string, string, string>> ButtonInfo
        {
            get
            {
                if (_buttonInfo.Count == 0)
                {
                    if (Views != null && Views.Any())
                    {
                        WireButtonInfo();
                    }
                }
                return _buttonInfo;
            }
        }

        /// <summary>
        /// Called when a part's imports have been satisfied and it is safe to use.
        /// </summary>
        public void WireButtonInfo()
        {
            // filter only those views that are in the navigation category
            foreach (var v in (from viewInfo in Views
                               where viewInfo.Metadata.Category.Equals("Navigation")
                               select Tuple.Create(
                                   viewInfo.Metadata.ExportedViewType,
                                   viewInfo.Metadata.MenuName,
                                   viewInfo.Metadata.ToolTip)).Distinct())
            {
                _buttonInfo.Add(v);
            }
        }
    }
}