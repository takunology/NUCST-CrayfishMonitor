using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Management;
using System.Text.RegularExpressions;

namespace AppTest
{
    [TestClass]
    public class SerialDevices
    {
        [TestMethod]
        public void GetDevicesTest()
        {
            var deviceNameList = new List<ComPort>();
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
                        deviceNameList.Add(new ComPort { 
                            DeviceId = device.GetPropertyValue("DeviceID").ToString(),
                            DeviceName = deviceName.ToString(),
                        });
                    }
                }
            }
            deviceNameList.ForEach(p => Console.WriteLine($"{p.DeviceId}\t{p.DeviceName}"));
        }
    }

    public struct ComPort
    {
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
    }
}