# Navigation

Navigation in Jounce is very simple. Anytime you wish to activate a view, simply raise an event:

{code:c#}
EventAggregator.Publish(new ViewNavigationArgs("MyView")); 
{code:c#}

This will find the view, bind the view to the view model, and call Initialize and/or Activate on the view model. 

Because views have their visual state bound to the view model, the view model can transition to states wheneve the view is both activated and deactivated.

Once all of the basic view location and view model calls are complete, the **ViewNavigatedArgs** is raised. You can listen for this event and react as needed.

Use the **NavigationTrigger** behavior in your XAML to fire navigation events. You can pass a view tag or bind the target, and the behavior will automatically fire the event aggregator event for you.

The Jounce [region management](region-management) module uses this event to handle placing views in regions.