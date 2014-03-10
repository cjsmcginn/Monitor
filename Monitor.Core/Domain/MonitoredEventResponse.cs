using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;


namespace Monitor.Core.Domain
{
    /// <summary>
    /// The response sent back to the requester when they request to publish a message.
    /// Typically this might have an errors collection, but this is a demo and we are not 
    /// really handling errors
    /// </summary>
    [DataContract]
    public class MonitoredEventResponse:IMonitoredEventResponse
    {
        [DataMember]
        public bool Success { get; set; }
    }
}