# Registering a View Model

Registering a view model for management by the Jounce framework is as simple as exporting it using the "ExportAsViewModel" tag. 

You may tag your view model any way you like. If you prefer a more strongly-typed method, export it using a constant: 

{code:c#}
[ExportAsViewModel(Constants.MYVIEWMODEL)](ExportAsViewModel(Constants.MYVIEWMODEL))
public class MyViewModel : BaseViewModel
{
}
{code:c#}

You can also export based on type. If you export based on type, the tag will be the full type name. For example, a view model in project "MyProject" and namespace "MyProject.ViewModels" called "MyViewModel" will have a tag of "MyProject.ViewModels.MyViewModel" if it is exported like this: 

{code:c#}
[ExportAsViewModel(typeof(MyViewModel)](ExportAsViewModel(typeof(MyViewModel))
public class MyViewModel : BaseViewModel
{
}
{code:c#}

This must be done to locate view models and bind view models to views.

At runtime, you can also find out what views have been routed for the view model via the Router (IViewModelRouter) property provided in the base view model class. To get a list of tags, call: 

{code:c#}
string[]() tags = Router.GetViewTagsForViewModel("MyViewModelTag"); 
{code:c#}

Then, you can grab the view metadata this way: 

{code:c#}
IExportAsViewMetadata metadata = GetMetaDataForView(tags[0](0)); 
{code:c#}