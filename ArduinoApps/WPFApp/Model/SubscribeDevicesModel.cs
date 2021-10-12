using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Management;

namespace WPFApp.Model
{
    //使用可能なポート番号とその機器名を登録する
    public class SubscribeDevicesModel
    {
        public ObservableCollection<string> Devices = new ObservableCollection<string>();
        public SubscribeDevicesModel()
        {
            //COM接続しているデバイスの初期化
            ManagementClass manageDevice = new ManagementClass("Win32_PnPEntity");
            var comChecker = new System.Text.RegularExpressions.Regex("(COM[0-9][0-9]?[0-9]?)");
            string[] devicePort = SerialPort.GetPortNames();
            int comNum = 0;

            foreach (var deviceInstance in manageDevice.GetInstances())
            {
                var deviceObject = deviceInstance.GetPropertyValue("Name");

                if (deviceObject is null)
                {
                    continue;
                }
                
                if(comChecker.IsMatch(deviceObject.ToString()))
                {
                    Devices.Add(deviceObject.ToString());
                }

            }

        }
    }
}
