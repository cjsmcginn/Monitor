using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(Monitor.Hub.Startup))]

namespace Monitor.Hub
{
    /// <summary>
    /// The OWIN startup class.
    /// </summary>
    /// <remarks>
    /// In the web config, we  have an app setting value of 
    /// owin:AutomaticAppStartup set to false to prevent owin from colliding with our WCF service
    /// NOTICE the call to the UseCors helper. This is necessary to allow clients from other domains to 
    /// communicate with our SignalR hub.
    /// </remarks>
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
}
