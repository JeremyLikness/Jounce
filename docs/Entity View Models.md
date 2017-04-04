# Entity View Models

Use the _BaseEntityViewModel_ for view models that help facilitate CRUD operations or drive interactive forms with validation. 

The entity view models are dervied from [Basic View Models](Basic-View-Models) but add additional functionality. 

The _INotifyDataErrorInfo_ interface is implemented to facilitate validations by allowing you to call:

{code:c#}
if (!_Validate())
{
   SetError(()=>Property,"There was an error with this property."); 
}
else
{
   Clearerrors(()=>Property);
}
{code:c#}

Using the built-in validation framework with Silverlight, errors registered with these commands will appear in the UI and also in any validation summary controls such as those offered by the Silverlight toolkit.

Override _ValidateAll()_ to provide aggregate validation over multiple properties.

This view model also exposes the _Committed_ flag, which is like an inverse dirty flag. The initial state of the view model should be to set Committed to "true" meaning any changes needed have been committed (as the fields have not been touched). Jounce will automatically detect when a property has changed and set _Committed_ to false. In addition, the CommitCommand can be bound to your save functionality. It will remain disabled unless there are pending changes _and_ no errors. In addition, when fired, it will call _ValidateAll()_ and only if this call succeeds, will then call _OnCommitted()_. Use _OnCommitted_ to process any changes necessary - Jounce will then set the Committed flag to true.