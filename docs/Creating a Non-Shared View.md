# Creating a Non-Shared View

There may be circumstances that require multiple copies of the same view. In that case, you can generate a view directly from Jounce. There are two ways to do this. 

**Using IViewModelRouter** 

Import the IViewModelRouter interface (this is provided for you in the base view model class):

{code:c#}
[Import](Import)
public IViewModelRouter Router { get; set; }
{code:c#}

Next, specify the view you want by passing the view tag and the data context you would like bound (it can be null):

{code:c#}
var view = Router.GetNonSharedView("MyViewTag", null);
{code:c#}

There is also a type-safe version that defaults the tag to the full type name of the view: 

{code:c#}
var view = Router.GetNonSharedView<MyView>(null); 
{code:c#} 

If you pass a Jounce view model, Jounce will automatically hook into the loaded event for the view and tie the view model to the view as well as wire any visual state properties and call initialize and activate.

**Using the JounceViewConverter** 

The Jounce view converter allows you to automatically generate a view in XAML based on the existing view model. It is intended for lists or other containers that have a view model data bound. It will automatically find the associated views and bind the first view, unless you specify the parameter of a view tag in which case the passed view will be generated.

First, add the namespace to your XAML: 

{code:text}
xmlns:ViewModel="clr-namespace:Jounce.Framework.ViewModel;assembly=Jounce"
{code:text}

Next, create a resource key for the Jounce view converter: 

{code:xml}
<UserControl.Resources>
    <ViewModel:JounceViewConverter x:Key="ViewConverter"/>
</UserControl.Resources>
{code:xml}

To automatically spin up the view based on the view model route, and if there is only one view type per view model, use the following convention and simply bind to the existing view model - not that any container can be used:

{code:xml}
<ListBox ItemsSource="{Binding ViewModels}">
    <ListBox.ItemTemplate>
        <DataTemplate>
            <ContentControl Content="{Binding Converter={StaticResource ViewConverter}}"/>
        </DataTemplate>
    </ListBox.ItemTemplate>
</ListBox>
{code:xml}

The above example will work like a data template selector and will change the view based on the view model type, so if you have a mixed list, you can get mixed views output. 

The specify the view, simply pass the view tag in the parameter, like this: 

{code:xml}
<ListBox ItemsSource="{Binding ViewModels}">
    <ListBox.ItemTemplate>
        <DataTemplate>
            <ContentControl Content="{Binding Converter={StaticResource ViewConverter}, ConverterParameter=MyViewTag}"/>
        </DataTemplate>
    </ListBox.ItemTemplate>
</ListBox>
{code:xml}
