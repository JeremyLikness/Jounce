# Binding View Models to Views

In Jounce, binding a view model to a view is easy. Anywhere in the application, you simply need to export a **ViewModelRoute**. The route is passed a view model and a view, and Jounce takes care of the rest.

For example, to bind the view model tagged "Calc" to the view tagged "Shell" you do this:

{code:c#}
[Export](Export)
public ViewModelRoute Binding
{
    get
    {
        return ViewModelRoute.Create("Calc", "Shell");
    }
}    
{code:c#}

If you are exporting views and view models usingly typed names, the routes can be created using the types as well, like this: 

{code:c#}
[Export](Export)
public ViewModelRoute Binding
{
    get
    {
        return ViewModelRoute.Create<CalcViewModel,ShellView>();
    }
}    
{code:c#}

You may also perform an explicit, runtime binding like this: 

{code:c#}
[Import](Import)
public IFluentViewModelRouter Router { get; set; }

public void SomeMethod()
{
   Router.RouteViewModelForView("Calc","Shell");
}

{code:c#}

If you export views and view models using types, you can use the type-safe version like this: 

{code:c#}
[Import](Import)
public IFluentViewModelRouter Router { get; set; }

public void SomeMethod()
{
   Router.RouteViewModelForView<CalcViewModel,ShellView>();
}

{code:c#}


That's it! A view model may be bound to multiple views.