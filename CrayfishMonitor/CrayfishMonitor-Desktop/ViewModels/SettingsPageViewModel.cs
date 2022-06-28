using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrayfishMonitor_Desktop.Models;
using System.IO.Ports;
using Windows.Storage;

namespace CrayfishMonitor_Desktop.ViewModels
{
    public class SettingsPageViewModel
    {
        public ReactiveCommand SaveCommand { get; set; } = new ReactiveCommand();

        public List<int> DataBitList { get; set; } = new List<int>(SerialPortSetting.DataBitList);
        public List<int> BaudRateList { get; set; } = new List<int>(SerialPortSetting.BaudRateList);
        public List<string> ParityList { get; set; } = new List<string>(SerialPortSetting.ParityList);
        public List<string> StopBitList { get; set; } = new List<string>(SerialPortSetting.StopBitsList);
        public List<string> EncodingList { get; set; } = new List<string>(SerialPortSetting.EncodingList);

        public ReactivePropertySlim<int> SelectedBaudRate { get; set; } = new ReactivePropertySlim<int>(SerialPortSetting.BaudRate);
        public ReactivePropertySlim<int> SelectedDataBits { get; set; } = new ReactivePropertySlim<int>(SerialPortSetting.DataBits);
        public ReactivePropertySlim<string> SelectedParity { get; set; } = new ReactivePropertySlim<string>(SerialPortSetting.Parity.ToString());
        public ReactivePropertySlim<string> SelectedStopBit { get; set; } = new ReactivePropertySlim<string>(SerialPortSetting.StopBits.ToString());
        public ReactivePropertySlim<string> SelectedEncoding { get; set; } = new ReactivePropertySlim<string>(EncodingName(SerialPortSetting.Encoding));
        public ReactivePropertySlim<bool> IsDtrEnable { get; set; } = new ReactivePropertySlim<bool>(SerialPortSetting.DtrEnable);

        public SettingsPageViewModel()
        {
            // ページ読み込み時にデフォルトの設定項目を読み込んでおく
            // Setting.cs を読みこまない
            SelectedDataBits.PropertyChanged += (s, e) => SaveDataBits();
        }

        private static string EncodingName(Encoding encoding)
        {
            return encoding.HeaderName switch
            {
                "utf-32" => "UTF-32",
                "utf-16" => "Unicode",
                "us-ascii" => "ASCII",
                _ => "UTF-8"
            };
        }

        private void SaveDataBits()
        {
            Properties.ArduinoSettings.Default.BaudRate = SelectedBaudRate.Value;
            Properties.ArduinoSettings.Default.Save();
        }
    }
}
