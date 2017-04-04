# Design-Time Data

There are many ways to provide design-time data. While the "locator" method is quite popular, Jounce suggests (but doesn't require) a more straightforward manner.

For a non-interfaced approach that uses compiler directives, read this post: 

[Clean Design-Time Friendly View Models: A Walkthrough](http://csharperimage.jeremylikness.com/2011/03/clean-design-time-friendly-viewmodels.html)

First, define an interface for your view model:

{code:c#}
public interface IMainViewModel
{
    string Welcome { get; }
}
{code:c#}

Next, create a folder called SampleData and add a DesignViewModel class that inherits the interface. Don't worry about property changed events and don't implement any methods, simply provide the data points where they are needed.

{code:c#}
public class DesignTimeViewModel : IMainViewModel  
{
    public string Welcome
    {
        get { return "Welcome to the designer for Jounce."; }
    }
}
{code:c#}

Finally, create your actual view model inheriting from one of the base view models in Jounce.

{code:c#}
[ExportAsViewModel("MainViewModel")](ExportAsViewModel(_MainViewModel_))
public class MainViewModel : BaseViewModel, IMainViewModel 
{
    public string Welcome
    {
        get { return "Welcome to Jounce."; }
    }
}
{code:c#}

The runtime view model will be bound by the Jounce framework. For the design-time view model, just add a small piece of xaml to the view.

First, declare a sample data namespace. Make sure the design-time namespaces are declared as well.

{code:xml}
<UserControl xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
      xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
      xmlns:sampleData="clr-namespace:FluentBinding.SampleData"
      mc:Ignorable="d">
{code:xml}

Next, use the design-time data context to specify the design-time view model. You must compile the project before the data will show, but it then should show immediately in the browser when you bind to properties:

{code:xml}
<Grid x:Name="LayoutRoot" Background="White" d:DataContext="{d:DesignInstance sampleData:DesignTimeViewModel, IsDesignTimeCreatable=True}">
    <TextBlock Text="{Binding Welcome}"/>
</Grid>
{code:xml}