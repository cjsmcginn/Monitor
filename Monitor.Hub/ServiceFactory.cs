using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Monitor.Hub
{
    /// <summary>
    /// This class allows us to have a single instance of the hub service. It mimics
    /// a durable wcf service that uses correlation. To keep the demo curt, we simplify the desired behavior
    /// as not to focus on a whole other topic.
    /// </summary>
    public class ServiceFactory
    {
        private static HubService _hubService;

        public static HubService GetHubService()
        {
            if (_hubService == null)
                _hubService = new HubService();
            return _hubService;
        }
    }
}