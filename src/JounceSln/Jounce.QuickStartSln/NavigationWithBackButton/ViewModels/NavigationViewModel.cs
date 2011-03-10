using System;
using System.Collections.ObjectModel;
using System.Linq;
using Jounce.Core.Event;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework;
using Jounce.Framework.ViewModels;

namespace NavigationWithBackButton.ViewModels
{
    [ExportAsViewModel("Navigation")]
    public class NavigationViewModel : BaseViewModel, IEventSink<ViewNavigationArgs>
    {
        public NavigationViewModel()
        {
            Menu = new ObservableCollection<Tuple<string, string>>();

            if (!InDesigner) return;

            Menu.Add(Tuple.Create("Clock", "A Gigantic Clock"));
            Menu.Add(Tuple.Create("Fibonacci", "The Fibonacci Ratio"));
            Menu.Add(Tuple.Create("Shapes", "Random Shapes"));
            SelectedView = Menu[0];
        }

        private Tuple<string,string> _selectedView; 

        public Tuple<string, string> SelectedView
        {
            get { return _selectedView; }
            set 
            { 
                _selectedView = value;
                RaisePropertyChanged(()=>SelectedView);

                if (!InDesigner)
                {
                    EventAggregator.Publish(value.Item1.AsViewNavigationArgs());
                }
            }
        }    

        public ObservableCollection<Tuple<string, string>> Menu { get; private set; }

        public override void _Initialize()
        {
            var query = (from vi in ((ViewModelRouter) Router).Views
                         where vi.Metadata.Category.Equals("Content")
                         orderby vi.Metadata.MenuName
                         select Tuple.Create(vi.Metadata.ExportedViewType, vi.Metadata.MenuName)).Distinct();            

            foreach(var item in query)
            {
                Menu.Add(item);
            }

            SelectedView = Menu[0];

            EventAggregator.Subscribe(this);

            base._Initialize();
        }

        public void HandleEvent(ViewNavigationArgs publishedEvent)
        {
            if (publishedEvent.Deactivate)
            {
                return; 
            }

            var item = (from menuItem in Menu
                        where menuItem.Item1.Equals(publishedEvent.ViewType)
                        select menuItem).FirstOrDefault();

            if (item != null)
            {
                _selectedView = item;
                RaisePropertyChanged(()=>SelectedView);
            }
        }
    }
}