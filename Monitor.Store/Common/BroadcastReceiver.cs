using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.AspNet.SignalR.Client;

namespace Monitor.Store.Common
{
    /// <summary>
    /// Receives broadcasts from a Signalr hub
    /// </summary>
    public class BroadcastReceiver
    {
        private static BroadcastReceiver _instance;
       
        public static BroadcastReceiver GetBroadcastReceiver()
        {
            if (_instance == null)
            {
                _instance = new BroadcastReceiver();
                _instance.Initialize();
            }
            return _instance;
        }
        private BroadcastReceiver()
        {
     
        }



        public event BoadcastReceivedHandler BroadcastReceived;
        public delegate void BoadcastReceivedHandler(object sender, BroadcastReceivedEventArgs e);
        /// <summary>
        /// Create, connect to hub, and assign callback to handle broadcasts
        /// </summary>
        /// <returns></returns>
        public async Task  Initialize()
        {
            var dataUri = new Uri("ms-appx:///Config/ServerConfig.txt");
            var file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
            var url = await FileIO.ReadTextAsync(file);
            var hubConnection = new HubConnection(url.Trim());
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
