using CrayfishMonitor_Desktop.Models;
using CrayfishMonitor_Desktop.Services;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace CrayfishMonitor_Desktop.ViewModels
{
    public class MonitorPageViewModel
    {
        public List<string> DeviceName { get; private set; } = new List<string>();
        public List<SerialDeviceData>Devices { get; private set; } = new List<SerialDeviceData>(SerialDeviceService.GetDevices());
        public ReactivePropertySlim<int> SelectedDeviceIndex { get; private set; } = new ReactivePropertySlim<int>(0);
        public ReactiveProperty<bool> MeasureButtonState { get; } = new ReactiveProperty<bool>(false);
        public ReactiveCommand MeasureButton { get; set; } = new ReactiveCommand();

        private CancellationTokenSource _tokenSource;

        public MonitorPageViewModel()
        {
            DeviceName = Devices.Select(x => x.DeviceName).ToList();
            MeasureButton.Subscribe(_ => SerialConnect(MeasureButtonState.Value));
        }

        private async void SerialConnect(bool toggleState)
        {
            if (toggleState is true)
            {
                //SerialDeviceService.SerialOpen(Devices[SelectedDeviceIndex.Value].DeviceId);
                _tokenSource = new CancellationTokenSource();
                await MeasurementService.Measurement(Devices[SelectedDeviceIndex.Value].DeviceId, _tokenSource.Token);
            }
            else
            {
                _tokenSource.Cancel();
                //SerialDeviceService.SerialClose();
            }
        }
            
    }
}
