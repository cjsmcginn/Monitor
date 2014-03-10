﻿using System;
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
            
        }

        public async Task Initialize()
        {
            await _receiver.Initialize();
            _receiver.BroadcastReceived += _receiver_BroadcastReceived;
        }
        /// <summary>
        /// Calls datasource add to collection method on the UI thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _receiver_BroadcastReceived(object sender, BroadcastReceivedEventArgs e)
        {
            
           Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () => MonitorDataSource.AddBroadcastedCategory(e.Broadcast));
        }

        public ObservableCollection<MonitoredCategory> MonitoredCategories { get; set; }
        public CoreDispatcher Dispatcher { get; set; }
    }
}
