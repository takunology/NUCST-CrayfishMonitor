using Reactive.Bindings;

namespace CrayfishMonitor.Models
{
    public class ArduinoData
    {
        public string? Date { get; set; } 
        public string? Time { get; set; } 
        public long Elapsed { get; set; } 
        public double Voltage { get; set; }
    }

    public static class ArduinoDataCollection
    {
        public static ReactiveCollection<ArduinoData> ArduinoDatas { get; set; } = new ReactiveCollection<ArduinoData>();
    }
}
