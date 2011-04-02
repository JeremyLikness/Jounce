using System;
using System.Collections.Generic;
using Jounce.Core.Application;
using Jounce.Core.Event;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.View;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Jounce.Test.Framework.View
{
    [TestClass]
    public class ViewRouterFixture
    {
        [TestMethod]
        public void HandleEvent_With_Deactivate_Calls_DeactivateView_And_Publishes_Event()
        {
            Mock<IEventAggregator> eventAggregator = new Mock<IEventAggregator>();
            eventAggregator.Setup(e => e.Publish(It.Is<ViewNavigatedArgs>(a => a.ViewType == "MyViewType" && a.Deactivate))).Verifiable();

            Mock<IViewModelRouter> viewModelRouter = new Mock<IViewModelRouter>();
            viewModelRouter.Setup(v => v.DeactivateView("MyViewType")).Verifiable();

            ViewRouter vr = new ViewRouter
                                {
                                    EventAggregator = eventAggregator.Object,
                                    ViewModelRouter = viewModelRouter.Object
                                };

            var args = new ViewNavigationArgs("MyViewType") { Deactivate = true };

            vr.HandleEvent(args);

            eventAggregator.VerifyAll();
            viewModelRouter.VerifyAll();
        }

        [TestMethod]
        public void HandleEvent_With_Activate_Calls_ActivateView()
        {
            string viewName = "MyViewType";
            Mock<IEventAggregator> eventAggregator = new Mock<IEventAggregator>();
            eventAggregator.Setup(e => e.Publish(It.Is<ViewNavigatedArgs>(a => a.ViewType == viewName && !a.Deactivate))).Verifiable();

            Mock<IViewModelRouter> viewModelRouter = new Mock<IViewModelRouter>();
            viewModelRouter.Setup(v => v.ActivateView(viewName, It.Is<IDictionary<string, object>>(b => b.Count == 0))).Verifiable();

            var args = new ViewNavigationArgs(viewName) { Deactivate = false };
            ViewRouter vr = new ViewRouter
                                {
                                    EventAggregator = eventAggregator.Object,
                                    ViewModelRouter = viewModelRouter.Object,
                                    ViewLocations = new ViewXapRoute[0]
                                };

            vr.HandleEvent(args);

            eventAggregator.VerifyAll();
            viewModelRouter.VerifyAll();
        }

        [TestMethod]
        public void HandleEvent_With_View_Inside_Another_Xap_Calls_RequestXap_On_DeploymentService_And_Activates_View()
        {
            string viewName = "MyViewType";

            Mock<IEventAggregator> eventAggregator = new Mock<IEventAggregator>();
            eventAggregator.Setup(e => e.Publish(It.Is<ViewNavigatedArgs>(a => a.ViewType == viewName && !a.Deactivate))).Verifiable();

            Mock<IViewModelRouter> viewModelRouter = new Mock<IViewModelRouter>();
            viewModelRouter.Setup(v => v.ActivateView(viewName, It.Is<IDictionary<string, object>>(b => b.Count == 0))).Verifiable();

            Mock<IDeploymentService> deploymentService = new Mock<IDeploymentService>();
            deploymentService.Setup(d => d.RequestXap("FooXap.xap", It.IsAny<Action<Exception>>())).Callback<string, Action<Exception>>((s, a) => a.Invoke(null));

            var args = new ViewNavigationArgs(viewName) { Deactivate = false };
            ViewRouter vr = new ViewRouter
            {
                EventAggregator = eventAggregator.Object,
                ViewModelRouter = viewModelRouter.Object,
                ViewLocations = new ViewXapRoute[0],
                DeploymentService = deploymentService.Object
            };

            vr.RouteViewInXap(viewName, "FooXap.xap");
            vr.HandleEvent(args);

            viewModelRouter.VerifyAll();
            deploymentService.VerifyAll();
        }

        //[TestMethod]
        //[ExpectedException(typeof(ArgumentException), "XAP FAIL")]
        //public void HandleEvent_With_View_Inside_Another_Xap_Calls_RequestXap_On_DeploymentService_And_Thows_Exception_If_Xap_Load_Fails()
        //{
        //    string viewName = "MyViewType";

        //    Mock<IEventAggregator> eventAggregator = new Mock<IEventAggregator>();
        //    eventAggregator.Setup(e => e.Publish(It.Is<ViewNavigatedArgs>(a => a.ViewType == viewName && !a.Deactivate))).Verifiable();

        //    Mock<IViewModelRouter> viewModelRouter = new Mock<IViewModelRouter>();
        //    viewModelRouter.Setup(v => v.ActivateView(viewName, It.Is<IDictionary<string, object>>(b => b.Count == 0))).Verifiable();

        //    Mock<IDeploymentService> deploymentService = new Mock<IDeploymentService>();
        //    deploymentService.Setup(d => d.RequestXap("FooXap.xap", It.IsAny<Action<Exception>>())).Callback<string, Action<Exception>>((s, a) => a.Invoke(new ArgumentException("XAP FAIL")));

        //    var args = new ViewNavigationArgs(viewName) { Deactivate = false };
        //    ViewRouter vr = new ViewRouter
        //    {
        //        EventAggregator = eventAggregator.Object,
        //        ViewModelRouter = viewModelRouter.Object,
        //        ViewLocations = new ViewXapRoute[0],
        //        DeploymentService = deploymentService.Object
        //    };

        //    vr.RouteViewInXap(viewName, "FooXap.xap");
        //    vr.HandleEvent(args);

        //    viewModelRouter.VerifyAll();
        //    deploymentService.VerifyAll();
        //}
    }
}