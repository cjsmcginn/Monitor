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
        private static readonly MonitorDataSource _monitorDataSource = new MonitorDataSource();
        private readonly ObservableCollection<MonitoredCategory> _monitoredCategories = new ObservableCollection<MonitoredCategory>();
        private readonly ObservableCollection<MonitoredEvent> _monitoredEvents = new ObservableCollection<MonitoredEvent>();
        /// <summary>
        /// Used primarily by designer to bind during design time, designer will look for a instance method
        /// </summary>
        public ObservableCollection<MonitoredEvent> MonitoredEvents
        {
            get { return this._monitoredEvents; }
        }
        /// <summary>
        /// Used primarily by designer to bind during design time, designer will look for a instance method
        /// </summary>
        public ObservableCollection<MonitoredCategory> MonitoredCategories
        {
            get { return this._monitoredCategories; }
        }
        /// <summary>
        /// This method maps broadcasts to items of interest. These "interesting items" are the core of the applications data model.
        /// This method will take the broadcast and map it into MonitoredCategories, and MonitoredEvents
        /// Categories will simply keep track of the count of items grouped by category, where as the monitored event
        /// will serve as the whole of the broadcast, (i.e the Item in our application)
        /// </summary>
        /// <param name="broadcast"></param>
        public static void AddBroadcastedCategory(Broadcast broadcast)
        {
            var monitoredEvent = new MonitoredEvent
            {
                Id = broadcast.Message.Id,
                Title = broadcast.Message.Title,
                Content = broadcast.Message.Content,
                DateTimeUtc = broadcast.Message.DateTimeUtc,
                EventCategory = broadcast.Message.EventCategory
            };
            //We always add the monitored event to the collection this will be used in our "Item" view
            _monitorDataSource._monitoredEvents.Add(monitoredEvent);
            /*Because our "Group" object, the MonitoredCategory is only concerned with showing the count of events by category id
             we first see if it exists in the collection, if not, we add it, afterwards it exists, and the count is incremented
             * this is the displayed number in our chart
             */
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

        public static ObservableCollection<MonitoredCategory> GetMonitoredCategories()
        {
            return _monitorDataSource._monitoredCategories;
        }
        /// <summary>
        /// Collection already exists as monitored events are added as they are broadcasted.
        /// This method filters them by the event category id passed in
        /// </summary>
        /// <param name="eventCategoryId">The event category id of the category of events to filter</param>
        /// <returns></returns>
        public static IEnumerable<MonitoredEvent> GetMonitoredEventsByEventCategoryId(Guid eventCategoryId)
        {
            return
                new List<MonitoredEvent>(
                    _monitorDataSource._monitoredEvents.Where(x => x.EventCategory.Id == eventCategoryId));
        }
           
    }

    #region Data Models
    /// <summary>
    /// Represents an event that is part of a category, used to drive group page display.
    /// </summary>
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
    /// <summary>
    /// A common Id, Name structure used to logically group events
    /// </summary>
    public class EventCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
    /// <summary>
    /// A monitored category is a "Group" structure, it represents the unique categories our datasource receives
    /// </summary>
    public class MonitoredCategory
    {
        public Guid Id { get; set; }
        public EventCategory EventCategory { get; set; }

        public ObservableCollection<MonitoredCategoryEvent> MonitoredCategoryEvents { get; set; }


    }
    /// <summary>
    /// Monitored event is the "Detail" item, the application ultimately monitors these events.
    /// These events have categories, and the initial view groups them into unique categories
    /// where the detail "Items" view will display the monitored event object instance
    /// </summary>
    public class MonitoredEvent
    {
        public Guid Id { get; set; }
        public EventCategory EventCategory { get; set; }
        public DateTime DateTimeUtc { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
    }
    #endregion
}
