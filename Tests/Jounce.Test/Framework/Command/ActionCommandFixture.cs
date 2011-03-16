using System;
using Jounce.Framework.Command;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jounce.Test.Framework.Command
{
    [TestClass]
    public class ActionCommandFixture
    {
        [TestMethod]
        public void Execute_Fires_Designated_Action()
        {
            bool executed = false;
            ActionCommand<int> cmd = new ActionCommand<int>(i =>
                                                                {
                                                                    executed = true;
                                                                    Assert.IsTrue(30 == i);
                                                                });
            cmd.Execute(30);
            Assert.IsTrue(executed);
        }

        [TestMethod]
        public void Execute_Does_Not_Fire_If_CanExecute_False()
        {
            bool executed = false;
            ActionCommand<int> cmd = new ActionCommand<int>(i =>
            {
                executed = true;
                Assert.Fail("Should not fire since CanExecute is false.");
            }, i => false);
            cmd.Execute(30);
            Assert.IsFalse(executed);
        }

        [TestMethod]
        public void RaiseCanExecuteChanged_Does_Not_Throw_If_No_Subscribers()
        {
            ActionCommand<int> cmd = new ActionCommand<int>(i => { }, i => true);
            cmd.RaiseCanExecuteChanged();
        }

        [TestMethod]
        public void RaiseCanExecuteChanged_Fires_CanExecuteChanged_Event()
        {
            bool eventFired = false;
            ActionCommand<int> cmd = new ActionCommand<int>(i => { }, i => true);
            EventHandler eh = null;
            eh = (o, e) => { cmd.CanExecuteChanged -= eh;
                               eventFired = true;
            };
            cmd.CanExecuteChanged += eh;
            cmd.RaiseCanExecuteChanged();

            Assert.IsTrue(eventFired);
        }

        [TestMethod]
        public void OverrideAction_Sets_Overriden_To_True()
        {
            ActionCommand<int> cmd = new ActionCommand<int>(i => { }, i => true);
            cmd.OverrideAction(i => {});
            Assert.IsTrue(cmd.Overridden);
        }

    }
}