using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Newtonsoft.Json;

namespace Monitor.Store.Data
{
    public sealed class MonitorDataSource
    {
        private static MonitorDataSource _monitorDataSource = new MonitorDataSource();

        private static ObservableCollection<MonitoredCategory> _monitoredCategories =
            new ObservableCollection<MonitoredCategory>();

        public static async Task<IEnumerable<MonitoredCategory>> GetGroupsAsync()
        {
            await _monitorDataSource.LoadSampleData();

            return _monitorDataSource.MonitoredCategories;
        }
        async Task LoadSampleData()
        {
            Uri dataUri = new Uri("ms-appx:///DataModel/SampleMonitoredEventsData.json");
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
            string jsonText = await FileIO.ReadTextAsync(file);
            var deserialized = JsonConvert.DeserializeObject<MonitorDataSource>(jsonText);
            _monitoredCategories = deserialized.MonitoredCategories;
        }
        public static void AddMonitoredCategory(MonitoredCategory category)
        {
            _monitoredCategories.Add(category);
        }

        public ObservableCollection<MonitoredCategory> MonitoredCategories {
            get { return _monitoredCategories; }
        }
    }

    public class MonitoredEvent
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public int Count { get; set; }
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

        public ObservableCollection<MonitoredEvent> MonitoredEvents { get; set; }


    }
}
