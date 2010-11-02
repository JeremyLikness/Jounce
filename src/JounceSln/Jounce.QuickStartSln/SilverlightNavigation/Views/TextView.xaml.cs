using System;
using System.ComponentModel.Composition;
using System.Windows.Navigation;
using Jounce.Core.Event;
using Jounce.Core.View;
using Jounce.Regions.Core;

namespace SilverlightNavigation.Views
{
    [ExportAsView("TextView")]
    [ExportViewToRegion("TextView", "MainContainer")]
    public partial class TextView : IEventSink<NavigationContext>, IPartImportsSatisfiedNotification
    {
        [Import]
        public IEventAggregator EventAggregator { get; set; }

        public TextView()
        {
            InitializeComponent();                        
        }

        public void HandleEvent(NavigationContext publishedEvent)
        {
            if (publishedEvent.QueryString.ContainsKey("text"))
            {
                TextArea.Text = publishedEvent.QueryString["text"];
            }
        }

        /// <summary>
        /// Called when a part's imports have been satisfied and it is safe to use.
        /// </summary>
        public void OnImportsSatisfied()
        {
            EventAggregator.SubscribeOnDispatcher(this);
        }
    }
}
