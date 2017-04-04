# Dialogs

Jounce specifically does not provide a dialog implementation. Why? Everyone else has one, right? 

I've yet to be one two projects that consistently handle dialogs. 

One project uses the MessageBox (even though it has major blocking issues). A lot of people like the ChildWindow. In many projects, however, the ChildWindow is not customizeable enough, so instead a special region is created as an "overlay" region and the dialogs are placed as regular user controls into that region. 

If there is a compelling and flexible way to do this in Jounce, let me know and I'll be happy to implement it. For the most part, however, I find myself either going with a region that has an overlay and an items control to route dialogs (just using the region manager, and then a class that listens for views routed to the dialog area and turns the region on or off). I've been asked if the event aggregator is the way to go beacuse I gave an example of using that with Prism, but I prefer something far more simple, like this: 

[Simple Dialog Service](http://csharperimage.jeremylikness.com/2010/01/simple-dialog-service-in-silverlight.html)


