using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;


namespace Monitor.Core.Domain
{
    [DataContract]
    public class MonitoredEventResponse:IMonitoredEventResponse
    {
        [DataMember]
        public bool Success { get; set; }
    }
}