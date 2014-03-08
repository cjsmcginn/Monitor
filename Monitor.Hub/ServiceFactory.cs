using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Monitor.Hub
{
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