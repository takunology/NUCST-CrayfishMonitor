using Reactive.Bindings;
using System.Collections.Generic;

namespace CrayfishMonitor_Desktop.Models
{
    public static class DataCollections
    {
        public static ReactiveCollection<MeasurementData> Measurements { get; set; } = new ReactiveCollection<MeasurementData>();
        public static List<MeasurementSaveData> SaveDataList = new List<MeasurementSaveData>();
    }
}
