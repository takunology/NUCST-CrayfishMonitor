using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using CrayfishMonitor_Desktop.Models;
using CrayfishMonitor_Desktop.Services;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.Win32;
using Reactive.Bindings;

namespace CrayfishMonitor_Desktop.ViewModels
{
    public class DataPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ReactiveCollection<MeasurementSaveData> SaveDataItems { get; private set; } = DataCollections.SaveDataList;
        public ReactivePropertySlim<MeasurementSaveData> SelectedDataItem { get; set; } = new ReactivePropertySlim<MeasurementSaveData>();
        public ReactivePropertySlim<bool> SaveButtonState { get; private set; } = new ReactivePropertySlim<bool>(false);
        public ReactivePropertySlim<bool> ChartButtonState { get; private set; } = new ReactivePropertySlim<bool>(false);
        public ReactivePropertySlim<bool> RemoveButtonState { get; private set; } = new ReactivePropertySlim<bool>(false);
        public ReactiveCommand SaveCommand { get; } = new ReactiveCommand();
        public ReactiveCommand ShowChartCommand { get; } = new ReactiveCommand();
        public ReactiveCommand RemoveCommand { get; } = new ReactiveCommand();

        private Views.DataPage _dataPage;

        public DataPageViewModel(Views.DataPage dataPage)
        {
            this._dataPage = dataPage;
            SelectedDataItem.PropertyChanged += (s, e) => ButtonStateChange();
            ShowChartCommand.Subscribe(() => ShowChart());
            SaveCommand.Subscribe(() => DataSave());
            RemoveCommand.Subscribe(() => SaveDataItems.RemoveAt(SaveDataItems.IndexOf(SelectedDataItem.Value)));
        }

        private void ButtonStateChange()
        {
            if (SelectedDataItem.Value is not null)
            {
                SaveButtonState.Value = true;
                ChartButtonState.Value = true;
                RemoveButtonState.Value = true;
            }
            else
            {
                SaveButtonState.Value = false;
                ChartButtonState.Value = false;
                RemoveButtonState.Value = false;
            }
        }

        private async void DataSave()
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "名前をつけて保存";
                saveFileDialog.FileName = $"{SelectedDataItem.Value.Measurements[0].Time.ToString("yyyyMMdd_HHmmss")}_計測データ";
                saveFileDialog.Filter = "CSV形式(.csv)|*.csv|テキストファイル(.txt)|*.text";
                bool? retult = saveFileDialog.ShowDialog();

                if (retult == true)
                {
                    using (Stream stream = saveFileDialog.OpenFile())
                    using (var writer = new StreamWriter(stream, Encoding.GetEncoding("UTF-8")))
                    {
                        writer.WriteLine("Time,Elapsed[s],Voltage[V]");
                        SelectedDataItem.Value.Measurements.ForEach(data =>
                            writer.WriteLine($"{data.Time.ToString("HH:mm:ss")},{string.Format("{0:f3}",data.Elapsed)},{data.Voltage}"));
                    }
                }
            }
            catch (Exception ex)
            {
                await ContentDialogService.ShowExceptionAsync(_dataPage, ex);
            }
        }

        private void ShowChart()
        {
            DataCollections.SelectedListIndex = SaveDataItems.IndexOf(SelectedDataItem.Value);
            this._dataPage.Frame.Navigate(typeof(Views.ChartViewerPage), null, new SlideNavigationTransitionInfo()
            {
                Effect = SlideNavigationTransitionEffect.FromRight
            });
        }
    }
}
