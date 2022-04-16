using System;

namespace CrayfishMonitor_Desktop.Models
{
    public interface IMeasurementData
    {
        public DateTime Time { get; set; }
        public long Elapsed { get; set; }
        public double Voltage { get; set; }
    }
}
