using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;
using Jounce.Core.Event;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework;

namespace ToDoList.ViewModels
{
    [Export]
    public class WindowController : IEventSink<ViewNavigationArgs>,
                                    IPartImportsSatisfiedNotification
    {
        [Import]
        public IEventAggregator EventAggregator { get; set; }

        [Import]
        public IViewModelRouter Router { get; set; }

        public void HandleEvent(ViewNavigationArgs publishedEvent)
        {
            var parms = publishedEvent.ViewParameters;
            if (!parms.ContainsKey(Global.Constants.PARM_WINDOW) ||
                !parms.ParameterValue<bool>(Global.Constants.PARM_WINDOW))
            {
                return;
            }

            var viewModelTag = Router.GetViewModelTagForView(
                publishedEvent.ViewType);
            var viewModel = Router.GetNonSharedViewModel(viewModelTag);
            var view = Router.GetNonSharedView(
                publishedEvent.ViewType,
                viewModel,
                publishedEvent.ViewParameters 
                as Dictionary<string, object>);
            new Window
                    {
                        Title = parms.ParameterValue<string>(
                            Global.Constants.PARM_TITLE),
                        Width = parms.ParameterValue<int>(
                            Global.Constants.PARM_WIDTH),
                        Height = parms.ParameterValue<int>(
                            Global.Constants.PARM_HEIGHT),
                        Content = view,
                        Visibility = Visibility.Visible
                    };
        }

        public void OnImportsSatisfied()
        {
            EventAggregator.SubscribeOnDispatcher(this);
        }
    }
}