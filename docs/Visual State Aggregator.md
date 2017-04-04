# Visual State Aggregator

The purpose of the _VisualStateAggregator_ is to allow coordination of transitions between multiple controls without having to involve code-behind or view models. 

If you aren't familiar with the Visual State Manager (VSM) it's a powerful way to abstract states and transitions for UI elements away from the actual details of how those states/transitions are done. Jounce provides a mechanism for view models to transition to states. The Visual State Aggregator, however, allows you to trigger a state change using any event (via the _VisualStateAggregatorTrigger_) and then multiple controls can respond.

A simple example would be two pages you want to transition between. You can set up a region via [Region Management](Region-Management) as a Grid and target the region with two views. Give each view a view state, then use a trigger attached to your navigation (i.e. maybe it's clicking on a particular button) and bind it to a common event. When the button is clicked on View A, it can fire an event for View A to fade and View B to zoom in. Conversely, when a button is clicked on View B, it can fire an event to View B to zoom out and View A to fade in. All of this can be done entirely in the UI layer. Even the navigation doesn't require anything more than a [Navigation Trigger](Navigation). This is true separation of concerns!

For more comprehensive documentation with a full example, read [Introducing the Visual State Aggregator](http://csharperimage.jeremylikness.com/2010/03/introducing-visual-state-aggregator.html).