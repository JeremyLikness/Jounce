# Quickstarts

I've provided a few quickstarts to help you get familiar with Jounce. The quickstart solution is below the root of the main Jounce solution.

**DynamicXapCalculator** 

This project, in conjunction with the misspelled _DyanmicXapCalculatorAdvanced_, demonstrates explicit Xap loading. The project provides a simple calculator that supports addition and subtraction, and an option to dynamically load the multiplication and division options.

**EntityViewModel**

This project shows how to use the _BaseEntityViewModel_ for validation and Create/Read/Update/Delete operations with cancel/confirm.

**EventAggregator**

This project demonstrates how to pass messages between modules and view models. It includes a trap for the unexpected error message that is published by Jounce when an unhandled error is encountered.

**FluentBinding**

OK, so fluent sounds "cool" but this is really just runtime binding. This project shows an example of binding the view to the viewmodel at runtime using a bootstrapper instead of an export.

**NonSharedViews**

This example shows how to generate non-shared (i.e. multiple copies) views and view models.

**RegionManagement**

The region management quickstart shows off many features. These include how region adapters automatically handle navigation and includes a custom adapter for tabbed controls.

See how the _Visual State Manager_ can be invoked from your view model in one example that animates the view when it is opened and closed.

This project implements a custom _ILogger_ that displays Jounce messages in realtime via a list box. It also supports a dynamic region that can be loaded with a click - in the code you'll see it looks the same as any other navigation, but has the added hint for Jounce to locate the Xap file it needs.

**SilverlightNavigationFramework**

This example shows how to use Jounce with the existing navigation framework built into Silverlight.

**SimpleNavigation**

This project illustrates a simple navigation example without using region management.

**NavigationWithBackButton** 

This project demonstrates navigation with a remembered "back stack" so hitting the back button reverses the flow as in a web application.

**SimpleNavigationWithRegion**

This project refactors the previous to use regions.

**SilverlightNavigation**

This project illustrates using Jounce in conjunction with the Silverlight Navigation framework.

**VSMAggregator**

This project demonstrates visual states, both using the VisualStateAggregator and programmatically transitioning states.

**Workflow** 

Workflows are very powerful sequential asynchronous operations. This project shows a workflow that performs computations in the background without blocking the UI using the background worker workflow object, along with other workflow objects that are part of Jounce.