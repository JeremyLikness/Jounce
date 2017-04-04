# Workflows

Workflows are largely inspired by Rob Eisenberg's [Caliburn](http://caliburn.codeplex.com/) framework. This is not the exact implementation used there - it is far more lightweight. It is an implementation of co-routines that allow you to fire asynchronous processes in a sequential fashion. This improves the readability of code and provides more flexibility for managing long running processes.

The _IWorkflow_ interface defines:

{code:c#}
public interface IWorkflow
{
    void Invoke();
    Action Invoked { get; set; }
}
{code:c#}

The _Invoke()_ method is called to start the unit of work. When the unit is completed, it should call _Invoked_. A _WorkflowController_ is provided and handles setting the _Invoked_ property; this should not be set by you. 

A few workflow implementations exist:

**WorkflowAction** defines a generic action that can either finish immediately (Jounce will call _Invoked_ for you) or is long running (you call _Invoked_ when done). 
**WorkflowBackgroundWorker** launches a background worker and waits for it to finish.
**WorkflowDelay** will wait a specified period of time before continuing the workflow.
**WorkflowEvent** wraps an existing event. You can type it as _WorkflowEvent<T>_. You specify how the event is triggered and how to wire in the completed handler, and Jounce does the rest - it will fire the event, wait for it to complete, then store the result.
**WorkflowRoutedEvent** does the same with routed events.

Here is an example workflow - it will not block the UI thread, but each workflow item will run asynchronously and the workflow will wait for it to finish, then continue at the next statement after the _yield_. 

{code:c#}
public MainPage()
        {
            InitializeComponent();
            WorkflowController.Begin(Workflow(),
                                     ex => JounceHelper.ExecuteOnUI(() => MessageBox.Show(ex.Message)));
        }

        private IEnumerable<IWorkflow> Workflow()
        {
            Button.Visibility = Visibility.Visible;
            Text.Text = "Initializing...";
            Button.Content = "Not Yet";
            Button.IsEnabled = false;

            // wait two seconds
            yield return new WorkflowDelay(TimeSpan.FromSeconds(2));

            Button.IsEnabled = true;
            Text.Text = "First Click";
            Button.Content = "Click Me!";

            yield return new WorkflowRoutedEvent(() => { }, h => Button.Click += h, h => Button.Click -= h);

            Text.Text = "Now we'll start counting!";
            Button.Content = "Click Me!";

            yield return new WorkflowRoutedEvent(() => { }, h => Button.Click += h, h => Button.Click -= h);

            Button.IsEnabled = false;
            Button.Content = "Counting...";

            yield return new WorkflowBackgroundWorker(_BackgroundWork, _BackgroundProgress);

            Text.Text = "We're Done.";
            Button.Visibility = Visibility.Collapsed;
        }

        private void _BackgroundProgress(BackgroundWorker arg1, ProgressChangedEventArgs arg2)
        {
            Text.Text = string.Format("{0}% complete, counted to: {1}", arg2.ProgressPercentage, (int) arg2.UserState);
        }

        private static void _BackgroundWork(BackgroundWorker obj)
        {
            for (var x = 0; x < int.MaxValue; x++)
            {
                var mod = (int)Math.Ceiling(int.MaxValue/100);

                if (x%mod == 0)
                {
                    obj.ReportProgress(x/mod, x);
                }
            }
        }
{code:c#}