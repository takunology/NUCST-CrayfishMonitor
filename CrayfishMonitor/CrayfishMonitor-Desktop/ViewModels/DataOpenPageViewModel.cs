using Microsoft.Win32;
using OxyPlot;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrayfishMonitor_Desktop.ViewModels
{
    public class DataOpenPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ReactivePropertySlim<string> FilePathStr { get; private set; } = new ReactivePropertySlim<string>();
        public ReactiveCommand OpenFileCommand { get; } = new ReactiveCommand();

        public ReactiveProperty<PlotModel> PlotView;
        
        private List<long> _ElapsedDataList = new List<long>();
        private List<double> _VoltageDataList = new List<double>();
        private Views.DataOpenPage _dataOpenPage;

        public DataOpenPageViewModel(Views.DataOpenPage dataOpenPage)
        {
            this._dataOpenPage = dataOpenPage;
            OpenFileCommand.Subscribe(() => OpenFile());
        }

        private void OpenFile()
        {
            try
            {
                var dialog = new OpenFileDialog();
                dialog.Filter = "CSV形式 (*.csv)|*.csv";

                if (dialog.ShowDialog() == true)
                {
                    FilePathStr.Value = dialog.FileName;
                }
                else return;

                using (StreamReader reader = new StreamReader(File.OpenRead(FilePathStr.Value)))
                {
                    reader.ReadLine();
                    var elapsed = new List<long>();
                    var voltage = new List<double>();
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        elapsed.Add(long.Parse(values[2]));
                        voltage.Add(long.Parse(values[3]));
                    }
                    _ElapsedDataList = elapsed;
                    _VoltageDataList = voltage;
                }

            }
            catch (Exception ex)
            {
                Task.Run(async () =>
                {
                    await Services.ContentDialogService.ShowExceptionAsync(_dataOpenPage, ex);
                });
            }
        }
    
    }
}
