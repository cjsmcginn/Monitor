using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Monitor.Core.Domain;
using Nancy;

namespace Monitor.Website.Modules
{
    public class BroadcastModule : NancyModule
    {
        private Guid _monitorId;
        private Dictionary<Guid, string> _categories;
        public BroadcastModule()
        {
            _categories = new Dictionary<Guid, string>();
            _categories.Add(Guid.Parse("3e4b4ec9-9e9d-4c18-a0a5-3aa8c2d5ef3b"), "Errors");
            _categories.Add(Guid.Parse("4af1e93e-7c1a-4f1d-ad5d-3dad1b06baf2"), "Payments");
            _categories.Add(Guid.Parse("7b84895a-919c-4591-a15b-eef906089946"), "Registrations");
            GetMonitor();
            Get["/"] = p =>
            {
                var fname = "index.html";
                return Response.AsFile(fname);
            };
            Post["/"] = p =>
            {
                var fname = "index.html";
                var category = this.Request.Form["category"];
                var title = this.Request.Form["title"];
                var content = this.Request.Form["content"];
                BroadcastMessage(category, title, content);
                return Response.AsFile(fname);
            };
        }

        void GetMonitor()
        {
            var client = new HubServiceReference.ServiceClient();
            var monitor = client.GetMonitor();
            if (monitor != null) _monitorId = monitor.Value;
        }

        Guid GetCategoryGuid(string category)
        {
            return _categories.Single(x => x.Value.Equals(category, StringComparison.InvariantCultureIgnoreCase)).Key;
        }
        void BroadcastMessage(string category, string title, string content)
        {
            var client = new HubServiceReference.ServiceClient();
            var request = new HubServiceReference.PostMonitoredEvent();
            var requestType = new MonitoredEventRequest();
            requestType.EventMonitorId = _monitorId;
            requestType.MonitoredEvent = new MonitoredEvent
            {
                EventCategory = new EventCategory { Id = GetCategoryGuid(category), Name = category },
                Title = title,
                Content = content,
                DateTimeUtc = System.DateTime.UtcNow,
                Id = Guid.NewGuid()
            };
            request.request = requestType;

            client.PostMonitoredEvent(request);
        }
    }
}