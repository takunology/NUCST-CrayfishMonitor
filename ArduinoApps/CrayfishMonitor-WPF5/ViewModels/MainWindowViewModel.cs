using Reactive.Bindings;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrayfishMonitor_WPF5.Models;
using System.IO.Ports;
using System.Windows;
using System.Windows.Data;
using System.Diagnostics;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using System.Windows.Threading;
using System.Windows.Media;
using Microsoft.Win32;
using System.IO;

namespace CrayfishMonitor_WPF5.ViewModels
{
    public class MainWindowViewModel
    {
        public ReactiveProperty<string> StatusText { get; private set; } = new ReactiveProperty<string>("切断中");
        public ReactiveProperty<string> StatusDialogText { get; private set; } = new ReactiveProperty<string>("準備完了");
        public ReactiveProperty<SolidColorBrush> StatusColor { get; private set; } = new ReactiveProperty<SolidColorBrush>(new SolidColorBrush(Colors.WhiteSmoke));
        public ReactiveCollection<string> SerialPortList { get; private set; } = new ReactiveCollection<string>();
        public ReactiveProperty<bool> IsEnabledSerialPortList { get; private set; } = new ReactiveProperty<bool>(true);
        public ReactiveProperty<string> SelectedPort { get; private set; } = new ReactiveProperty<string>();
        public ReactiveCommand ConnectCommand { get; private set; } = new ReactiveCommand();
        public ReactiveProperty<string> ConnectButtonText { get; private set; } = new ReactiveProperty<string>("接続");
        public ReactiveProperty<PlotModel> PlotView { get; private set; } = new ReactiveProperty<PlotModel>();
        public ReactiveCommand DataSaveCommand { get; private set; } = new ReactiveCommand();
        public ReactiveCommand GraphSaveCommand { get; private set; } = new ReactiveCommand();
        public ReactiveCommand ResetCommand { get; private set; } = new ReactiveCommand();
        public ReactiveCollection<ArduinoData> ArduinoDatas { get; private set; } = new ReactiveCollection<ArduinoData>();
        
        private SerialPort _serialPort = new SerialPort();
        private Stopwatch _stopWatch = new Stopwatch();
        private DispatcherTimer _Timer = new DispatcherTimer();
        private PlotModel PlotModel { get; } = new PlotModel() { Background = OxyColors.White };
        private LineSeries LineSeries = new LineSeries();

        private MainWindow MainWindow;

        public MainWindowViewModel()
        {
            // 非同期なスレッドでコレクションを使うための有効化
            BindingOperations.EnableCollectionSynchronization(ArduinoDatas, new object());
            GetMainWindow();
            GraphSetup();
            PortInitialize();
            // ボタン発火イベントの登録
            this.ConnectCommand.Subscribe(_ => Connection());
            this.DataSaveCommand.Subscribe(_ => SaveData());
            this.GraphSaveCommand.Subscribe(_ => SaveGraph());
            this.ResetCommand.Subscribe(_ => DataReset());
        }

        // MainWindow View にあるコントロールを取得する
        private void GetMainWindow()
        {
            var window = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault(_ => _ is MainWindow);
            MainWindow = (MainWindow)window;
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
            ArduinoDatas.Clear();

            _serialPort = new SerialPort
            {
                PortName = SelectedPort.Value,
                BaudRate = 9600,
                DataBits = 8,
                Parity = Parity.None,
                StopBits = StopBits.One,
                Encoding = Encoding.UTF8,
                DtrEnable = true
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

            //データ取得とModelへの書き込み
            ArduinoData arduinoData = new ArduinoData();
            DateTime dateTime = DateTime.Now;
            arduinoData.Date = dateTime.ToString("yyyy/MM/dd");
            arduinoData.Time = dateTime.ToString("HH:mm:ss:fff");
            arduinoData.Elapsed = _stopWatch.ElapsedMilliseconds;
            arduinoData.Voltage = double.Parse(_serialPort.ReadLine());

            ArduinoDatas.Add(arduinoData);

            Task.Run(async () => 
            {
                // プロット数が 1000 超えたらデキュー
                if (this.LineSeries.Points.Count >= 1000)
                {
                    this.LineSeries.Points.RemoveAt(0);
                }
                // 自動スクロール
                var ArduinoDataGrid = MainWindow.ArduinoDataGrid;
                MainWindow.Dispatcher.Invoke(() =>
                {
                    if (ArduinoDataGrid.Items.Count > 0)
                    {
                        ArduinoDataGrid.Dispatcher.Invoke(() =>
                        ArduinoDataGrid.ScrollIntoView(ArduinoDataGrid.Items.GetItemAt(ArduinoDataGrid.Items.Count - 1)));
                    }

                    lock (this.PlotModel.SyncRoot)
                    {
                        this.LineSeries.Points.Add(new DataPoint(arduinoData.Elapsed, arduinoData.Voltage));
                        this.PlotModel.InvalidatePlot(true);
                    }
                });
            });
        }

        // グラフ初期化
        private void GraphSetup()
        {
            var Axes_x = new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                TickStyle = TickStyle.Inside,
                AbsoluteMinimum = 0,
                MajorGridlineStyle = LineStyle.Automatic,
                MinorGridlineStyle = LineStyle.Dash,
                TitleFontSize = 16,
                Title = "Time [ms]"
            };

            var Axes_y = new LinearAxis()
            {
                Position = AxisPosition.Left,
                MajorTickSize = 10,
                Maximum = 5,
                Minimum = 0,
                TickStyle = TickStyle.Inside,
                AbsoluteMinimum = 0,
                MajorGridlineStyle = LineStyle.Automatic,
                MinorGridlineStyle = LineStyle.Dash,
                TitleFontSize = 16,
                Title = "Voltage [V]"
            };

            this.PlotModel.Axes.Add(Axes_x);
            this.PlotModel.Axes.Add(Axes_y);

            this.LineSeries.StrokeThickness = 1.5;
            this.LineSeries.Color = OxyColor.FromRgb(0, 100, 205);

            this.PlotModel.Series.Add(this.LineSeries);
            this.PlotView.Value = this.PlotModel;
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
                        foreach (var data in ArduinoDatas)
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

        // グラフの保存
        private void SaveGraph()
        {
            try
            {
                var GraphView = MainWindow.GraphView;
                var dlg = new SaveFileDialog
                {
                    Filter = "PNG形式|*.png",
                    DefaultExt = ".png"
                };
                if (dlg.ShowDialog().Value)
                {
                    var ext = Path.GetExtension(dlg.FileName).ToLower();
                    switch (ext)
                    {
                        case ".png":
                            GraphView.SaveBitmap(dlg.FileName, 0, 0);
                            break;
                    }
                }
                StatusDialogText.Value = "グラフを保存しました";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存に失敗しました。\n詳細:\n{ex}", "保存エラー");
            }

        }

        // 記録データのリセット
        private void DataReset()
        {
            try
            {
                ArduinoDatas.Clear();
                _stopWatch.Reset();
                LineSeries.Points.Clear();
                PlotModel.InvalidatePlot(true);
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
