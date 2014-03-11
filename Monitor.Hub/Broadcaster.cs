using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using Monitor.Core.Domain;
using Newtonsoft.Json;

namespace Monitor.Hub
{
    /// <summary>
    /// Initializes the signalR hub on startup. Activated in Global.asax
    /// The hub needs publish on another port then the one running the WCF service.
    /// </summary>
    public class Broadcaster
    {
        private static IHubContext _Hub;
        /// <summary>
        /// Initializes the SignalR hub, this is self hosted. This Project will create two servers
        /// occupyig two distinct ports, one for applications that wish to broadcast messages (they will use the WCF service)
        /// and another (this self hosted instance) for applications that wish to receive broadcasts.
        /// </summary>
        public static void InitializeHub()
        {
            var url = System.Configuration.ConfigurationManager.AppSettings["signalRUrl"];
            WebApp.Start(url);
            _Hub = GlobalHost.ConnectionManager.GetHubContext<MessageHub>();
        }
        /// <summary>
        /// This method finds an instance of a monitored event, in the monitors collection.
        /// If it is found, a message is broadcast to all clients.
        /// </summary>
        /// <param name="monitor"></param>
        /// <param name="monitoredEventId"></param>
        public static void PushMessage(IEventMonitor monitor,Guid monitoredEventId)
        {
            var monitoredEvent = monitor.MonitoredEvents.SingleOrDefault(x => x.Id == monitoredEventId);
            if(monitoredEvent != null)
                _Hub.Clients.All.broadcastMessage(monitoredEvent.Title,JsonConvert.SerializeObject(monitoredEvent));
        }
    }
}