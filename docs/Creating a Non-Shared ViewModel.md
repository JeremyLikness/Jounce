# Creating a Non-Shared View Model

There are times when you must generate a new copy of a view model instead of using the default, shared view model that Jounce provides. This is not a problem. In fact, because Jounce provides a "lazy load" service for view models, if you have certain view models that are only non-shared, a single copy will never be generated unless you ask for one. The built-in navigation and view routing system supports the shared model. For non-shared, simply import an instance of IViewModelRouter: 

{code:c#}
[Import](Import)
public IViewModelRouter Router { get; set; }
{code:c#}

This is already provided for you in the base view model class. 

Next, simply pass the view model tag and request a non-shared copy: 

{code:c#}
var vm = Router.GetNonSharedViewModel("MyViewModelTag");
{code:c#}

If you export view models using types, you can use the type-safe version (the tag is passed as the full type name) like this: 

{code:c#}
var vm = Router.GetNonSharedViewModel<MyViewModel>();
{code:c#}

That's it - you now have a new copy of the view model to use.