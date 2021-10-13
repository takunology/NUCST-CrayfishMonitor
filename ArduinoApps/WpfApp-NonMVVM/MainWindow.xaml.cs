using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;

namespace WpfApp_NonMVVM
{
    public partial class MainWindow : Window
    {
        private SerialPort serialPort;
        private Stopwatch stopWatch = new Stopwatch();
        private ObservableCollection<Data> Datas = new ObservableCollection<Data>();
        private string SelectedPort { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            PortInit();
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
            MessageBox.Show("計測データをリセットしました。", "リセット", MessageBoxButton.OK, MessageBoxImage.Information);
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
        private void GraphSave_Button(object sender, RoutedEventArgs e)
        {

        }

        private void GetData(object sender, SerialDataReceivedEventArgs e)
        {
            if (serialPort == null) return;
            if (serialPort.IsOpen == false) return;

            Data ArduinoData = new Data();
            DateTime dateTime = DateTime.Now;
            ArduinoData.Date = dateTime.ToString("yyyy/MM/dd");
            ArduinoData.Time = dateTime.ToString("HH:mm:ss:fff");
            ArduinoData.Elapsed = stopWatch.ElapsedMilliseconds.ToString();
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

    }
}
