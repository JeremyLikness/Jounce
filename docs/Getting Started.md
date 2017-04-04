# Getting Started

Getting started with Jounce is simple and easy.

One you download the [Source Code](http://jounce.codeplex.com/SourceControl/list/changesets), you can compile it to a single DLL:

**Jounce.dll** contains the full implementation. 

To register the Jounce framework with your application, simply add a reference to the framework namespace and include the ApplicationService reference in your App.xaml. 

The App.xaml will look like this:

{code:xml}
<Application xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml" 
             xmlns:Services="clr-namespace:Jounce.Framework.Services;assembly=Jounce" 
             x:Class="Jounce.App"
             >
    <Application.ApplicationLifetimeObjects>
        <Services:ApplicationService/>
    </Application.ApplicationLifetimeObjects>
    <Application.Resources>        
    </Application.Resources>
</Application>
{code:xml}

You can delete everything in the App.xaml.cs code behind except the call to InitializeComponent. Jounce will handle exceptions by raising a special event (see the [Event Aggregator](Event-Aggregator) to learn more).