using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Text;
using System.Windows.Threading;

namespace WPFApp.Model
{
    public class ReceiveDataModel
    {
        public ObservableCollection<DataLogModel> Data { get; set; }
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();
        private SerialPort serialPort;

        public ReceiveDataModel()
        {
            
        }

        public void Open(string portName, int serialRate)
        {
            serialPort = new SerialPort("COM6", 9600, Parity.None, 8, StopBits.One);

            serialPort.Open();

            DateTime dateTime = DateTime.Now;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            dispatcherTimer.Start();

            serialPort.DataReceived += (s, e) =>
            {
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
            };
        }

        public void Close()
        {
            dispatcherTimer.Stop();
            serialPort.Close();
        }

    }
}
