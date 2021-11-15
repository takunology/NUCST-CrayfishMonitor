using System;
using System.Collections.Generic;
using System.Text;
using Reactive.Bindings;
using CrayfishMonitor.Models;
using System.IO.Ports;
using System.Windows;

namespace CrayfishMonitor.ViewModels
{
    public class ArduinoSettingsViewModel
    {
        public ReactiveCommand ApplyCommand { get; private set; } = new ReactiveCommand();
        public ReactivePropertySlim<bool> IsEnableApplyButton { get; set; } = new ReactivePropertySlim<bool>(Flags.ApplySettingsFlag.Value);

        // 設定用バインディング
        public List<int> BaudRate { get; set; } = new List<int>(ArduinoSettings.BaudRateList);
        public int DataBits { get; set; } = ArduinoSettings.DataBits;
        public List<string> Parity { get; set; } = new List<string>();
        public List<string> StopBits { get; set; } = new List<string>();
        public List<string> Encoding { get; set; } = new List<string>();
        public List<string> DtrEnable { get; set; } = new List<string>();

        // 説明用バインディング
        public string BaudRate_Disctiption { get; private set; } = ArduinoSettings.BaudRate_Discription;
        public string DataBits_Disctiption { get; private set; } = ArduinoSettings.DataBits_Discription;
        public string Parity_Disctiption { get; private set; } = ArduinoSettings.Parity_Discription;
        public string StopBits_Disctiption { get; private set; } = ArduinoSettings.StopBits_Discription;
        public string Encoding_Disctiption { get; private set; } = ArduinoSettings.Encoding_Discription;
        public string DtrEnable_Disctiption { get; private set; } = ArduinoSettings.DtrEnable_Discription;

        // 選択用プロパティ
        public int SelectedBaudRate { get; set; } = ArduinoSettings.BaudRate;
        public int SelectedDataBits { get; set;} = ArduinoSettings.DataBits;
        public string SelectedParity { get; set; } = ArduinoSettings.Parity.ToString();
        public string SelectedStopBits { get; set; } = ArduinoSettings.StopBits.ToString();
        public string SelectedEncoding { get; set;} = ArduinoSettings.Encoding.ToString();
        public string SelectedDtrEnable { get; set; } = ArduinoSettings.DtrEnable == true ? "有効" : "無効";

        //設定値反映用バインディング
        public string DisplayEncoding { get; set; }

        public ArduinoSettingsViewModel()
        {
            SettingsInit();
            ApplyCommand.Subscribe(_ => ApplySettings());
        }

        private void ApplySettings()
        {
            try
            {
                ArduinoSettings.BaudRate = SelectedBaudRate;
                ArduinoSettings.DataBits = SelectedDataBits;
                ArduinoSettings.Parity = SelectedParity.ToParity();
                ArduinoSettings.StopBits = SelectedStopBits.ToStopBits();
                ArduinoSettings.Encoding = SelectedEncoding.ToEncoding();
                ArduinoSettings.DtrEnable = SelectedDtrEnable.ToDtrEnable();

                MessageBox.Show("設定を適用しました。再接続してください。", "設定の適用", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("設定を適用できませんでした。", "設定エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void SettingsInit()
        {
            foreach (var item in ArduinoSettings.ParityList) Parity.Add(item.ToString());
            foreach (var item in ArduinoSettings.StopBitsList) StopBits.Add(item.ToString());
            foreach (var item in ArduinoSettings.EncodingList) Encoding.Add(item.EncodingName);
            DtrEnable.Add("有効"); DtrEnable.Add("無効");
        }
    }

    // 拡張メソッド
    public static class _
    {
        public static Parity ToParity(this string str)
        {
            if (str == Parity.None.ToString()) return Parity.None;
            else if (str == Parity.Odd.ToString()) return Parity.Odd;
            else if (str == Parity.Even.ToString()) return Parity.Even;
            else if (str == Parity.Mark.ToString()) return Parity.Mark;
            else return Parity.Space;
        }

        public static StopBits ToStopBits(this string str)
        {
            if (str == StopBits.None.ToString()) return StopBits.None;
            else if (str == StopBits.One.ToString()) return StopBits.One;
            else if (str == StopBits.Two.ToString()) return StopBits.Two;
            else return StopBits.OnePointFive;
        }

        public static Encoding ToEncoding(this string str)
        {
            if (str == Encoding.UTF8.EncodingName) return Encoding.UTF8;
            else if (str == Encoding.Unicode.EncodingName) return Encoding.Unicode;
            else if (str == Encoding.UTF32.EncodingName) return Encoding.UTF32;
            else if (str == Encoding.ASCII.EncodingName) return Encoding.ASCII;
            else return Encoding.Default;
        }

        public static bool ToDtrEnable(this string str)
        {
            return str == "有効" ? true : false;
        }
    }
}
