using Monitor.Store.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Store.Common
{
    public class Message{
        public Guid Id{get;set;}
        public EventCategory EventCategory{get;set;}
        public string Title{get;set;}

        public DateTime DateTimeUtc { get; set; }

        public string Content { get; set; }
    }
    public class Broadcast
    {
        public string Name { get; set; }
        public Message Message { get; set; }
    }
    public class BroadcastReceivedEventArgs
    {
        public Broadcast Broadcast { get; set; }
    }
}
