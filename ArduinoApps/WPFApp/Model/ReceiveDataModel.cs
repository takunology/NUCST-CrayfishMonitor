using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace WPFApp.Model
{
    public class ReceiveDataModel
    {
        public ObservableCollection<DataLogModel> Data = new ObservableCollection<DataLogModel>();
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();
        private SerialPort serialPort;

        public ReceiveDataModel(string portName, int serialRate)
        {
            portName = "COM6"; 
            serialPort = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One);
        }

        public void Open()
        {
            serialPort.Open();

            serialPort.DataReceived += (s, e) =>
            {
                DateTime dateTime = DateTime.Now;
                dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
                dispatcherTimer.Start();

                var readData = serialPort.ReadLine();

                Data.Add(
                    new DataLogModel
                    {
                        Date = dateTime.Date,
                        Time = dateTime.TimeOfDay,
                        MiliSec = dateTime.Millisecond,
                        DigitalData = double.Parse(readData)
                    }
                );

                MessageBox.Show(readData);
            };
        }

        public void Close()
        {
            dispatcherTimer.Stop();
            serialPort.Close();
        }

    }
}
