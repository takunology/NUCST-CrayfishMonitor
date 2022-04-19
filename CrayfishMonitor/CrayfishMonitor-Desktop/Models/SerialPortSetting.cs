using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

namespace CrayfishMonitor_Desktop.Models
{
    public static class SerialPortSetting
    {
        public static string PortName { get; set; }
        public static int BaudRate { get; set; }
        public static int DataBits { get; set; }
        public static Parity Parity { get; set; }
        public static StopBits StopBits { get; set; }
        public static int WriteTimeOut { get; set; }
        public static int ReadTimeOut { get; set; }
        public static Encoding Encoding { get; set; } = Encoding.UTF8;
        public static bool DtrEnable { get; set; }

        public readonly static List<int> BaudRateList = new List<int>() { 300, 1200, 2400, 9600, 14400, 19200, 28800, 38400, 57600, 115200 };
        public readonly static List<Parity> ParityList = new List<Parity>() { Parity.None, Parity.Odd, Parity.Even, Parity.Mark, Parity.Space };
        public readonly static List<StopBits> StopBitsList = new List<StopBits>() { StopBits.None, StopBits.One, StopBits.Two, StopBits.OnePointFive };
        public readonly static List<Encoding> EncodingList = new List<Encoding>() { Encoding.UTF8, Encoding.Unicode, Encoding.UTF32, Encoding.ASCII };
    }
}
