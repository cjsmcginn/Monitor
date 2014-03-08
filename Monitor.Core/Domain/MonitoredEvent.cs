using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace Monitor.Core.Domain
{
    [DataContract]
    public class MonitoredEvent : IMonitoredEvent
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public EventCategory EventCategory { get; set; }
    }
}
