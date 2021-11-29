using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrayfishMonitor.Models
{
    public class ArduinoData
    {
        public string? Date { get; set; }
        public string? Time { get; set; }
        public long Elapsed { get; set; }
        public double Voltage { get; set; }
    }
}
