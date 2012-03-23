using System;
using System.ComponentModel;
using System.Windows.Controls;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ToDoList.Contracts;
using ToDoList.ViewModels;
using ToDoList.Views;

namespace ToDoList.SLTests
{
    [Tag("Views")]
    [TestClass]
    public class QuickAddViewTests : SilverlightTest
    {
        private QuickAddView _target;
        private QuickAddViewModel _mockViewModel;

        [TestInitialize]
        public void TestInitialize()
        {
            _target = new QuickAddView();
            _mockViewModel = new QuickAddViewModel();
        }

        [Asynchronous]
        [TestMethod]
        public void TestDueDateBinding()
        {
            var now = DateTime.Now.Date;

            _target.Loaded +=
                (o, e) =>
                    {
                        _target.DataContext = _mockViewModel;
                        var textBox = TestHelper
                            .FindInChildrenByName<TextBox>(TestPanel, 
                            "DueDate");
                        textBox.Text = now.ToString();
                        Assert.AreEqual(now, 
                            DateTime.Parse(_mockViewModel.DueDate).Date,
                            "Due date was not set through binding.");
                        EnqueueTestComplete();
                    };
            TestPanel.Children.Add(_target);
        }
    }
}