using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace Monitor.Store.Common
{
    public class BroadcastReceiver
    {
        public event BoadcastReceivedHandler BroadcastReceived;
        public delegate void BoadcastReceivedHandler(object sender, BroadcastReceivedEventArgs e);

        public async Task  Initialize()
        {
            var url = "http://localhost:2882";
            var hubConnection = new HubConnection(url);
            var hubProxy = hubConnection.CreateHubProxy("MessageHub");
            hubProxy.On<string, string>("broadcastMessage", (name, message) =>
            {
                if (BroadcastReceived == null)
                    return;
                var m = Newtonsoft.Json.JsonConvert.DeserializeObject<Message>(message);
                var b = new Broadcast { Message = m, Name = name };
                var args = new BroadcastReceivedEventArgs { Broadcast = b };
                BroadcastReceived(this, args);
   
            });
            await hubConnection.Start();
        }
    }
}
