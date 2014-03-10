using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;


namespace Monitor.Core.Domain
{
    /// <summary>
    /// This is the request sent to the WCF service to publish an event
    /// </summary>
    /// <seealso cref="MonitoredEventResponse"/>
   [DataContract]
    public class MonitoredEventRequest:IMonitoredEventRequest
    {
        [DataMember]
        public Guid EventMonitorId { get; set; }
        [DataMember]
        public IMonitoredEvent MonitoredEvent { get; set; }
    }
}