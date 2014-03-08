using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Monitor.Core.Domain;

namespace Monitor.Hub
{
    public class HubService:IHubService
    {
        public Guid GetMonitor()
        {
            var monitor = new EventMonitor {Id = Guid.NewGuid()};
            EventMonitors.ToList().Add(monitor);
            return monitor.Id;
        }

        public IMonitoredEventResponse PostMonitoredEvent(IMonitoredEventRequest request)
        {
            var response = new MonitoredEventResponse();

            var targetMonitor = EventMonitors.ToList().SingleOrDefault(x => x.Id == request.EventMonitorId);
            if (targetMonitor != null)
            {
                targetMonitor.MonitoredEvents.ToList().Add(request.MonitoredEvent);
                response.Success = true;
                Broadcaster.PushMessage(targetMonitor, request.MonitoredEvent.Id);
            }
           
            return response;
        }

        private IEnumerable<IEventMonitor> _eventMonitors;
        public IEnumerable<IEventMonitor> EventMonitors
        {
            get { return _eventMonitors ?? (_eventMonitors = new List<IEventMonitor>()); }
            set
            {
                _eventMonitors = value;
            }
        }
    }
}