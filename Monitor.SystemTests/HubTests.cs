using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Monitor.Core.Domain;

namespace Monitor.SystemTests
{
    /// <summary>
    /// This is more of a system test that calls methods on a service reference just as remote clients
    /// that wish to broacast messages would. The methods tested are the same as in the HubService
    /// because the the HubService is the underlying provider for the WCF service.
    /// </summary>
    /// <seealso cref="HubServiceTests"/>
    [TestClass]
    public class HubTests
    {
        [TestMethod]
        public void GetMonitorTest()
        {
            var client = new Hub.ServiceClient();
            var actual = client.GetMonitor();
            Assert.IsTrue(actual.HasValue, "Expected Guid.HasValue");
            Assert.IsFalse(actual.Value == Guid.Empty,"Expected non empty guid");
        }
        /// <summary>
        /// This exemplifies how your systems will broadcast messages in whole
        /// They need an instance of a monitor (which can be reused)
        /// They will make requests to PostMonitoredEvent
        /// </summary>
        [TestMethod]
        public void PostMonitoredEventTest()
        {
            var client = new Hub.ServiceClient();
            var request = new Hub.PostMonitoredEvent();
            var requestType = new MonitoredEventRequest();
            var guid = client.GetMonitor();
            if (guid != null)
                requestType.EventMonitorId = guid.Value;
            requestType.MonitoredEvent = new MonitoredEvent();
            request.request = requestType;
            requestType.MonitoredEvent.EventCategory = new EventCategory();

            var response = client.PostMonitoredEvent(request);
            Assert.IsInstanceOfType(response, typeof (IMonitoredEventResponse),
                "Expected type of IMonitoredEventResponse");

        }
    }
}
