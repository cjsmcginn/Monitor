using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;


namespace Monitor.Core.Domain
{
   [DataContract]
    public class MonitoredEventRequest:IMonitoredEventRequest
    {
        [DataMember]
        public Guid EventMonitorId { get; set; }
        [DataMember]
        public IMonitoredEvent MonitoredEvent { get; set; }
    }
}