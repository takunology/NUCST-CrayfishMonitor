using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrayfishMonitor_Desktop.Models;
using System.IO.Ports;

namespace CrayfishMonitor_Desktop.ViewModels
{
    public class SettingsPageViewModel
    {
        public ReactiveCommand SaveCommand { get; set; } = new ReactiveCommand();

        public List<int> BaudRate { get; set; } = new List<int>(SerialPortSetting.BaudRateList);
        public ReactivePropertySlim<int> StopBits { get; set; } = new ReactivePropertySlim<int>(8);
        public List<Parity> Parity { get; set; } = new List<Parity>(SerialPortSetting.ParityList);

        public ReactivePropertySlim<int> SelectedBaudRateIndex { get; set; } = new ReactivePropertySlim<int>(SerialPortSetting.BaudRateList[3]);
        public ReactivePropertySlim<Parity> SelectedParity { get; set; } = new ReactivePropertySlim<Parity>(SerialPortSetting.ParityList[3]);
        public SettingsPageViewModel()
        {


        }
    }
}
