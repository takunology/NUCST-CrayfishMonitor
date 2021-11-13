using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reactive.Bindings;
using CrayfishMonitor.Models;
using System.IO.Ports;

namespace CrayfishMonitor.ViewModels
{
    public class ArduinoSettingsViewModel
    {
        public int BaudRate { get; private set; } = ArduinoSettings.BaudRate;
        public Parity Parity { get; private set; } = ArduinoSettings.Parity;
        public StopBits StopBits { get; private set; } = ArduinoSettings.StopBits;
        public Encoding Encoding { get; private set; } = ArduinoSettings.Encoding;
        public bool DtrEnable { get; private set; } = ArduinoSettings.DtrEnable;

        public ArduinoSettingsViewModel()
        {

        }

    }
}
