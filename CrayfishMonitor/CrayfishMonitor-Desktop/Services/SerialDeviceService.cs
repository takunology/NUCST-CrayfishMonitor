using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using CrayfishMonitor_Desktop.Models;

namespace CrayfishMonitor_Desktop.Services
{
    public static class SerialDeviceService
    {
        private static SerialPort _serialPort = null;
        private static Stopwatch _stopWatch = new Stopwatch();

        public static List<SerialDeviceData> GetDevices()
        {
            var deviceList = new List<Models.SerialDeviceData>();
            var regexCheck =  new Regex("(COM[1-9][0-9]?[0-9]?)");
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
                        deviceList.Add(new Models.SerialDeviceData
                        {
                            DeviceId = device.GetPropertyValue("DeviceID").ToString(),
                            DeviceName = deviceName.ToString(),
                        });
                    }
                }
            }
            return deviceList;
        }

        public static bool SerialOpen(string portId)
        {
            DataCollections.Measurements.Clear();
            _serialPort = new SerialPort
            {
                PortName = portId,
                BaudRate = 115200,
                DataBits = 8,
                Parity = Parity.None,
                StopBits = StopBits.One,
                WriteTimeout = 5000,
                ReadTimeout = 5000,
                Encoding = Encoding.UTF8,
                DtrEnable = true,
            };
            _stopWatch.Start();
            try
            {
                _serialPort.DataReceived -= DataReceive;
                _serialPort.DataReceived += DataReceive;
                _serialPort.Open();
# if DEBUG
                Debug.WriteLine("Serial Open");
#endif
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public static bool SerialClose()
        {
            try
            {
                _serialPort?.Close();
                _serialPort?.Dispose();
                _stopWatch.Stop();
                _stopWatch.Reset();
#if DEBUG
                Debug.WriteLine("Serial Close.");
#endif
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public static async void DataReceive(object sender, SerialDataReceivedEventArgs e)
        {
            if (_serialPort is null) return;
            if (_serialPort.IsOpen.Equals(null)) return;

            var readData = _serialPort.ReadExisting();
            var splitData = readData.Split("\n");

            foreach (var datas in splitData)
            {
                if (datas.Length > 0 && !datas.StartsWith('.') && !datas.StartsWith(','))
                {
                    if (!datas.Contains(',')) continue;
                    var data = datas.Split(',');
                    if (!long.TryParse(data[0], out long longValue)) continue;
                    if (!double.TryParse(data[1], out double doubleValue)) continue;
                    MeasurementData measurementData = new MeasurementData();
                    DateTime dateTime = DateTime.Now;
                    measurementData.Time = dateTime;
                    measurementData.Elapsed = long.Parse(data[0]);
                    measurementData.Voltage = double.Parse(data[1]);
                    DataCollections.Measurements.Add(measurementData);
                }
            }

            /*await Task.Run(() =>
            {
                
            });
            /*var splitData = readData.Split("\n");
            await SplitAsync(splitData);
            foreach (var data in splitData)
            {
                foreach(var item in data.Split("\f"))
                {

                }
                MeasurementData data = new MeasurementData();
                DateTime dateTime = DateTime.Now;
                data.Time = dateTime;
                data.Elapsed = _stopWatch.ElapsedMilliseconds;
                data.Voltage = double.Parse(_serialPort.ReadLine());
                DataCollections.Measurements.Add(data);
                //Debug.WriteLine(split);
            }
            /**/
#if DEBUG
            //Debug.WriteLine($"{_serialPort.ReadExisting()}");
#endif
            //_serialPort.DataReceived -= DataReceive;
        }
    }
}