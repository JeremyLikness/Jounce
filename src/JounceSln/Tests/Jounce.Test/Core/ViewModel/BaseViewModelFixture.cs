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

        [TestMethod]
        public void Initialize_Calls_InitializeVm()
        {
            var logger = new Mock<ILogger>();
            logger.Setup(l => l.Log(LogSeverity.Information, "Jounce.Test.Core.ViewModel.BaseViewModelFixture+DummyViewModel", "Initialize")).Verifiable();

            DummyViewModel dm = new DummyViewModel();
            dm.Logger = logger.Object;

            dm.Initialize();
            Assert.IsTrue(dm.InitializeVmFired);
            logger.Verify();
        }

        [TestMethod]
        public void Activate_Calls_ActivateView()
        {
            var logger = new Mock<ILogger>();
            logger.Setup(l => l.LogFormat(LogSeverity.Information, "Jounce.Test.Core.ViewModel.BaseViewModelFixture+DummyViewModel", "{0} [{1}]", "Activate", "MyView")).Verifiable();

            DummyViewModel dm = new DummyViewModel();
            dm.Logger = logger.Object;

            dm.Activate("MyView");
            Assert.IsTrue(dm.ActivateViewFired);
            logger.Verify();
        }

        [TestMethod]
        public void Deactivate_Calls_DeactivateView()
        {
            var logger = new Mock<ILogger>();
            logger.Setup(l => l.LogFormat(LogSeverity.Information, "Jounce.Test.Core.ViewModel.BaseViewModelFixture+DummyViewModel", "{0} [{1}]", "Deactivate", "MyView")).Verifiable();

            DummyViewModel dm = new DummyViewModel();
            dm.Logger = logger.Object;

            dm.Deactivate("MyView");
            Assert.IsTrue(dm.DeactivateViewFired);
            logger.Verify();
        }

        public class DummyViewModel : BaseViewModel
        {
            public bool InitializeVmFired { get; private set; }
            public bool ActivateViewFired { get; private set; }
            public bool DeactivateViewFired { get; private set; }

            protected override void InitializeVm()
            {
                base.InitializeVm();
                InitializeVmFired = true;
            }

            protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
            {
                base.ActivateView(viewName, viewParameters);
                ActivateViewFired = true;
            }

            protected override void DeactivateView(string viewName)
            {
                base.DeactivateView(viewName);
                DeactivateViewFired = true;
            }
        }
    }
}