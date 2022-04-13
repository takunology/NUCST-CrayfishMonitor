using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrayfishMonitor_Desktop.Models
{
    public static class DataCollections
    {
        public static ReactiveCollection<MeasurementData> Measurements { get; set; } = new ReactiveCollection<MeasurementData>();
    }
}
