using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using CrayfishMonitor.Models;
using Microsoft.Win32;
using Reactive.Bindings;

namespace CrayfishMonitor.ViewModels
{
    public class MainWindowViewModel
    {
        public ReactivePropertySlim<string> StatusText { get; private set; } = new ReactivePropertySlim<string>("切断中");
        public ReactivePropertySlim<string> StatusDialogText { get; private set; } = new ReactivePropertySlim<string>("準備完了");
        public ReactivePropertySlim<SolidColorBrush> StatusColor { get; private set; } = new ReactivePropertySlim<SolidColorBrush>(new SolidColorBrush(Colors.WhiteSmoke));
        public ReactiveCollection<string> SerialPortList { get; private set; } = new ReactiveCollection<string>();
        public ReactivePropertySlim<bool> IsEnabledSerialPortList { get; private set; } = new ReactivePropertySlim<bool>(true);
        public ReactivePropertySlim<string> SelectedPort { get; private set; } = new ReactivePropertySlim<string>();
        public ReactiveCommand ConnectCommand { get; private set; } = new ReactiveCommand();
        public ReactivePropertySlim<string> ConnectButtonText { get; private set; } = new ReactivePropertySlim<string>("接続");
        public ReactiveCommand DataSaveCommand { get; private set; } = new ReactiveCommand();
        public ReactiveCommand GraphSaveCommand { get; private set; } = new ReactiveCommand();
        public ReactiveCommand ResetCommand { get; private set; } = new ReactiveCommand();
        
        private SerialPort _serialPort = new SerialPort();
        private Stopwatch _stopWatch = new Stopwatch();
        private DispatcherTimer _Timer = new DispatcherTimer();

        public MainWindowViewModel()
        {
            PortInitialize();
            this.ConnectCommand.Subscribe(_ => Connection());
            this.DataSaveCommand.Subscribe(_ => SaveData());
            this.ResetCommand.Subscribe(_ => DataReset());
        }

        // 接続ポート取得 
        public void PortInitialize()
        {
            try
            {
                string[] ports = SerialPort.GetPortNames();
                foreach (string port in ports)
                {
                    this.SerialPortList.Add(port);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "COMポートを読み込めません", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        // 接続処理
        private void Connection()
        {

            if (this.ConnectButtonText.Value.Equals("接続"))
            {
                Connect();
            }
            else
            {
                DisConnect();
            }

        }

        // ポートの接続 -> 計測
        private void Connect()
        {
            ArduinoDataCollection.ArduinoDatas.Clear();

            _serialPort = new SerialPort
            {
                PortName = SelectedPort.Value,
                BaudRate = ArduinoSettings.BaudRate,
                DataBits = ArduinoSettings.DataBits,
                Parity = ArduinoSettings.Parity,
                StopBits = ArduinoSettings.StopBits,
                Encoding = ArduinoSettings.Encoding,
                DtrEnable = ArduinoSettings.DtrEnable
            };

            try
            {
                _serialPort.Open();
                this.ConnectButtonText.Value = "切断";
                this.StatusText.Value = this.SelectedPort.Value + " と接続中...";
                this.IsEnabledSerialPortList.Value = false;
                this.StatusDialogText.Value = "準備完了";
                this.StatusColor.Value = new SolidColorBrush(Colors.MistyRose);
                _stopWatch.Start();
                _serialPort.DataReceived += new SerialDataReceivedEventHandler(GetData);
            }
            catch
            {
                MessageBox.Show($"{SelectedPort.Value} に接続できませんでした。", "接続エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // 切断とデータの初期化
        private void DisConnect()
        {
            _stopWatch.Stop();
            _Timer.Stop();

            _serialPort.Close();
            _serialPort.Dispose();

            this.ConnectButtonText.Value = "接続";
            this.StatusText.Value = "切断中";
            this.StatusColor.Value = new SolidColorBrush(Colors.WhiteSmoke);
            this.IsEnabledSerialPortList.Value = true;
        }

        // 計測フェーズ -> Arduino からのデータ取得イベント -> DataGrid 書き込み
        private void GetData(object sender, SerialDataReceivedEventArgs e)
        {
            if (_serialPort.Equals(null)) return;
            if (_serialPort.IsOpen.Equals(null)) return;

            // データ取得
            ArduinoData arduinoData = new ArduinoData();
            DateTime dateTime = DateTime.Now;
            arduinoData.Date = dateTime.ToString("yyyy/MM/dd");
            arduinoData.Time = dateTime.ToString("HH:mm:ss:fff");
            arduinoData.Elapsed = _stopWatch.ElapsedMilliseconds;
            arduinoData.Voltage = double.Parse(_serialPort.ReadLine());
            // データの保存
            ArduinoDataCollection.ArduinoDatas.Add(arduinoData);
        }

        // データの保存
        private void SaveData()
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV形式(.csv)|*.csv|テキストファイル(.txt)|*.txt";
                saveFileDialog.Title = "計測データの保存";
                bool? result = saveFileDialog.ShowDialog();

                if (result == true)
                {
                    using (Stream stream = saveFileDialog.OpenFile())
                    using (StreamWriter streamWriter = new StreamWriter(stream, Encoding.GetEncoding("UTF-8")))
                    {
                        streamWriter.WriteLine("Date,Time,Elapsed[ms],Voltage[V]");
                        foreach (var data in ArduinoDataCollection.ArduinoDatas)
                        {
                            streamWriter.WriteLine($"{data.Date},{data.Time},{data.Elapsed},{data.Voltage}");
                        }
                    }
                }
                StatusDialogText.Value = "データを保存しました";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存に失敗しました。\n詳細:\n{ex}", "保存エラー");
            }
        }

        // データのリセット
        private void DataReset()
        {
            try
            {
                ArduinoDataCollection.ArduinoDatas.Clear();
                _stopWatch.Reset();
                StatusDialogText.Value = "リセット完了";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"詳細:\n{ex}", "リセットできませんでした。");
                StatusDialogText.Value = "リセット失敗";
            }
        }
    }
}
