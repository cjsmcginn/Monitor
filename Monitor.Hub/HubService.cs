using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Monitor.Core.Domain;

namespace Monitor.Hub
{
    public class HubService:IHubService
    {
        /// <summary>
        /// Each application that wishes to communicate with a monitoring instance
        /// calls this method to get an Id. In a non-demo application, this would likely be handled
        /// by a correlation id to support durability.
        /// </summary>
        /// <returns></returns>
        public Guid GetMonitor()
        {
            var monitor = new EventMonitor {Id = Guid.NewGuid()};
            EventMonitors.Add(monitor);
            return monitor.Id;
        }
        /// <summary>
        /// This is the meat and potatoes of the demo, clients posting events call this method
        /// the event eventually gets broadcast on the signalr hub via the Broadcaster
        /// </summary>
        /// <see cref="Broadcaster.PushMessage"/>
        /// <param name="request"></param>
        /// <returns></returns>
        public IMonitoredEventResponse PostMonitoredEvent(IMonitoredEventRequest request)
        {
            var response = new MonitoredEventResponse();

            var targetMonitor = EventMonitors.SingleOrDefault(x => x.Id == request.EventMonitorId);
            if (targetMonitor != null)
            {
                targetMonitor.MonitoredEvents.Add(request.MonitoredEvent);
                response.Success = true;
                Broadcaster.PushMessage(targetMonitor, request.MonitoredEvent.Id);
            }
           
            return response;
        }

        private List<IEventMonitor> _eventMonitors;
        public IList<IEventMonitor> EventMonitors
        {
            get { return _eventMonitors ?? (_eventMonitors = new List<IEventMonitor>()); }
            set { _eventMonitors = value.ToList(); }
        }
    }
}