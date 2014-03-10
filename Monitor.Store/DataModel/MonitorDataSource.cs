using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Monitor.Store.Common;
using Newtonsoft.Json;
using Windows.UI.Xaml;
using System.ComponentModel;

namespace Monitor.Store.Data
{
    public sealed class MonitorDataSource
    {
        private static MonitorDataSource _monitorDataSource = new MonitorDataSource();

        private ObservableCollection<MonitoredCategory> _monitoredCategories = new ObservableCollection<MonitoredCategory>();
        private ObservableCollection<MonitoredEvent> _monitoredEvents = new ObservableCollection<MonitoredEvent>();

        public ObservableCollection<MonitoredEvent> MonitoredEvents
        {
            get { return this._monitoredEvents; }
        }
        public ObservableCollection<MonitoredCategory> MonitoredCategories
        {
            get { return this._monitoredCategories; }
        }
        public static async Task AddBroadcastedCategory(Broadcast broadcast)
        {
            var monitoredEvent = new MonitoredEvent
            {
                Id = broadcast.Message.Id,
                Title = broadcast.Message.Title,
                Content = broadcast.Message.Content,
                DateTimeUtc = broadcast.Message.DateTimeUtc,
                EventCategory = broadcast.Message.EventCategory
            };
            _monitorDataSource._monitoredEvents.Add(monitoredEvent);

            var mc = _monitorDataSource._monitoredCategories.SingleOrDefault(x => x.EventCategory.Name == broadcast.Message.EventCategory.Name);
            if (mc == null)
            {
                mc = new MonitoredCategory();
                mc.EventCategory = broadcast.Message.EventCategory;
                mc.Id = Guid.NewGuid();
                mc.MonitoredCategoryEvents = new ObservableCollection<MonitoredCategoryEvent>();
                _monitorDataSource._monitoredCategories.Add(mc);
            }
            var me = mc.MonitoredCategoryEvents.SingleOrDefault(x => x.Title == broadcast.Message.Title);
            if (me == null)
            {
                me = new MonitoredCategoryEvent { Title = broadcast.Message.Title, Id = Guid.NewGuid(), Count = 1 };
                mc.MonitoredCategoryEvents.Add(me);
            }
            else
            {
                me.Count += 1;
            }
        }
        public static async Task<IEnumerable<MonitoredCategory>> GetMonitoredCategoriesAsync()
        {

            return _monitorDataSource._monitoredCategories;
        }
        public static ObservableCollection<MonitoredCategory> GetMonitoredEventsByCategoryId()
        {
            return _monitorDataSource._monitoredCategories;
        }
        public static ObservableCollection<MonitoredCategory> GetMonitoredCategories()
        {
            return _monitorDataSource._monitoredCategories;
        }

        public static IEnumerable<MonitoredEvent> GetMonitoredEventsByEventCategoryId(Guid eventCategoryId)
        {
            return
                new List<MonitoredEvent>(
                    _monitorDataSource._monitoredEvents.Where(x => x.EventCategory.Id == eventCategoryId));
        }
           
    }

    public class MonitoredCategoryEvent:INotifyPropertyChanged
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        private int _count;
        public int Count {
            get{return _count;}
            set
            {
                if (_count != value)
                {
                    _count = value;
                    OnPropertyChanged("Count");

                }
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class EventCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class MonitoredCategory
    {
        public Guid Id { get; set; }
        public EventCategory EventCategory { get; set; }

        public ObservableCollection<MonitoredCategoryEvent> MonitoredCategoryEvents { get; set; }


    }

    public class MonitoredEvent
    {
        public Guid Id { get; set; }
        public EventCategory EventCategory { get; set; }
        public DateTime DateTimeUtc { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
    }
}
