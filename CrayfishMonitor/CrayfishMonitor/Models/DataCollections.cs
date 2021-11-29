using Reactive.Bindings;

namespace CrayfishMonitor.Models
{
    public static class DataCollections
    {
        public static ReactiveCollection<ArduinoData> ArduinoDatas { get; set; } = new ReactiveCollection<ArduinoData>();
        public static ReactiveCollection<FFTData> FFTDatas { get; set; } = new ReactiveCollection<FFTData>();
        public static ReactiveCollection<DiffData> DiffDatas { get; set; } = new ReactiveCollection<DiffData>();
    }
}
