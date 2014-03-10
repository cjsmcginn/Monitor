using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Monitor.Core.Domain;

namespace Monitor.ConsoleRunner
{
    /// <summary>
    /// Posts events while console is running to simulate system that broadcasts messages
    /// </summary>
    public class EventPoster
    {
        

        public static void PostWebsiteErrors()
        {
            Timer t = new Timer();
            var counter = 0;
            var data =
                Newtonsoft.Json.JsonConvert.DeserializeObject<List<MonitoredEvent>>(SerializedEvents.WebsiteErrors);
            var service = new Hub.ServiceClient();
            var monitorId = service.GetMonitor().Value;

            t.AutoReset = true;
            t.Interval = 3000;
            t.Elapsed += (sender, e) =>
            {
                if (counter >= data.Count())
                    counter = 0;
                var monitoredEvent = data.ElementAt(counter);
                var request = new Hub.PostMonitoredEvent();
                var requestType  = new MonitoredEventRequest();
                requestType.EventMonitorId = monitorId;
                monitoredEvent.Id = Guid.NewGuid();
                requestType.MonitoredEvent = monitoredEvent;
                request.request = requestType;
                service.PostMonitoredEvent(request);
                counter += 1;
                Console.Out.WriteLine("Posting Website Errors");

            };
            t.Start();
        }

        public static void PostPayments()
        {
            Timer t = new Timer();
            var counter = 0;
            var data =
                Newtonsoft.Json.JsonConvert.DeserializeObject<List<MonitoredEvent>>(SerializedEvents.Payments);
            var service = new Hub.ServiceClient();
            var monitorId = service.GetMonitor().Value;

            t.AutoReset = true;
            t.Interval = 8000;
            t.Elapsed += (sender, e) =>
            {
                if (counter >= data.Count())
                    counter = 0;
                var monitoredEvent = data.ElementAt(counter);
                var request = new Hub.PostMonitoredEvent();
                var requestType = new MonitoredEventRequest();
                requestType.EventMonitorId = monitorId;
                monitoredEvent.Id = Guid.NewGuid();
                requestType.MonitoredEvent = monitoredEvent;
                request.request = requestType;
                service.PostMonitoredEvent(request);
                counter += 1;
                Console.Out.WriteLine("Posting Payments");

            };
            t.Start();
        }
    }
}
