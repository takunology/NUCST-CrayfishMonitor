using System;

namespace CrayfishMonitor_Desktop.Models
{
    public class MeasurementData : IMeasurementData
    {
        public DateTime Time { get; set; }
        public double Elapsed { get; set; }
        public double Voltage { get; set; }
    }
}
