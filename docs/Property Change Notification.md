# Property Change Notification

The BaseNotify class provides basic property change notification. This is based on work ranging from [Prism](http://compositewpf.codeplex.com) to [Caliburn Micro](http://caliburnmicro.codeplex.com).

You can strongly type your notifications by providing an expression that points to a property:

{code:c#}
public class MyNotifyClass : BaseNotify
{

   private string _title;

   public string Title
   {
       get { return _title; }
       set
       {
           _title = value;
           RaisePropertyChanged(()=>Title);
       }
   }
}
{code:c#}

If you are concerned with performance, you can use a variable that is initialized in the constructor and reference the variable for property change notifications.