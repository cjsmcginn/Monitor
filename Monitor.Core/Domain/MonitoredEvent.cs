using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace Monitor.Core.Domain
{
    /// <summary>
    /// The heart of the application, these are the events that can be monitored
    /// they are categorized and are somewhat generic in structure as not to limit them to
    /// errors (like logging applications)
    /// </summary>
    [DataContract]
    public class MonitoredEvent : IMonitoredEvent
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public EventCategory EventCategory { get; set; }

        [DataMember]
        public string Content { get; set; }

        [DataMember]
        public DateTime DateTimeUtc { get; set; }
    }
}
