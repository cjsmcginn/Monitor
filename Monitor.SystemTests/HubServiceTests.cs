using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Monitor.Core.Domain;
using Monitor.Hub;

namespace Monitor.SystemTests
{
    /// <summary>
    /// These tests are meant to test the HubService (not a running instance of the hub)
    /// Get things working in the service, make sure the WCF calls only methods on your service
    /// and everything should work the way the tests 
    /// </summary>
    [TestClass]
    public class HubServiceTests
    {
        IHubService GetTarget()
        {
            return new HubService();
        }
        [TestMethod]
        public void GetMonitorTest()
        {
            var target = GetTarget();
            var actual = target.GetMonitor();
            Assert.IsFalse(actual == Guid.Empty,"Expected non empty Guid.");
        }

        [TestMethod]
        public void PostMonitoredEventTest()
        {
            var target = GetTarget();
            var request = new MonitoredEventRequest();
            var actual = target.PostMonitoredEvent(request);
            Assert.IsInstanceOfType(actual, typeof (IMonitoredEventResponse),
                "Expected return type of IMonitoredEventRequest");
        }
    }
}
