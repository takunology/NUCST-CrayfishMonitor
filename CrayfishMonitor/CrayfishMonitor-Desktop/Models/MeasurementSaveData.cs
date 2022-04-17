using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
