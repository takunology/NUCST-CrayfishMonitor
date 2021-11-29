using System.Collections.Generic;
using System.Text;
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

        //説明
        public readonly static string BaudRate_Discription = "シリアルポートの通信速度（ポーレート）を設定できます。デフォルトでは 9600[bps] です。";
        public readonly static string DataBits_Discription = "バイトごとのデータの長さを設定できます。";
        public readonly static string Parity_Discription = "通信の信頼性を確保（パリティチェック）するためのビットを設定します。";
        public readonly static string StopBits_Discription = "バイトごとの通信処理の終了を示すビットを設定できます。";
        public readonly static string Encoding_Discription = "データのエンコードを設定します。既定値では UTF-8 です。";
        public readonly static string DtrEnable_Discription = "シリアル通信中に、DTR (Data Terminal Ready) シグナルを有効にするかを設定できます。DTRは端末側の準備が完了しているときにデータを送ります。";

        // 設定値
        public readonly static List<int> BaudRateList = new List<int>() { 300, 1200, 2400, 4800, 9600, 14400, 19200, 28800, 38400, 57600, 115200, 230400, 250000 };
        public readonly static List<Parity> ParityList = new List<Parity> { Parity.None, Parity.Odd, Parity.Even, Parity.Mark, Parity.Space };
        public readonly static List<StopBits> StopBitsList = new List<StopBits> { StopBits.None, StopBits.One, StopBits.Two, StopBits.OnePointFive };
        public readonly static List<Encoding> EncodingList = new List<Encoding> { Encoding.UTF8, Encoding.Unicode, Encoding.UTF32, Encoding.ASCII };
    }
}
