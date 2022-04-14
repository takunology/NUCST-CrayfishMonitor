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
using CrayfishMonitor_Desktop.Models;

namespace CrayfishMonitor_Desktop.Services
{
    public static class MeasurementService
    {
        private static Stopwatch _stopWatch = new Stopwatch();
        private static SerialDevice _serialDevice;

        private static async Task DeviceSetupAsync(string deviceId)
        {
            string aqs = SerialDevice.GetDeviceSelector(deviceId);
            var devices = await DeviceInformation.FindAllAsync(aqs);
            try
            {
                if (devices.Any())
                {               
                    deviceId = devices.First().Id;
                    _serialDevice = await SerialDevice.FromIdAsync(deviceId);
                    if (_serialDevice != null)
                    {
                        _serialDevice.WriteTimeout = TimeSpan.FromMilliseconds(1000);
                        _serialDevice.ReadTimeout = TimeSpan.FromMilliseconds(1000);
                        _serialDevice.BaudRate = 115200;
                        _serialDevice.Parity = SerialParity.None;
                        _serialDevice.StopBits = SerialStopBitCount.One;
                        _serialDevice.DataBits = 8;
                        _serialDevice.IsDataTerminalReadyEnabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public static async Task Measurement(string serialId, CancellationToken cancelToken)
        {
            await DeviceSetupAsync(serialId);
            try
            {
                var dataReader = new DataReader(_serialDevice.InputStream);
                dataReader.InputStreamOptions = InputStreamOptions.Partial;
                string rxBuffer = "";
                _stopWatch.Start();
                while (true)
                {
                    if (cancelToken.IsCancellationRequested)
                    {
                        dataReader.DetachStream();
                        dataReader.DetachBuffer();
                        dataReader.Dispose();
                        _serialDevice.Dispose();

                        return;
                    }

                    var bytesToRead = await dataReader.LoadAsync(256);
                    rxBuffer = dataReader.ReadString(bytesToRead);
                    Debug.Write(rxBuffer);

                    /*await Task.Run(async () =>
                    {
                        var bytesToRead = await dataReader.LoadAsync(7);
                        rxBuffer = dataReader.ReadString(bytesToRead);
                        Debug.WriteLine(bytesToRead);
                        string[] subs = rxBuffer.Split('\n');
                        foreach (string str in subs)
                        {
                            MeasurementData data = new MeasurementData();
                            DateTime dateTime = DateTime.Now;
                            data.Time = dateTime;
                            data.Elapsed = _stopWatch.ElapsedMilliseconds;
                            double num;
                            if (double.TryParse(str, out num))
                            {
                                data.Voltage = double.Parse(str);
                            }
                            else
                            {
                                continue;
                            }
                            DataCollections.Measurements.Add(data);

                            Debug.WriteLine($"{data.Elapsed}\t{data.Voltage}");
                        }
                        //Debug.WriteLine($"{rxBuffer}");
                    });*/
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
