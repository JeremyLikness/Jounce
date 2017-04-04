# Registering a View

Registering a view is quite simple: just like the view model, you export the view with a tag. 

Exactly one view must be registered as the "shell" view by setting IsShell = true:

{code:c#}
[ExportAsView("Shell",IsShell = true)](ExportAsView(_Shell_,IsShell-=-true))
public partial class Shell 
{
        public Shell()
        {
            InitializeComponent();
        }
}
{code:c#}

If you choose, you may use a strongly-typed export that will default the view tag to the full type name of the view. A view called "MyView" in "MyProject" namespace "Views" will be exported with the tag "MyProject.Views.MyView": 

{code:c#}
[ExportAsView(typeof(MyView),IsShell = false)](ExportAsView(typeof(MyView),IsShell-=-false))
public partial class MyView 
{
        public MyView()
        {
            InitializeComponent();
        }
}
{code:c#}

You may add additional meta data to a view to help with constructing navigation, filtering views, etc. 

Additional tags include: 

**Category** - use this to group views. For example, top level navigation views might be tagged "Main". 
**MenuName** - use this to dynamically construct menu information and provide a user-friendly name for the view that may not match the tag 
**ToolTip** - use this to specify a detailed description of the view for use in tool tips and descriptions.

At runtime, you can get the metadata for a view using the IViewModelRouter (provided as Router in the base view model): 

{code:c#}
IExportAsViewMetadata metadata = Router.GetMetadataForView("MyViewTag"); 
{code:c#}