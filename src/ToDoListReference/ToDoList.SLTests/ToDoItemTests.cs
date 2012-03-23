using System;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ToDoList.Contracts;
using ToDoList.Model;

namespace ToDoList.SLTests
{
    [TestClass]
    public class ToDoItemTests
    {
        private ToDoItem _target;
        private Mock<IPublisher> _mockPublisherSubscriber;

        [TestInitialize]
        public void Initialize()
        {
            _target = new ToDoItem();
            _mockPublisherSubscriber = new Mock<IPublisher>();
            _target.PubSub = _mockPublisherSubscriber.Object;
        }

        [Tag("ToDoCtor")]
        [TestMethod]
        public void WhenConstructedExpectIdIsNotNullOrEmpty()
        {
            Assert.IsNotNull(_target.Id, "Id should not be null.");
            Assert.AreNotEqual(Guid.Empty, _target.Id, "Id should not be empty.");
        }

        [TestMethod]
        public void WhenMarkedCompleteExpectIsCompleteSetAndCompletedDateUpdated()
        {
            var now = DateTime.Now;
            var acceptableTimespan = TimeSpan.FromMilliseconds(100);
            _target.MarkComplete();
            Assert.IsTrue(_target.IsComplete, "Is complete should be set.");
            Assert.IsTrue(_target.CompletedDate - now <= acceptableTimespan,
                          "Completed date was not updated to now.");
        }

        [ExpectedException(typeof (InvalidOperationException))]
        [TestMethod]
        public void WhenMarkedCompleteAfterCompleteExpectException()
        {
            _target.MarkComplete();

            // second call should generate the exception
            _target.MarkComplete();
        }

        [TestMethod]
        public void WhenDueDateIsInPastExpectPastDueIsTrue()
        {
            _target.DueDate = DateTime.Now.AddDays(-1);
            Assert.IsTrue(_target.IsPastDue, "Past due should be true when due date is in the past.");
        }

        [TestMethod]
        public void WhenDueDateIsInFutureExpectPastDueIsFalse()
        {
            _target.DueDate = DateTime.Now.AddDays(1);
            Assert.IsFalse(_target.IsPastDue, "Past due should be false when due date is in the future.");
        }

        [TestMethod]
        public void WhenDueDateIsTomorrowExpectIsDueTomorrowIsTrue()
        {
            _target.DueDate = DateTime.Now.Date.AddDays(1);
            Assert.IsTrue(_target.IsDueTomorrow, "Is due tomorrow should be true when due date is tomorrow.");
        }

        [TestMethod]
        public void WhenDueDateIsNotTomorrowExpectIsDueTomorrowIsFalse()
        {
            _target.DueDate = DateTime.Now.Date.AddDays(2);
            Assert.IsFalse(_target.IsDueTomorrow, "Is due tomorrow should be false when the due date is not tomorrow.");
        }

        [TestMethod]
        public void WhenDueDateIsInFutureExpectIsInFutureIsTrue()
        {
            _target.DueDate = DateTime.Now.AddHours(1);
            Assert.IsTrue(_target.IsInFuture, "Is in future should be true when due date is in the future.");
        }

        [TestMethod]
        public void WhenDueDateIsInPastExpectIsInFutureIsFalse()
        {
            _target.DueDate = DateTime.Now.AddHours(-1);
            Assert.IsFalse(_target.IsInFuture, "Is in future should be false when due date is in the past.");
        }

        [TestMethod]
        public void WhenDueDateIsInThePastExpectIsDueNextWeekIsFalse()
        {
            _target.DueDate = DateTime.Now.AddHours(-1);
            Assert.IsFalse(_target.IsDueNextWeek, "Is due next week should be false when due date is in the past.");
        }

        [TestMethod]
        public void WhenDueDateIsNextWeekExpectIsDueNextWeekIsTrue()
        {
            _target.DueDate = DateTime.Now.AddDays(6);
            if (_target.DueDate.DayOfWeek.Equals(DayOfWeek.Sunday))
            {
                _target.DueDate = _target.DueDate.AddDays(1);
            }
            Assert.IsTrue(_target.IsDueNextWeek, "Is due next week should be true when due date is next week.");
        }

        [TestMethod]
        public void WhenMarkedCompleteExpectToDoItemCompleteMessage()
        {
            IToDoItem item = null;
            _mockPublisherSubscriber.Setup(
                m => m.Publish(It.IsAny<MessageToDoItemComplete>()))
                .Callback((MessageToDoItemComplete m) =>
                          item = m.Item);
            _target.MarkComplete();
            _mockPublisherSubscriber.Verify(
                m => m.Publish(It.IsAny<MessageToDoItemComplete>()),
                Times.Once(),
                "Item complete message should be published when marked complete.");
            Assert.IsTrue(ReferenceEquals(item, _target),
                          "Message should contain the to-do item.");
        }

        [TestMethod]
        public void WhenCreateNewExpectNewItemMessage()
        {
            IToDoItem item = null;
            _mockPublisherSubscriber.Setup(
                m => m.Publish(It.IsAny<MessageNewToDoItem>()))
                .Callback((MessageNewToDoItem m) =>
                          item = m.Item);
            _target.CreateNew();
            _mockPublisherSubscriber.Verify(
                m => m.Publish(It.IsAny<MessageNewToDoItem>()),
                Times.Once(),
                "Item new message should be published when create new is called.");
            Assert.IsTrue(ReferenceEquals(item, _target),
                          "Message should contain the to-do item.");
        }
    }
}