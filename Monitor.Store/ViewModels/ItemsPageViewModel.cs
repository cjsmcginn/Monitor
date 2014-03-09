using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Monitor.Store.Data;
using Newtonsoft.Json;
using Monitor.Store.Common;
using Windows.UI.Core;


namespace Monitor.Store.ViewModels
{
    public class ItemsPageViewModel
    {
        BroadcastReceiver _receiver;
        public ItemsPageViewModel()
        {
            _receiver = new BroadcastReceiver();
            MonitoredCategories = MonitorDataSource.GetMonitoredCategories();
            MonitoredCategories.CollectionChanged += MonitoredCategories_CollectionChanged;
            
        }

        public async Task Initialize()
        {
            await _receiver.Initialize();
            _receiver.BroadcastReceived += _receiver_BroadcastReceived;
        }

        void _receiver_BroadcastReceived(object sender, BroadcastReceivedEventArgs e)
        {
            
           Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                MonitorDataSource.AddBroadcastedCategory(e.Broadcast);
            });
        }

        public async Task LoadViewData()
        {
            var t = await MonitorDataSource.GetMonitoredCategoriesAsync();
   
        }

        void MonitoredCategories_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var x = "Y";
        }
        public ObservableCollection<MonitoredCategory> MonitoredCategories { get; set; }
        public CoreDispatcher Dispatcher { get; set; }
    }
}
