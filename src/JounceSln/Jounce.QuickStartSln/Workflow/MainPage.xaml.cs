using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Jounce.Core.View;
using Jounce.Core.Workflow;
using Jounce.Framework;
using Jounce.Framework.Workflow;

namespace Workflow
{
    /// <summary>
    ///     This is an example of a workflow - this view gets exported as the 
    ///     shell so Jounce will wire it as the root visual
    /// </summary>
    [ExportAsView("Workflow", IsShell = true)]
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            // workflows yield IWorkflow objects. We can optionally handled errors encountered
            // during the workflow, at which point the workflow is terminated.
            WorkflowController.Begin(Workflow(),
                                     ex => JounceHelper.ExecuteOnUI(() => MessageBox.Show(ex.Message)));
        }

        /// <summary>
        ///     A sequential asynchronous workflow
        /// </summary>
        /// <returns>The workflow steps</returns>
        /// <remarks>
        ///     The work helpers that implement IWorkflow are used to set up various steps or actions.
        ///     The two methods exposed by the contract are Invoke() and Invoked. 
        /// 
        ///     Invoke - you set what happens, but never call - the controller does this
        ///     Invoked - you call this when your work is completed, but never set it - the controller does this
        /// 
        ///     When you yield return an item, the controller will call Invoked. It is then up to the
        ///     class implementation to call Invoke when the work is complete. For example, the WorkflowDelay
        ///     creates a dispatch timer. When invoke is called by the controller, the timer is started.
        ///     When the tick event fires, Invoked is called to indicate the work is done. 
        /// 
        ///     We also provide support for wrapping events. In this case, there is no "start" to the event
        ///     so the start delegate is empty. In the case of a Storyboard, for example, you would pass
        ///     "Storyboard.Begin" for the first Action, and then stop or unhook the storyboard when
        ///     done. There is a helper for both direct events and routed events.
        /// </remarks>
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

            // the initial action is to do nothing, but this will wait for the click to fire before
            // continuing - the events are always unhooked at the end of the step
            yield return new WorkflowRoutedEvent(() => { }, h => Button.Click += h, h => Button.Click -= h);

            Text.Text = "Now we'll start counting!";
            Button.Content = "Click Me!";

            yield return new WorkflowRoutedEvent(() => { }, h => Button.Click += h, h => Button.Click -= h);

            Button.IsEnabled = false;
            Button.Content = "Counting...";

            // this will count some numbers for us
            // you can leave the second parameter off if your background thread won't report progress
            // keep in mind the whole point of the background worker is to marshall updates to the 
            // UI thread, so we don't have to worry about doing it explicitly
            yield return new WorkflowBackgroundWorker(_BackgroundWork, _BackgroundProgress);

            Text.Text = "We're Done.";
            Button.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        ///     Show some updates - this is always on the UI thread
        /// </summary>
        /// <param name="arg1">The worker</param>
        /// <param name="arg2">The change args</param>
        private void _BackgroundProgress(BackgroundWorker arg1, ProgressChangedEventArgs arg2)
        {
            Text.Text = string.Format("{0}% complete, counted to: {1}", arg2.ProgressPercentage, (int) arg2.UserState);
        }

        /// <summary>
        ///     The main "work" loop
        /// </summary>
        /// <param name="obj">The worker</param>
        private static void _BackgroundWork(BackgroundWorker obj)
        {
            // count to maximum integer value
            for (var x = 0; x < int.MaxValue; x++)
            {
                // get 1% increments
                var mod = (int)Math.Ceiling(int.MaxValue/100);

                // if we did this every number we'd never finish
                if (x%mod == 0)
                {
                    obj.ReportProgress(x/mod, x);
                }
            }
        }

        /// <summary>
        ///     Auto-update the binding to show text as it is typed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb != null)
            {
                var binding = tb.GetBindingExpression(TextBox.TextProperty);
                if (binding != null)
                {
                    binding.UpdateSource();
                }
            }
        }
    }
}