using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monitor.Store.Data;

namespace Monitor.Store.ViewModels
{
    public class SplitPageViewModel
    {
        public ObservableCollection<MonitoredEvent> MonitoredEvents { get; set; }
        public SplitPageViewModel()
        {
            MonitoredEvents = new ObservableCollection<MonitoredEvent>();
        }
        /// <see cref="MonitorDataSource.GetMonitoredEventsByEventCategoryId"/>
        /// <param name="eventCategoryId"></param>
        public void LoadViewData(Guid eventCategoryId)
        {
            foreach (var item in MonitorDataSource.GetMonitoredEventsByEventCategoryId(eventCategoryId))
            {
                MonitoredEvents.Add(item);
            }
        }
    }
}
