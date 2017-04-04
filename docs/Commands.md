# Commands

Jounce supports commanding through IActionCommand and the implementation, ActionCommand.

{code: c#}
ActionCommand<T> : IActionCommand<T> : IActionCommand : ICommand
{code: c#}

This is similar to the DelegateCommand introduced by [Prism](http://compositewpf.codeplex.com), however you can construct your action and condition delegates (or create it with nothing and it will simply default to always true and "do nothing.") There is also an option to override the action which sets a flag that can be interrogated for commands that may transform over time. The "RaiseExecuteChanged" is used to notify when the condition for the command has changed.

{code:c#}
public IActionCommand<MyEntity> SaveCommand { get; private set; }

public MyConstructor()
{
   SaveCommand = new ActionCommand<MyEntity>( entity => Service.Save(entity), entity=>IsValid(entity) );
}

{code:c#}