using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reactive.Bindings;

namespace CrayfishMonitor.Models
{
    public static class Flags
    {
        public static ReactivePropertySlim<bool> ApplySettingsFlag { get; set; } = new ReactivePropertySlim<bool>(true);
    }
}
