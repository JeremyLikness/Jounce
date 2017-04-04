# Basic View Models

The simplest view model derives from BaseNotify for [Property Change Notification](Property-Change-Notification) and implements IViewModel. 

The basic view model will automatically import the [Event Aggregator](Event-Aggregator), the [Logger](Logger), and the view model router (see [Finding a View Model with the Router](Finding-a-View-Model-with-the-Router)).

Some other services you will receive by deriving from BaseViewModel include: 

**Visual State Management** - when Jounce binds a view to a view model, it will set a delegate on the view model for state transitions. To transition to state "CustomState" on your view using transitions, you will simply execute: 

{code:c#}
GoToVisualState("CustomState", true); 
{code:c#}

If you bind multiple views to the same view model (Jounce has absolutely no problem with this) you can call:

{code:c#}
GoToVisualStateForView(viewName, stateName, useTransitions);
{code:c#}

Any views bound to the view model will be contained in the _RegisteredViews_ list.

**Design-time awareness** - you can use Jounce to determine whether your view model is in the designer or not. For example, to wire sample data, you can use: 

{code:c#}
public class MyViewModel : BaseViewModel 
{
   public MyViewModel()
   {
      if (InDesigner) 
      { 
          // design-time data wired here
      } 
   }
}
{code:c#}

You can then reference the view model as a design-time instance like this: 

{code:xml}
<Grid d:DataContext="{d:DesignInstance vm:MyViewModel,IsDesignTimeCreateable=True}">
{code:xml}

Because Jounce manages the binding and activation of views and view models, the view model is aware of view events. 

**_Initialize()** is called by the framework the first time a view model is created. Override this to spin up first-time data, call services, etc - anything necessary to set up the view model.

**_Activate(string viewName)** is called whenever a view is navigated to (see the [Event Aggregator](Event-Aggregator) and [Navigation](Navigation)). The view tag is passed in the case of multiple views being bound to the same view model.

**_Deactivate(string viewName)** is called whenever the view is navigated away from. 