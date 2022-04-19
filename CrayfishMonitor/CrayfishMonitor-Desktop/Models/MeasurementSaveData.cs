using System.Collections.Generic;

namespace CrayfishMonitor_Desktop.Models
{
    public class MeasurementSaveData
    {
        public string Date { get; set; } = "";
        public string Time { get; set; } = "";
        public string DeviceName { get; set; } = "";
        public List<MeasurementData> Measurements { get; set; } = new List<MeasurementData>();
        public string Title { get; set; } = "";
    }
}
