using System.ComponentModel;
using Jounce.Core.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jounce.Test.Core.Model
{
    [TestClass]
    public class BaseNotifyFixture
    {
        [TestMethod]
        public void RaisePropertyChanged_Does_Not_Throw_If_No_Subscriber_For_PropertyChanged_Event()
        {
            DummyBaseNotify dbn = new DummyBaseNotify { MyProperty = "Test" };
        }

        [TestMethod]
        public void RaisePropertyChanged_Infers_Property_Name_From_Stack_Trace()
        {
            bool propertyChangedFired = false;
            DummyBaseNotify dbn = new DummyBaseNotify();
            PropertyChangedEventHandler handler = null;
            handler = (s, e) =>
                          {
                              dbn.PropertyChanged -= handler;
                              Assert.AreEqual("MyProperty", e.PropertyName);
                              propertyChangedFired = true;
                          };
            dbn.PropertyChanged += handler;
            dbn.MyProperty = "Test";

            Assert.IsTrue(propertyChangedFired);
        }

        [TestMethod]
        public void RaisePropertyChanged_With_Array_Fires_Multiple_PropertyChange_Events()
        {
            int propertyChangedFireCount = 0;
            DummyBaseNotify dbn = new DummyBaseNotify();
            PropertyChangedEventHandler handler = null;
            handler = (s, e) =>
                          {
                              if (propertyChangedFireCount++ >= 1)
                              {
                                  dbn.PropertyChanged -= handler;
                              }
                          };
            dbn.PropertyChanged += handler;
            dbn.NotifyMultipleProperty = "Test";
            Assert.AreEqual(2, propertyChangedFireCount);
        }

        [TestMethod]
        public void RaisePropertyChanged_With_Lambda_Fires_PropertyChange_Event()
        {
            bool propertyChangedFired = false;
            DummyBaseNotify dbn = new DummyBaseNotify();
            PropertyChangedEventHandler handler = null;
            handler = (s, e) =>
            {
                dbn.PropertyChanged -= handler;
                Assert.AreEqual("MyOtherProperty", e.PropertyName);
                propertyChangedFired = true;
            };
            dbn.PropertyChanged += handler;
            dbn.MyOtherProperty= "Test";
            Assert.IsTrue(propertyChangedFired);
        }

        public class DummyBaseNotify : BaseNotify
        {
            private string _myProperty;
            private string _myOtherProperty;
            private string _notifyMultipleProperty;

            public string MyProperty
            {
                get { return _myProperty; }
                set
                {
                    _myProperty = value;
                    RaisePropertyChanged("MyProperty");
                }
            }

            public string MyOtherProperty
            {
                get { return _myOtherProperty; }
                set
                {
                    _myOtherProperty = value;
                    RaisePropertyChanged(() => MyOtherProperty);
                }
            }

            public string NotifyMultipleProperty
            {
                get { return _notifyMultipleProperty; }
                set
                {
                    _notifyMultipleProperty = value;
                    RaisePropertyChanged(new[] { "NotifyMultipleProperty", "MyOtherProperty" });
                }
            }
        }
    }
}