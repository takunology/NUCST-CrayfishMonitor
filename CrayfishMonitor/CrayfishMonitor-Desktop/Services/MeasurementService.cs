using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.SerialCommunication;
using Windows.Devices.Enumeration;
using Windows.Storage.Streams;
using System.Threading;
using System.Windows;

namespace CrayfishMonitor_Desktop.Services
{
    public static class MeasurementService
    {
        private static Stopwatch _stopWatch = new Stopwatch();

        public static async Task Measurement(string DeviceId, CancellationToken cancelToken)
        {
            try
            {
                string aqs = SerialDevice.GetDeviceSelector(DeviceId);
                var dis = await DeviceInformation.FindAllAsync(aqs);
                var _serialDevice = await SerialDevice.FromIdAsync(dis[0].Id);

                _serialDevice.WriteTimeout = TimeSpan.FromMilliseconds(1000);
                _serialDevice.ReadTimeout = TimeSpan.FromMilliseconds(1000);
                _serialDevice.BaudRate = 9600;
                _serialDevice.Parity = SerialParity.None;
                _serialDevice.StopBits = SerialStopBitCount.One;
                _serialDevice.DataBits = 8;
                _serialDevice.IsDataTerminalReadyEnabled = true;

                const uint maxReadLength = 256;
                var dataReader = new DataReader(_serialDevice.InputStream);
                string rxBuffer = "";

                try
                {
                    _stopWatch.Start();

                    while (true)
                    {
                        if (cancelToken.IsCancellationRequested)
                        {
#if DEBUG
                            Debug.WriteLine("Task Canceled.");
#endif
                            _serialDevice.Dispose();
                            return;
                        }

                        uint bytesToRead = await dataReader.LoadAsync(maxReadLength);
                        rxBuffer = dataReader.ReadString(bytesToRead);
                        if (rxBuffer.Contains("\n"))
                        {
                            Debug.WriteLine($"{_stopWatch.Elapsed}\t{rxBuffer}");
                        }
                    }

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
