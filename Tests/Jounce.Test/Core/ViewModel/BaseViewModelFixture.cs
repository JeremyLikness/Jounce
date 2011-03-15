using System;
using Jounce.Core.Application;
using Jounce.Core.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Jounce.Test.Core.ViewModel
{
    [TestClass]
    public class BaseViewModelFixture
    {
        [TestMethod]
        public void GoToVisualStateForView_Calls_Action_For_Appropriate_View()
        {
            bool actionExecuted = false;
            DummyViewModel vm = new DummyViewModel();
            vm.RegisterVisualState("MyView", (state, useTransition) =>
                                                                        {
                                                                            actionExecuted = true; 
                                                                            Assert.AreEqual("MyState", state); 
                                                                            Assert.AreEqual(true, useTransition);
                                                                        });
            vm.GoToVisualStateForView("MyView", "MyState", true);

            Assert.IsTrue(actionExecuted);
        }

        [TestMethod]
        public void Initialize_Calls_Logger()
        {
            var logger = new Mock<ILogger>();
            logger.Setup(l => l.Log(LogSeverity.Information, "Jounce.Test.Core.ViewModel.BaseViewModelFixture+DummyViewModel", "Initialize")).Verifiable();

            DummyViewModel vm = new DummyViewModel();
            vm.Logger = logger.Object;

            vm.Initialize();

            logger.Verify();
        }

        public class DummyViewModel : BaseViewModel
        {
        }
    }
}