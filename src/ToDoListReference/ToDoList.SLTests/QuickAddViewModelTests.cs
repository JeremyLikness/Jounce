using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Jounce.Core.Event;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ToDoList.Contracts;
using ToDoList.Model;
using ToDoList.ViewModels;

namespace ToDoList.SLTests
{
    [TestClass]
    public class QuickAddViewModelTests
    {
        private const string TITLE = "Test Task";
            
        private QuickAddViewModel _target;
        private Mock<IEventAggregator> _eventAggregator;
        private Mock<IToDoListApplicationContext> _appContext;

        [TestInitialize]
        public void Initialize()
        {
            _eventAggregator = new Mock<IEventAggregator>();
            _appContext = new Mock<IToDoListApplicationContext>();
            
            _target = new QuickAddViewModel
                          {
                              GetToDoItem = () => new ToDoItem
                                                      {
                                                          PubSub = new PubSub
                                                                       {
                                                                           EventAggregator = _eventAggregator.Object
                                                                       }
                                                      },
                              EventAggregator = _eventAggregator.Object,
                              ApplicationContext = _appContext.Object
                          };                       
        }

        [TestMethod]
        public void WhenDueDateIsInPastExpectViewModelToHaveError()
        {
            _target.DueDate = DateTime.Now.AddDays(-1).ToString();
            var errors = ((INotifyDataErrorInfo) _target).GetErrors("DueDate")
                            as IEnumerable<string>;
            Assert.IsTrue(errors.Count() > 0,
                "Due date in past should have triggered an error.");
        }

        [TestMethod]
        public void WhenErrorsExistExpectCommitCommandDoesNotExecute()
        {
            _eventAggregator.Setup(
                e => e.Publish(It.IsAny<MessageNewToDoItem>()));
            _target.CommitCommand.Execute(null);
            _eventAggregator.Verify(
                e => e.Publish(It.IsAny<MessageNewToDoItem>()),
                Times.Never(),
                "Message should not be published wihen errors exist.");
        }

        [TestMethod]
        public void WhenSubmittedExpectNewItemMessage()
        {
            var dueDate = DateTime.Now.AddDays(1);
            IToDoItem target = null;
            _eventAggregator.Setup(
                e => e.Publish(It.IsAny<MessageNewToDoItem>()))
                .Callback((MessageNewToDoItem m) =>
                    target = m.Item);
            _target.Title = TITLE;
            _target.DueDate = dueDate.ToString();
            _target.CommitCommand.Execute(null);
            _eventAggregator.Verify(
                e => e.Publish(It.IsAny<MessageNewToDoItem>()),
                Times.Exactly(1),
                "Message should have been published.");
            Assert.IsNotNull(target, 
                "Message should contain target item.");
            Assert.AreEqual(TITLE, target.Title,
                "Title was not correct.");
            Assert.IsTrue(dueDate.IsCloseTo(target.DueDate),  "Due date does not match.");
        }        

        [TestMethod]
        public void WhenTitleEditedPromptBeforeClose()
        {
            _appContext.SetupProperty(a => a.PromptBeforeClose);
            _target.Title = TITLE;
            Assert.IsTrue(_appContext.Object.PromptBeforeClose, "Setting the title should set the property to prompt before closing the browser.");
        }
    }
}