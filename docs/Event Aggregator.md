# Event Aggregator

Jounce uses an [EventAggregator](http://martinfowler.com/eaaDev/EventAggregator.html) for messaging. The definition is a single source that can publish and provide subscriptions for multiple objects and events. The pattern here is based on the lightweight version Rob Eisenburg introduced with [Caliburn Micro](http://caliburnmicro.codeplex.com/).

Bascially, any message T can be sent.

Jounce publishes a few distinct message types: 

**(string) Constants.BEGIN_BUSY** - this message is sent when a Xap file begins downloading. You can use this to display a "busy indicator" dialog, for example. 
**(string) Constants.END_BUSY** - this message is sent once the Xap file has been downloaded.

I typically use these messages for long running processes and have a layer in the shell that activates based on this. The "begin...end" is because processes may overlap. The view model that handles these messages should retain a counter, and increment on begin, and decrement it on end. When the counter goes from zero to 1, the overlay is displayed. When the counter goes from 1 to zero, it is hidden.

**ViewNavigationArgs** - this message indicates a view is being navigated to. It will cause Jounce to search the list of available views and view models. If a match is found, the view model will be bound to the view and have various methods called. This event should be raised to notify Jounce a view is coming into focus or going out of focus, regardless of the navigation strategy you use. The event contains a tag that identifies the view (typically the type name) and a flag indicating whether the event is to activate or deactivate the view. At the end of this event, another event is raised: 

**ViewNavigatedArgs** - this message serves a dual purpose. It is raised at the conclusion of processing a view navigation event. It is also used by the [Region Manager](Region-Manager) to begin region management. In this way, the view model and view are wired first, then the region manager can swap the views into/out of focus as needed.

**UnhandledExceptionEvent** - this message is published when an unhandled exception occurs. Jounce will intercept the error and publish the message. Your application can then inspect the message. If you want to handle it, simply set Handled = true. Any handlers should inspect Handled and ignore the message if it is set.

To reference the Event Aggregator, simply import it: 

{code:c#}
[Import](Import)
public IEventAggregator EventAggregator { get; set; }
{code:c#}

To publish a message, call the publish method: 

{code:c#}
EventAggregator.Publish("A message of type string."); 
{code:c#}

To publish a null message, specify the type: 

{code:c#}
EventAggregator.Publish<MyMessage>(null);
{code:c#}

Subscriptions require two steps. First, whatever entity is handling the message must implement the event sink for the message (a listener). It does this by implementing IEventSink<T>. You may implement multiple messages.

{code:c#}
public class MyClass : IEventSink<string>, IEventSink<UnhandledExceptionEvent>
{
   public void HandleEvent(string eventText)
   {
      MessageBox.Show(eventText);
   }

   public void HandleEvent(UnhandledExceptionEvent unhandledEvent)
   {
      unhandledEvent.Handled = true;
      MessageBox.Show(unhandledEvent.UncaughtException.Message); 
   }
}
{code:c#}

In order to begin listening for events, you must subscribe. If you only have one event sink, you can subscribe like this: 

{code:c#}
EventAggregator.Subscribe(this);
{code:c#}

If you have multiple event sinks, you must specify the type you are subscribing for: 

{code:c#}
EventAggregator.Subscribe<string>(this);
EventAggregator.Subscribe<UnhandledExceptionEvent>(this);
{code:c#}

You may unsubscribe the same way. 