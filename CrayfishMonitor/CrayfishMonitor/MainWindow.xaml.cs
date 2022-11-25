using Microsoft.Win32;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CrayfishMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SerialPort _serialPort = new SerialPort();
        private static Stopwatch _stopWatch = new Stopwatch();

        List<string> SerialPortList = new List<string>();
        List<MeasurementData> MeasurementDataList = new List<MeasurementData>();
        ScottPlot.Plottable.ScatterPlotList plotList = new ();

        public MainWindow()
        {
            InitializeComponent();
            SerialDeviceSetup();
            PlotSetting();
        }

        private void SerialDeviceSetup()
        {
            var regexCheck = new Regex("(COM[1-9][0-9]?[0-9]?)");
            var Win32_PnPEntity = new ManagementClass("Win32_SerialPort");
            var deviceInstances = Win32_PnPEntity.GetInstances();
            foreach (var device in deviceInstances)
            {
                var deviceName = device.GetPropertyValue("Name");
                if (deviceName is null)
                {
                    continue;
                }
                else
                {
                    if (regexCheck.IsMatch(deviceName.ToString()))
                    {
                        SerialPortList.Add($"{device.GetPropertyValue("DeviceID")}");
                    }
                }
            }
            SerialPorts.ItemsSource = SerialPortList;
            
        }

        private void MeasurementButton(object sender, RoutedEventArgs e)
        {
            if(StatusLabel.Content == "接続中")
            {
                StatusLabel.Content = "切断中";
                _measurementButton.Background = new SolidColorBrush(Colors.LightGray);
                Disconnect();
            }
            else
            {
                Connect();
                StatusLabel.Content = "接続中";
                _measurementButton.Background = new SolidColorBrush(Colors.LightBlue);
                _serialPort.DataReceived += new SerialDataReceivedEventHandler(GetData);
            }
            
        }

        private void ResetButton(object sender, RoutedEventArgs e)
        {
            MeasurementDataList.Clear();
            Chart.Plot.Clear();
            plotList = new();
            MeasurementDataList.Clear();
            PlotSetting();
        }

        private void Connect()
        {
            //接続処理
            _serialPort = new SerialPort
            {
                PortName = SerialPorts.SelectedItem.ToString(),
                BaudRate = 9600,
                DataBits = 8,
                Parity = Parity.None,
                StopBits = StopBits.One,
                Encoding = Encoding.UTF8,
                DtrEnable = true
            };
            _serialPort.Open();
            StatusLabel.Content = "接続中";
        }

        private void Disconnect()
        {
            _serialPort.Close();
            _serialPort.Dispose();
        }

        private void GetData(object sender, SerialDataReceivedEventArgs e)
        {
            if (_serialPort is null) return;
            if (_serialPort.IsOpen.Equals(null)) return;
            try
            {
                var readData = _serialPort.ReadLine();
                if (!(readData.ToString().Length > 0)) return;
                ReadData(readData);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async void ReadData(string arduinoData)
        {
            if (!arduinoData.Contains(',')) return;
            var data = arduinoData.Split(',');
            if (!long.TryParse(data[0], out long longValue)) return; // 型チェック
            if (!double.TryParse(data[1], out double doubleValue)) return;
            MeasurementData measurementData = new MeasurementData();
            DateTime dateTime = DateTime.Now;
            measurementData.Elapsed = long.Parse(data[0]);
            measurementData.Voltage = double.Parse(data[1]);
            MeasurementDataList.Add(measurementData);
            if (MeasurementDataList.Count < 20) return;

            plotList.Add((double)measurementData.Elapsed / 1000, measurementData.Voltage);

            await Task.Run(() =>
            {
                this.Dispatcher.InvokeAsync(() =>
                {
                    if (MeasurementDataList.Count % 10 == 0)
                    {
                        if(AutoScaleBox.IsChecked.Value)
                        {
                            Chart.Plot.AxisAutoX();
                        }
                        try
                        {
                            Chart.Render();
                        }
                        catch (Exception ex) { }
                    }
                });
            });
        }

        private void PlotSetting()
        {
            Chart.Plot.Add(plotList);

            Chart.Plot.XAxis.Label("Time (s)", size: 26, fontName: "Segoe UI");
            Chart.Plot.XAxis.TickLabelStyle(fontSize: 18, fontName: "Segoe UI");
            Chart.Plot.XAxis.MajorGrid(lineWidth: 2, lineStyle: LineStyle.Solid);
            Chart.Plot.XAxis.Line(width: 2);
            plotList.MarkerSize = 0;
            plotList.LineWidth = 2;
            Chart.Plot.Palette = ScottPlot.Drawing.Palette.Category10;

            Chart.Plot.YAxis.Label("Voltage (V)", size: 26, fontName: "Segoe UI");
            Chart.Plot.YAxis.TickLabelStyle(fontSize: 18, fontName: "Segoe UI");
            Chart.Plot.YAxis.MajorGrid(lineWidth: 2, lineStyle: LineStyle.Solid);
            Chart.Plot.YAxis.Line(width: 2);
        }

        private void DataSaveButton(object sender, RoutedEventArgs e)
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
                        streamWriter.WriteLine("Elapsed[s],Voltage[V]");
                        foreach (var data in MeasurementDataList)
                        {
                            streamWriter.WriteLine($"{(double)data.Elapsed / 1000},{data.Voltage}");
                        }
                        MessageBox.Show("保存しました");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存できませんでした。\n詳細:\n{ex}", "保存エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenButton(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new OpenFileDialog();
                dialog.Filter = "CSVファイル (*.csv)|*.csv";

                if (dialog.ShowDialog() == true)
                {
                    using (StreamReader reader = new StreamReader(File.OpenRead(dialog.FileName)))
                    {
                        reader.ReadLine(); //1行目は項目なので捨てる
                        //var elapsed = new List<long>();
                        //var voltage = new List<double>();
                        //var measurementData = new MeasurementData();
                        plotList = new();
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            var values = line.Split(',');
                            //measurementData.Elapsed = long.Parse(values[0]);
                            //measurementData.Voltage = double.Parse(values[1]);
                            plotList.Add(double.Parse(values[0])/1000, double.Parse(values[1]));
                        }
                    }
                    Chart.Plot.AxisAutoX();
                    Chart.Render();
                }
                else return;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ファイル読み込み時にエラーが発生しました。\n詳細:\n{ex}", "ファイルオープンエラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
