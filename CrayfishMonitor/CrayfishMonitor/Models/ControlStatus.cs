using Reactive.Bindings;

namespace CrayfishMonitor.Models
{
    public static class ControlStatus
    {
        public static ReactivePropertySlim<bool> IsEnableArduinoSettingsControl = new ReactivePropertySlim<bool>(false);
        public static ReactivePropertySlim<int> IsGetCSVDatas = new ReactivePropertySlim<int>(0);
    }
}
