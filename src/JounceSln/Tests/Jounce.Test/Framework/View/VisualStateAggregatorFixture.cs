using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Input;
using Jounce.Framework.View;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jounce.Test.Framework.View
{
    [TestClass]
    public class VisualStateAggregatorFixture : SilverlightTest
    {
        [TestMethod]
        public void AddSubscription()
        {
            Button button = new Button();
            button.Click += button_Click;

            VisualStateAggregator vsa = new VisualStateAggregator();
            vsa.AddSubscription(button, "Click", "MyState", true);

            this.TestPanel.Children.Add(button);

            var list = VisualStateManager.GetVisualStateGroups(this.TestPanel);
            VisualStateGroup vsg = new VisualStateGroup();
            vsg.States.Add(new VisualState());
            list.Add(vsg);

            ButtonAutomationPeer peer =new ButtonAutomationPeer(button);
            IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
            invokeProv.Invoke();
        }

        void button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var x = 0;
            x++;
        }
    }
}