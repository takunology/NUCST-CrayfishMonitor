using System;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using System.Threading;

namespace WpfApp_NonMVVM
{
    public partial class MainWindow : Window
    {
        private SerialPort serialPort;
        PlotModel plotModel { get; } = new PlotModel() {  Background = OxyColors.White };
        LineSeries lineSeries = new LineSeries();
        private ObservableCollection<Data> Datas = new ObservableCollection<Data>();
        private Stopwatch stopWatch = new Stopwatch();
        private string SelectedPort { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            PortInit();
            GraphSetup();
        }
        private void DisConnect_Button(object sender, RoutedEventArgs e)
        {
            serialPort.Close();
            serialPort = null;
            stopWatch.Stop();

            Connect_Label.Content = "切断中";
            disConnect_Button.IsEnabled = false;
            connect_Button.IsEnabled = true;
            dataReset_Button.IsEnabled = true;
            dataSave_Button.IsEnabled = true;
            graphSave_Button.IsEnabled = true;
        }
        private void Connect_Button(object sender, RoutedEventArgs e)
        {
            SelectedPort = Devices_ComboBox.SelectedValue.ToString();

            serialPort = new SerialPort()
            {
                PortName = SelectedPort,
                BaudRate = 9600,
                DataBits = 8,
                Parity = Parity.None,
                StopBits = StopBits.One,
                Encoding = Encoding.UTF8,
                DtrEnable = true
            };
          
            try
            {
                serialPort.Open();
                stopWatch.Start(); //計測開始
                disConnect_Button.IsEnabled = true;
                connect_Button.IsEnabled = false;
                dataSave_Button.IsEnabled = false;
                graphSave_Button.IsEnabled = false;
                dataReset_Button.IsEnabled = false;
                Connect_Label.Content = SelectedPort + "と接続中";
                serialPort.DataReceived += new SerialDataReceivedEventHandler(GetData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(SelectedPort + "を開けません。");
            }
            
        }
        private void DataReset_Button(object sender, RoutedEventArgs e)
        {
            connect_Button.IsEnabled = true;
            disConnect_Button.IsEnabled = false;
            dataSave_Button.IsEnabled = false;
            graphSave_Button.IsEnabled = false;
            Datas.Clear();
            stopWatch.Reset();
            Arduino_DataGrid.ItemsSource = Datas;
            lineSeries.Points.Clear();
            plotModel.InvalidatePlot(true);
            MessageBox.Show("計測データをリセットしました。", "リセット", MessageBoxButton.OK, MessageBoxImage.Information);
            plotModel.InvalidatePlot(false);

        }
        private void DataSave_Button(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV形式(.csv)|*.csv|テキストファイル(.txt)|*.txt";
                saveFileDialog.Title = "計測データの保存";
                bool? result = saveFileDialog.ShowDialog();

                if(result == true)
                {
                    using (Stream stream = saveFileDialog.OpenFile())
                    using (StreamWriter streamWriter = new StreamWriter(stream, Encoding.GetEncoding("UTF-8")))
                    {
                        streamWriter.WriteLine("Date,Time,Elapsed[ms],Voltage[V]");
                        foreach(var data in Arduino_DataGrid.Items)
                        {
                            streamWriter.WriteLine(string.Join(",", Arduino_DataGrid.Columns
                                .Select(x => x.OnCopyingCellClipboardContent(data)?.ToString())
                            ));
                        }
                        MessageBox.Show("測定データを保存しました。", "データの保存", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "エラー");
            }
        }
        private void GetData(object sender, SerialDataReceivedEventArgs e)
        {
            if (serialPort == null) return;
            if (serialPort.IsOpen == false) return;

            Data ArduinoData = new Data();
            DateTime dateTime = DateTime.Now;
            ArduinoData.Date = dateTime.ToString("yyyy/MM/dd");
            ArduinoData.Time = dateTime.ToString("HH:mm:ss:fff");
            ArduinoData.Elapsed = stopWatch.ElapsedMilliseconds;
            ArduinoData.Voltage = double.Parse(serialPort.ReadLine());

            Task.Run(() => {

                Arduino_DataGrid.Dispatcher.Invoke(() => Datas.Add(ArduinoData));
                Arduino_DataGrid.Dispatcher.Invoke(() => Arduino_DataGrid.ItemsSource = Datas);
                //自動スクロール
                if (Arduino_DataGrid.Items.Count > 0)
                {
                    Arduino_DataGrid.Dispatcher.Invoke(() =>
                    Arduino_DataGrid.ScrollIntoView(Arduino_DataGrid.Items.GetItemAt(Arduino_DataGrid.Items.Count - 1)));
                }
                //グラフの自動水平移動を行うプロット数
                if (lineSeries.Points.Count >= 2000)
                {
                    lineSeries.Points.RemoveAt(0);
                }
                lock (plotModel.SyncRoot)
                {
                    lineSeries.Points.Add(new DataPoint(ArduinoData.Elapsed, ArduinoData.Voltage));
                    plotModel.InvalidatePlot(true);
                }
            });
            
        }
        private void PortInit()
        {
            try
            {
                string[] ports = SerialPort.GetPortNames();
                foreach (string port in ports)
                {
                    Devices_ComboBox.Items.Add(port);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "COMポートを読み込めません", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void GraphSetup()
        {
            var Axes_x = new OxyPlot.Axes.LinearAxis()
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
                MajorTickSize=10,
                TickStyle = TickStyle.Inside,
                AbsoluteMinimum = 0,
                MajorGridlineStyle = LineStyle.Automatic,
                MinorGridlineStyle = LineStyle.Dash,
                TitleFontSize = 16,
                Title = "Voltage [V]"
            };

            plotModel.Axes.Add(Axes_x);
            plotModel.Axes.Add(Axes_y);

            lineSeries.StrokeThickness = 1.5;
            lineSeries.Color = OxyColor.FromRgb(0, 100, 205);

            plotModel.Series.Add(lineSeries);
            GraphView.Model = plotModel;
        }
        private void GraphSave_Button(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog
            {
                Filter = "PNG形式|*.png",
                DefaultExt = ".png"
            };
            if (dlg.ShowDialog(this).Value)
            {
                var ext = Path.GetExtension(dlg.FileName).ToLower();
                switch (ext)
                {
                    case ".png":
                        GraphView.SaveBitmap(dlg.FileName, 0, 0);
                        break;
                }
            }
        }
    }
}
