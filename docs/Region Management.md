# Region Management

Region management with Jounce is extremely simple. 

[Prism](http://compositewpf.codeplex.com) established this pattern and contains a very rich set of features and adapters to support it. The concept: put a container somewhere, and tag it as a region. Next, tag a view to target the region. When the view is navigated to, it appears in the region. Different regions handle views different ways.

Jounce provides two region managers "out of the box." One is for an items control, which will handle keeping multiple views at once. The other is for a content control, which will handle one view at a time. Often a content control is all that is needed. If you are looking to serve one view at a time, but wish to animate views into and out of display, then the items control makes sense because you can animate the old view while transitioning the new. 

To tag a region, you use an attached property. This example tags a tab control as a "TabRegion" (the adapter is custom, more on that later) and an items control as the "AppRegion."

{code:xml}
<Controls:TabControl Grid.Row="1" Regions:ExportAsRegion.RegionName="TabRegion"/>
<ItemsControl Grid.Row="2" HorizontalAlignment="Stretch" VerticalAlignment="Stretch"
                HorizontalContentAlignment="Stretch" VerticalContentAlignment="Stretch"
                Regions:ExportAsRegion.RegionName="AppRegion">
    <ItemsControl.ItemsPanel>
        <ItemsPanelTemplate>
            <StackPanel Orientation="Horizontal"/>
        </ItemsPanelTemplate>
    </ItemsControl.ItemsPanel>
</ItemsControl>
{code:xml}

You can also do this fluently, by importing the fluent region manager: 

{code:c#}
[Import](Import)
public IFluentRegionManager RegionManager { get; set; }

private void RegisterRegion()
{
     RegionManager.RegisterRegion(MyItemsControl, "AppRegion");
}
{code:c#}

To route a view to a region, you use the _ExportViewToRegion_ tag. Here is a view that is registered (see [Registering a View](Registering-a-View)) and also tagged for the region:

{code:c#}
[ExportAsView(CIRCLE)](ExportAsView(CIRCLE))
[ExportViewToRegion(CIRCLE,LocalRegions.APP_REGION)](ExportViewToRegion(CIRCLE,LocalRegions.APP_REGION))
public partial class Circle
{
}
{code:c#}

The strongly typed version looks like this:

{code:c#}
[ExportAsView(typeof(Circle)](ExportAsView(typeof(Circle))
[ExportViewToRegion(typeof(Circle),LocalRegions.APP_REGION)](ExportViewToRegion(typeof(Circle),LocalRegions.APP_REGION))
public partial class Circle
{
}
{code:c#}

To fluently route a view, import the fluent region manager and route it like this: 

{code:c#}

RegionManager.ExportViewToRegion("Circle", "AppRegion");

// or
RegionManager.ExportViewToRegion<Circle>("AppRegion");

{code:c#}


Region management is optional and you opt-in by including the regions DLL and then tagging views and regions. After a view is navigated by the framework, and all of the view model/view wiring takes place, the framework fires a _ViewNavigatedArgs_ event. The region manager taps into this and routes the information to the region adapters, which then understand how to activate the view. 

You can also create your own region adapters. The quickstart has an example of a tab control adapter that reads metadata from the exported views to construct the tab titles. Notice that you simply tag the adapter with the _RegionAdatperFor_ attribute and pass the type it will handle, then derive from _RegionAdapterBase_.

{code:c#}
[RegionAdapterFor(typeof(TabControl))](RegionAdapterFor(typeof(TabControl)))
public class TabRegion : RegionAdapterBase<TabControl>
{
    [Import](Import)
    public IEventAggregator EventAggregator { get; set; }

    /// <summary>
    ///     Keep track of views already added
    /// </summary>
    private readonly List<string> _addedViews = new List<string>();

    /// <summary>
    ///     View metadata
    /// </summary>
    [ImportMany(AllowRecomposition=true)](ImportMany(AllowRecomposition=true))
    public Lazy<UserControl, IExportAsViewMetadata>[]() Views { get; set; }               
        
    /// <summary>
    ///     Activates a control for a region
    /// </summary>
    /// <param name="viewName">The name of the control</param>
    /// <param name="targetRegion">The name of the region</param>
    public override void ActivateControl(string viewName, string targetRegion)
    {
        ValidateControlName(viewName);
        ValidateRegionName(targetRegion);

        var region = Regions[targetRegion](targetRegion);

        if (!_addedViews.Contains(viewName))
        {
            _addedViews.Add(viewName);
            _SetupTabForView(region, viewName);
        }
                                   
        region.SelectedIndex = _addedViews.IndexOf(viewName);
    }

    /// <summary>
    ///     Set up the tab - insert the view
    /// </summary>
    /// <param name="region">The region</param>
    /// <param name="viewName">The view</param>
    private void _SetupTabForView(ItemsControl region, string viewName)
    {
        var metadata =
            (from v in Views where v.Metadata.ExportedViewType.Equals(viewName) select v.Metadata).FirstOrDefault();

        var header = metadata == null ? viewName : metadata.MenuName;
            
        var tabControlItem = new TabItem {Header = header, Content = Controls[viewName](viewName)};            

        region.Items.Add(tabControlItem);           
    }
}
{code:c#}

Those are the only steps required - tag the region, tag the view, and route the view to the region. When the view is navigated to (or when navigation is raised to deactivate) the region managers will handle it.

The quickstart contains an example of a view that is closed and animates both to appear and hide based on the wiring between the view model and the visual state manager.