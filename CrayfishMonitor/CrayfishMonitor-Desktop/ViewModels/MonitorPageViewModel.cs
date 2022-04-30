using CrayfishMonitor_Desktop.Models;
using CrayfishMonitor_Desktop.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using OxyPlot;
using OxyPlot.Series;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using Windows.Devices.Enumeration;
using Windows.UI.Core;

namespace CrayfishMonitor_Desktop.ViewModels
{
    public class MonitorPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ReadOnlyReactiveCollection<string> DeviceName { get; set; }
        public ReactiveCollection<SerialDeviceData>Devices { get; private set; } = SerialDeviceService.GetDevices();
        public ReactivePropertySlim<int> SelectedDeviceIndex { get; private set; } = new ReactivePropertySlim<int>(0);
        public ReactivePropertySlim<bool> MeasureButtonState { get; private set; } = new ReactivePropertySlim<bool>(false);
        public ReactivePropertySlim<PlotModel> MeasurementChart { get; private set; } = new ReactivePropertySlim<PlotModel>();
        public ReactiveCommand MeasureCommand { get; } = new ReactiveCommand();
        public ReactiveCommand ShowDataCommand { get; } = new ReactiveCommand();

        private Views.MonitorPage _monitorPage;
        private DeviceWatcher _deviceWatcher = DeviceInformation.CreateWatcher();

        public MonitorPageViewModel(Views.MonitorPage monitorPage)
        {
            this._monitorPage = monitorPage;
            DeviceName = Devices.ToReadOnlyReactiveCollection(x => x.DeviceName);
            MeasureCommand.Subscribe(() => SerialConnect(MeasureButtonState.Value));
            ShowDataCommand.Subscribe(() => ShowDataDialog());
            this._deviceWatcher.Updated += (s, e) => Devices = SerialDeviceService.GetDevices();
            this._deviceWatcher.Start();
        }

        private async void SerialConnect(bool toggleState)
        {
            if (toggleState is true)
            {
                try
                {
                    MeasureButtonState.Value = SerialDeviceService.SerialOpen(Devices[SelectedDeviceIndex.Value].DeviceId);
                }
                catch (Exception ex)
                {
                    await ContentDialogService.ShowExceptionAsync(_monitorPage, ex);
                    MeasureButtonState.Value = false;
                }
            }
            else
            {
                MeasureButtonState.Value = SerialDeviceService.SerialClose();
                SaveMeasurementData();

            }
        }
            
        private async void ShowDataDialog()
        {
            if (DataCollections.Measurements.Count > 0)
            {
                string datas = "";
                foreach (var measurement in DataCollections.Measurements)
                {
                    datas += $"{measurement.Time.ToString("hh:mm:ss")} \t {measurement.Elapsed} : {measurement.Voltage} [V]\n";
                }
           
                await ContentDialogService.ShowScrollAsync(_monitorPage, "測定データ", datas);
            }
            else
            {
                await ContentDialogService.ShowAsync(_monitorPage, "計測データがありません", "計測ボタンを押して測定を行ってください");
            }
           
        }

        private void SaveMeasurementData()
        {
            MeasurementSaveData measurementSaveData = new MeasurementSaveData();
            measurementSaveData.Date = DateTime.Now.ToString("yyyy/MM/dd");
            measurementSaveData.Time = DateTime.Now.ToString("hh:mm:ss");
            measurementSaveData.DeviceName = Devices[SelectedDeviceIndex.Value].DeviceName;
            measurementSaveData.Title = $"{measurementSaveData.Date} {measurementSaveData.Time} {measurementSaveData.DeviceName}";
            measurementSaveData.Measurements = DataCollections.Measurements.ToList();
            DataCollections.SaveDataList.Add(measurementSaveData);            
        }
    }
}
