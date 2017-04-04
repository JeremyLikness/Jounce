# Finding a View Model with the Router

While the [Event Aggregator](Event-Aggregator) is designed to handle "global/generic" events in a publisher/subscriber model, view models may interact directly. In order to handle "finding" other view models, the view model router can be used. It is part of the base class that all Jounce view models derive from.

To resolve a view model using the view model interface, call the router and pass either a type (assuming the view model is tagged by type) or a string that represents the tag you exported the view model as.

For example, if you exported a view model with the tag "CustomViewModel", you can find it this way:

{code:c#}
var viewModel = Router.ResolveViewModel("CustomViewModel"); 
{code:c#}

This looser coupling allows you to reference few models that may exist in non-referenced, dynamically loaded assemblies without having to reference the view model type directly.

You can also resolve the fully-typed view model: 

{code:c#}
var viewModel = Router.ResolveViewModel<CustomViewModel>("CustomViewModel"); 
{code:c#}

Sometimes you might access a view model before it is bound to a view. Jounce lazily loads all references, so no instance of the view model will be created until it is needed. The default behavior is to create the view model and pass it back, without calling the Activate(string viewName) method (Initialize() is always called when the view model is created). If you want to activate the view model as well, pass "true" as the first parameter, like this:

{code:c#}
var viewModel = Router.ResolveViewModel<CustomViewModel>(true,"CustomViewModel"); 
{code:c#}

This will also call Activate with string.Empty for the view name.

Once you have a reference, you can then interact the view as needed. 

**Note: do not attempt to call the router from your constructor.** MEF will not have wired the imports yet. Instead, implement IPartImportsSatisfiedNotification like this: 

{code:c#}
[ExportAsViewModel("MyViewModel")](ExportAsViewModel(_MyViewModel_))
public class MyViewModel : BaseViewModel, IPartImportsSatisfiedNotification
{
   private IViewModel _otherViewModel; 

   public void OnImportsSatisfied()
   {
      _otherViewModel = Router.ResolveViewModel("OtherViewModel");
   }
}
{code:c#}
