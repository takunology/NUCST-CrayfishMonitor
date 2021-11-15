using System;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
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
        public ReactiveCommand VersionCommand { get; private set; } = new ReactiveCommand();
        public ReactiveCommand AnalysisDataSaveCommand { get; private set; } = new ReactiveCommand();
        public ReactiveCommand AnalysisGraphSaveCommand { get; private set; } = new ReactiveCommand();

        private SerialPort _serialPort = new SerialPort();
        private Stopwatch _stopWatch = new Stopwatch();
        private DispatcherTimer _Timer = new DispatcherTimer();
        private MainWindow _mainWindow;

        public MainWindowViewModel()
        {
            BindingOperations.EnableCollectionSynchronization(SerialPortList, new object());

            GetMainWindow();
            PortInitialize();

            this.ConnectCommand.Subscribe(_ => Connection());
            this.DataSaveCommand.Subscribe(_ => SaveData());
            this.GraphSaveCommand.Subscribe(_ => SaveGraph());
            this.ResetCommand.Subscribe(_ => DataReset());
            this.VersionCommand.Subscribe(_ => ShowVersion());
            this.AnalysisDataSaveCommand.Subscribe(_ => SaveAnalysisData());
        }

        // 接続ポート取得 
        private void PortInitialize()
        {
            try
            {
                SerialPortList.Clear();
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
                this.StatusText.Value = "データ計測中...";
                this.IsEnabledSerialPortList.Value = false;
                this.StatusDialogText.Value = "準備完了";
                this.StatusColor.Value = new SolidColorBrush(Colors.MistyRose);
                Flags.ApplySettingsFlag.Value = false;
                _stopWatch.Start();
                _serialPort.DataReceived += new SerialDataReceivedEventHandler(GetData);
            }
            catch(Exception ex)
            {
                MessageBox.Show($"詳細:\n{ex}", "データ処理エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // 切断
        private void DisConnect()
        {
            try
            {
                _stopWatch.Stop();
                _Timer.Stop();

                _serialPort.Close();
                _serialPort.Dispose();

                this.ConnectButtonText.Value = "接続";
                this.StatusText.Value = "切断中";
                Flags.ApplySettingsFlag.Value = false;
                this.StatusColor.Value = new SolidColorBrush(Colors.WhiteSmoke);
                this.IsEnabledSerialPortList.Value = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"詳細:\n{ex}", "データ処理エラー。", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        // 計測フェーズ -> Arduino からのデータ取得イベント -> DataGrid 書き込み
        private void GetData(object sender, SerialDataReceivedEventArgs e)
        {
            try
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
            catch(Exception ex)
            {
                MessageBox.Show($"詳細:\n{ex}", "データ処理エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // データの保存
        private void SaveData()
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV形式(.csv)|*.csv|テキストファイル(.txt)|*.txt";
                saveFileDialog.Title = "計測データの保存";
                saveFileDialog.FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_Data";
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
                        StatusDialogText.Value = "データを保存しました";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存できませんでした。\n詳細:\n{ex}", "保存エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // グラフの保存
        private void SaveGraph()
        {
            try
            {
                var dlg = new SaveFileDialog
                {
                    Filter = "PNG形式|*.png",
                    DefaultExt = ".png"
                };
                if (dlg.ShowDialog().Value)
                {
                    dlg.FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_Graph";
                    var ext = Path.GetExtension(dlg.FileName).ToLower();
                    switch (ext)
                    {
                        case ".png":
                            _mainWindow.GraphView.GraphView.SaveBitmap(dlg.FileName, 0, 0);
                            break;
                    }
                    StatusDialogText.Value = "グラフを保存しました";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存にできませんでした。\n詳細:\n{ex}", "保存エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // 解析データ保存
        private void SaveAnalysisData()
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV形式(.csv)|*.csv|テキストファイル(.txt)|*.txt";
                saveFileDialog.Title = "解析データの保存";
                saveFileDialog.FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_FFT";
                bool? result = saveFileDialog.ShowDialog();

                if (result == true)
                {
                    using (Stream stream = saveFileDialog.OpenFile())
                    using (StreamWriter streamWriter = new StreamWriter(stream, Encoding.GetEncoding("UTF-8")))
                    {
                        streamWriter.WriteLine("Real,Imaginary,Phase,Magnitude,Frequency,Amplitude");
                        foreach (var data in AnalysisDataCollection.AnalysisDatas)
                        {
                            streamWriter.WriteLine($"{data.Real},{data.Imaginary},{data.Phase},{data.Magnitude},{data.Frequency},{data.Amplitude}");
                        }
                        StatusDialogText.Value = "データを保存しました";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存できませんでした。\n詳細:\n{ex}", "保存エラー", MessageBoxButton.OK, MessageBoxImage.Error);
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

        // コントロールを取得
        private void GetMainWindow()
        {
            var window = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault(_ => _ is MainWindow);
            _mainWindow = window;
        }

        private void ShowVersion()
        {
            var version = "【CrayfishMonitor】\nバージョン 1.2.0";
            var lib = "【使用ライブラリ】\n- OxyPlot.Wpf 2.1.0 (OxyPlot)" +
                "\n- ReactiveProperty 8.0.0 (neuecc, xin9le, okazuki)" +
                "\n- Math.NET Numerics 4.15.0 (Christoph Ruegg, Marcus Cuda, Jurgen Van Gael)";
            MessageBox.Show($"{version}\n\n{lib}", "バージョン情報", MessageBoxButton.OK);
        }
    }
}
