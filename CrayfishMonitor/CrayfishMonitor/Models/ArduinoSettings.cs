using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace CrayfishMonitor.Models
{
    // Arduino との接続設定
    public static class ArduinoSettings
    {
        public static string? PortName { get; set; }
        public static int BaudRate { get; set; } = 9600;
        public static int DataBits { get; set; } = 8;
        public static Parity Parity { get; set; } = Parity.None;
        public static StopBits StopBits { get; set; } = StopBits.One;
        public static Encoding Encoding { get; set; } = Encoding.UTF8;
        public static bool DtrEnable { get; set; } = true;
    }
}
