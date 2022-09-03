using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using CrayfishMonitor_Desktop.Models;
using Reactive.Bindings;

namespace CrayfishMonitor_Desktop.Services
{
    public static class SerialDeviceService
    {
        private static SerialPort _serialPort = null;
        private static Stopwatch _stopWatch = new Stopwatch();

        public static ReactiveCollection<SerialDeviceData> GetDevices()
        {
            var deviceList = new ReactiveCollection<Models.SerialDeviceData>();
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

            InitializeSerialSetting(portId);
            _stopWatch.Start();
            _serialPort.DataReceived -= DataReceive;
            _serialPort.DataReceived += DataReceive;
            _serialPort.Open();
#if DEBUG
            Debug.WriteLine("Serial Open");
#endif
            return true;
        }

        public static bool SerialClose()
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

        public static void DataReceive(object sender, SerialDataReceivedEventArgs e)
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

        private static void ReadData(string arduinoData)
        {
            if (!arduinoData.Contains(',')) return;
            var data = arduinoData.Split(',');
            if (!long.TryParse(data[0], out long longValue)) return; // 型チェック
            if (!double.TryParse(data[1], out double doubleValue)) return;
            MeasurementData measurementData = new MeasurementData();
            DateTime dateTime = DateTime.Now;
            measurementData.Time = dateTime;
            measurementData.Elapsed = double.Parse(data[0]) / 1000;
            measurementData.Voltage = double.Parse(data[1]);
            if (measurementData.Elapsed > _stopWatch.Elapsed.Seconds) return; // Arduino にスタックされたデータを無視
            DataCollections.Measurements.Add(measurementData);
        }

        private static void InitializeSerialSetting(string portId)
        {
            _serialPort = new SerialPort
            {
                PortName = portId,
                BaudRate = 9600,
                DataBits = SerialPortSetting.DataBits,
                Parity = SerialPortSetting.Parity,
                StopBits = SerialPortSetting.StopBits,
                WriteTimeout = SerialPortSetting.WriteTimeOut,
                ReadTimeout = SerialPortSetting.ReadTimeOut,
                Encoding = SerialPortSetting.Encoding,
                DtrEnable = SerialPortSetting.DtrEnable,
            };
        }
    }
}