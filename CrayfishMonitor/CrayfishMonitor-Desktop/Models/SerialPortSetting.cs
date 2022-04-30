using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

namespace CrayfishMonitor_Desktop.Models
{
    public static class SerialPortSetting
    {
        public static int BaudRate { get; set; } = Properties.ArduinoSettings.Default.BaudRate;
        public static int DataBits { get; set; } = Properties.ArduinoSettings.Default.DataBits;
        public static Parity Parity { get; set; } = Properties.ArduinoSettings.Default.Parity.ToParity();
        public static StopBits StopBits { get; set; } = Properties.ArduinoSettings.Default.StopBits.ToStopBits();
        public static int WriteTimeOut { get; set; } = Properties.ArduinoSettings.Default.WriteTimeOut;
        public static int ReadTimeOut { get; set; } = Properties.ArduinoSettings.Default.ReadTimeOut;
        public static Encoding Encoding { get; set; } = Properties.ArduinoSettings.Default.Encoding.ToEncoding();
        public static bool DtrEnable { get; set; } = Properties.ArduinoSettings.Default.DtrEnable;

        public readonly static List<int> BaudRateList = new List<int>() { 300, 1200, 2400, 9600, 14400, 19200, 28800, 38400, 57600, 115200 };
        public readonly static List<int> DataBitList = new List<int>() { 4, 5, 6, 7, 8 };
        public readonly static List<string> ParityList = new List<string>() { "None", "Odd", "Even", "Mark", "Space" };
        public readonly static List<string> StopBitsList = new List<string>() { "None", "One", "Two", "OnePointFive" };
        public readonly static List<string> EncodingList = new List<string>() { "UTF-8", "Unicode", "UTF-32", "ASCII" };

        public static Parity ToParity(this string parity)
        {
            return parity switch
            {
                "Odd" => Parity.Odd,
                "Even" => Parity.Even,
                "Mark" => Parity.Mark,
                "Space" => Parity.Space,
                _ => Parity.None
            };
        }

        public static StopBits ToStopBits(this string stopBit)
        {
            return stopBit switch
            {
                "One" => StopBits.One,
                "Two" => StopBits.Two,
                "OnePointFive" => StopBits.OnePointFive,
                _ => StopBits.None
            };
        }

        public static Encoding ToEncoding(this string encoding)
        {
            return encoding switch
            {
                "Unicode" => Encoding.Unicode,
                "UTF32" => Encoding.UTF32,
                "ASCII" => Encoding.ASCII,
                _ => Encoding.UTF8
            };
        }
    }
}
