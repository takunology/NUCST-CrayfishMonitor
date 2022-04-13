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

        public static List<SerialDevice> GetDevices()
        {
            var deviceList = new List<Models.SerialDevice>();
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
                        deviceList.Add(new Models.SerialDevice
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
            try
            {
                DataCollections.Measurements.Clear();
                _serialPort = new SerialPort
                {
                    PortName = portId,
                    BaudRate = 9600,
                    DataBits = 8,
                    Parity = Parity.None,
                    StopBits = StopBits.One,
                    WriteTimeout = 1000,
                    ReadTimeout = 1000,
                    Encoding = Encoding.UTF8,
                    DtrEnable = true,
                };
                _serialPort.Open();
                _stopWatch.Start();
                Task.Run(() => _serialPort.DataReceived += async (s, e) => await DataReceive());
                //_serialPort.DataReceived += (s, e) => Task.Run(() => DataReceive());
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

        public static async Task DataReceive()
        {
            await Task.Run(() =>
            {
                try
                {
                    if (_serialPort is null) return;
                    if (_serialPort.IsOpen.Equals(null)) return;

                    MeasurementData data = new MeasurementData();
                    DateTime dateTime = DateTime.Now;
                    data.Time = dateTime;
                    data.Elapsed = _stopWatch.ElapsedMilliseconds;
                    data.Voltage = double.Parse(_serialPort.ReadLine());
                    DataCollections.Measurements.Add(data);
#if DEBUG
                    Debug.WriteLine($"{_serialPort.ReadChar()}");
#endif
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            });

        }

        
    }
}