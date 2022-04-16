using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrayfishMonitor_Desktop.Models
{
    public class MeasurementData : IMeasurementData
    {
        public DateTime Time { get; set; }
        public long Elapsed { get; set; }
        public double Voltage { get; set; }
    }
}
