﻿using System;
using System.Threading;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Monitor.Core.Domain;

namespace Monitor.SystemTests
{
    [TestClass]
    public class BroadcastTests
    {

        [TestMethod]
        public async void BroadcastReceiveTest()
        {
            var url = System.Configuration.ConfigurationManager.AppSettings["signalRUrl"];

            var hubConnection = new HubConnection(url);
            var hubProxy = hubConnection.CreateHubProxy("MessageHub");
            bool received = false;
            hubProxy.On<string, string>("broadcastMessage", (name, message) =>
            {
                received = true;
            });
   
            var client = new Hub.ServiceClient();
            var request = new Hub.PostMonitoredEvent();
            var requestType = new MonitoredEventRequest();
            var guid = client.GetMonitor();
            if (guid != null)
                requestType.EventMonitorId = guid.Value;
            requestType.MonitoredEvent = new MonitoredEvent
            {
                EventCategory = new EventCategory {Id = Guid.NewGuid(), Name = "Test Category"},
                Title = "Test Broadcast",
                Id = Guid.NewGuid()
            };
            request.request = requestType;
            hubConnection.Start().Wait();

            var response = client.PostMonitoredEvent(request);
            Assert.IsTrue(received);
        }

    }
}
