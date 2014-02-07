using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JounceDesktop.Core.Model;
using Jounce.Tests.Helpers;

namespace Jounce.Tests.Core
{
    /// <summary>
    /// Tests for the <see cref="BaseNotify"/> class
    /// </summary>
    [TestClass]
    public class BaseNotifyTests
    {
        private BaseNotify _target;
        private const string PROPERTY_NAME = "TestProperty";
        private const string PROPERTY2_NAME = "SecondTestProperty";

       //todo 
        private static readonly string[] _properties = new[] { PROPERTY_NAME, PROPERTY2_NAME };

        private BaseNotifyTestHelper Target
        {
           get {return (BaseNotifyTestHelper) _target;}
        }

       /// <sumary>
       /// Initialize targets
       /// </sumary>
       [TestInitialize]
       public void Initialize()
        {
            _target = new BaseNotifyTestHelper();
        }

        [TestMethod]
        public void GivenInitialStateWhenRaisePropertyChangeCalledThenEventShouldFire()
       {
           var fired = false;
           _target.PropertyChanged += (o, e) =>
           {
               fired = true;
               Assert.AreEqual(PROPERTY_NAME, e.PropertyName,
                   "RaisePropertyChanged did not fire Property change event with correct property name");
               //TODO : Call if test complete
           };

           Target.RaisePropertyChanged(PROPERTY_NAME);
           Assert.IsTrue(fired, "RaisePropertyChange did not fire property change event");
       }
 


    }
}
