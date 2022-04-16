using CrayfishMonitor_Desktop.Models;
using CrayfishMonitor_Desktop.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrayfishMonitor_Desktop.ViewModels
{
    public class MonitorPageViewModel
    { 
        public List<string> DeviceName { get; private set; } = new List<string>();
        public List<SerialDeviceData>Devices { get; private set; } = new List<SerialDeviceData>(SerialDeviceService.GetDevices());
        public ReactivePropertySlim<int> SelectedDeviceIndex { get; private set; } = new ReactivePropertySlim<int>(0);
        public ReactiveProperty<bool> MeasureButtonState { get; private set; } = new ReactiveProperty<bool>(false);
        public ReactiveCommand MeasureCommand { get; } = new ReactiveCommand();
        public ReactiveCommand ShowDataCommand { get; } = new ReactiveCommand();

        private Views.MonitorPage _monitorPage;

        public MonitorPageViewModel(Views.MonitorPage monitorPage)
        {
            this._monitorPage = monitorPage;
            DeviceName = Devices.Select(x => x.DeviceName).ToList();
            MeasureCommand.Subscribe(_ => SerialConnect(MeasureButtonState.Value));
            ShowDataCommand.Subscribe(_ => ShowDataDialog());
        }

        private void SerialConnect(bool toggleState)
        {
            if (toggleState is true)
            {
                try
                {
                    MeasureButtonState.Value = SerialDeviceService.SerialOpen(Devices[SelectedDeviceIndex.Value].DeviceId);
                }
                catch (Exception ex)
                {
                    ExceptionDialog(ex);
                    MeasureButtonState.Value = false;
                }
            }
            else
            {
                MeasureButtonState.Value = SerialDeviceService.SerialClose();
            }
        }
            
        private async void ShowDataDialog()
        {
            if (DataCollections.Measurements.Count > 0)
            {
                string datas = "";
                foreach(var measurement in DataCollections.Measurements)
                {
                    datas += $"{measurement.Time.ToString("hh:mm:ss")} \t {measurement.Elapsed} : {measurement.Voltage} [V]\n";
                }

                ContentDialog dialog = new ContentDialog()
                {
                    Title = "計測データ",
                    Content = new ScrollViewer()
                    {
                        Content = new TextBlock() { Text = datas }
                    },
                    CloseButtonText = "閉じる",
                    XamlRoot = _monitorPage.Content.XamlRoot
                };
                await dialog.ShowAsync();
            }
            else
            {
                ContentDialog dialog = new ContentDialog()
                {
                    Title = "計測データがありません",
                    Content = "測定ボタンを押して測定を行ってください。",
                    CloseButtonText = "OK",
                    XamlRoot = _monitorPage.Content.XamlRoot
                };
                await dialog.ShowAsync();
            }
           
        }

        private async void ExceptionDialog(Exception ex)
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = "エラー",
                Content = ex.Message,
                CloseButtonText = "閉じる",
                XamlRoot = _monitorPage.Content.XamlRoot
            };
            await dialog.ShowAsync();
        }
    }
}
