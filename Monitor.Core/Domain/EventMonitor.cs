using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Monitor.Core.Domain
{
    public class EventMonitor:IEventMonitor
    {
        public Guid Id { get; set; }

        private IList<IMonitoredEvent> _monitoredEvents;
        public IList<IMonitoredEvent> MonitoredEvents
        {
            get { return _monitoredEvents ?? (_monitoredEvents = new List<IMonitoredEvent>()); }
            set
            {
                _monitoredEvents = value;
            }
        }
    }
}