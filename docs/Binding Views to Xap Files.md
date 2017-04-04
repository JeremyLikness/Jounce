# Binding Views to XAP Files

Often you may have views located in a separate, dynamically loaded XAP file. However, why should you have to worry about the plumbing necessary to handle the load when the user requests the view for the first time? Jounce lets you treat all views the same: if you want to activate a view, you raise the **ViewNavigationArgs** event, and that's it. 

If the view happens to live somewhere else, you can give Jounce a simple hint that maps the view tag to the XAP file it lives in. Anywhere in the project you simply export a **ViewXapRoute** like this:

{code:c#}
[Export](Export)
public ViewXapRoute DynamicRoute
{
            get
            {
                return ViewXapRoute.Create("Dynamic", "RegionManagementDynamic.xap");
            }
}
{code:c#}

You may also perform runtime binding like this - you must be sure to execute this before you request the view:

{code:c#}
[Import](Import)
public IFluentViewXapRouter XapRouter { get; set; }

public void MyMethod()
{
   XapRouter.RouteViewInXap("Dynamic","RegionManagementDynamic.xap"); 
}

{code:c#}

The first time the "Dynamic" view is requested, Jounce will automatically load the RegionManagementDynamic xap file.