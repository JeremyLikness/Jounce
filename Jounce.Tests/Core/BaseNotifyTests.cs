using System;
using Jounce.Core.Model;
using Jounce.Tests.Helpers;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jounce.Tests.Core
{
    /// <summary>
    /// Tests for the <see cref="BaseNotify"/> class
    /// </summary>
    [TestClass]
    public class BaseNotifyTests : SilverlightTest
    {
        private BaseNotify _target;
        private const string PROPERTY_NAME = "TestProperty";
        private const string PROPERTY2_NAME = "SecondTestProperty";
        private static readonly string[] _properties = new[] {PROPERTY_NAME, PROPERTY2_NAME};

        private BaseNotifyTestHelper Target
        {
            get { return (BaseNotifyTestHelper) _target; }
        }

        /// <summary>
        /// Initialize targets
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            _target = new BaseNotifyTestHelper();
        }

        [Asynchronous]
        [TestMethod]
        public void GivenInitialStateWhenRaisePropertyChangeCalledThenEventShouldFire()
        {
            var fired = false;
            _target.PropertyChanged += (o, e) =>
                                           {
                                               fired = true;
                                               Assert.AreEqual(PROPERTY_NAME, e.PropertyName,
                                                               "RaisePropertyChanged did not fire property change event with correct property name.");
                                               EnqueueTestComplete();
                                           };
            Target.RaisePropertyChanged(PROPERTY_NAME);
            Assert.IsTrue(fired, "RaisePropertyChanged did not fire property change event.");
        }

        [Asynchronous]
        [TestMethod]
        public void GivenInitialStateWhenRaisePropertyChangeCalledWithListThenMultipleEventsShouldFire()
        {
            var count = 0;
            _target.PropertyChanged += (o, e) =>
                                           {
                                               count++;
                                               CollectionAssert.Contains(_properties, e.PropertyName,
                                                                         "RaisePropertyChanged with list did not fire with correct property name.");
                                               if (count == 2)
                                               {
                                                   EnqueueTestComplete();
                                               }
                                           };
            Target.RaisePropertyChanged(_properties);
            Assert.AreEqual(2, count, "RaisePropertyChanged with list did not fire for all properties.");
        }

        [Asynchronous]
        [TestMethod]
        public void GivenInitialStateWhenRaisePropertyChangeCalledWithExpressionThenEventShouldFire()
        {
            var fired = false;
            _target.PropertyChanged += (o, e) =>
                                           {
                                               fired = true;
                                               Assert.AreEqual(BaseNotifyTestHelper.PROPERTY_NAME,
                                                               e.PropertyName,
                                                               "RaisePropertyChanged with expression did not evaluate to the correct property name.");
                                               EnqueueTestComplete();
                                           };
            Target.RaisePropertyChangedWithExpression();
            Assert.IsTrue(fired, "RaisePropertyChanged did not fire property change event.");
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void GivenInitialStateWhenExtractPropertyNameCalledWithNullThenThrowArgumentNullException()
        {
            Target.ExtractPropertyName<string>(null);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void GivenInitialStateWhenExtractPropertyNameCalledWithInvalidPropertyThenThrowArgumentException()
        {
            Target.ExtractPropertyName(() => true);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void GivenInitialStateWhenExtractPropertyNameCalledWithFieldThenThrowArgumentException()
        {
            Target.ExtractPropertyNameWithField();
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void GivenInitialStateWhenExtractPropertyNameCalledWithStaticPropertyThenThrowArgumentException()
        {
            Target.ExtractPropertyNameWithStaticProperty();
        }

        [TestMethod]
        public void GivenInitialStateWhenExtractPropertyNameCalledWithValidPropertyThenReturnPropertyName()
        {
            var actual = Target.ExtractPropertyName(() => Target.TestProperty);
            Assert.AreEqual(BaseNotifyTestHelper.PROPERTY_NAME, actual, "Extract property name failed to return the correct property name.");
        }
    }
}