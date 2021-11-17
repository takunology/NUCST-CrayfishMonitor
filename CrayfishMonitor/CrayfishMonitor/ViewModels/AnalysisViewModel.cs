using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.IntegralTransforms;
using Reactive.Bindings;
using CrayfishMonitor.Models;
using System.Numerics;
using System.Windows;
using System.Collections.ObjectModel;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using Microsoft.Win32;
using System.IO;

namespace CrayfishMonitor.ViewModels
{
    public class AnalysisViewModel
    {
        public ReactivePropertySlim<string> FilePathContent { get; private set; } = new ReactivePropertySlim<string>("データが参照されていません");
        public ReactivePropertySlim<bool> IsEnableAnalysis { get; private set; } = new ReactivePropertySlim<bool>(false);

        public ReactiveCommand DataLoadFromLocalCommand { get; private set; } = new ReactiveCommand();
        public ReactiveCommand DataLoadFromCSVCommand { get; private set; } = new ReactiveCommand();
        public ReactiveCommand FourierCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand DifferentialCommand { get; set; } = new ReactiveCommand();

        private double _sampling = 200; //サンプリング数
        private List<double> _voltageDataSource = new List<double>();
        private List<long> _elapsedDataSource = new List<long>();
        //private List<ArduinoData> _arduinoDataList = new List<ArduinoData>();

        public AnalysisViewModel()
        {
            DataLoadFromLocalCommand.Subscribe(_ => DataLoadFromLocal());
            DataLoadFromCSVCommand.Subscribe(_ => Task.Run(() => DataLoadFromCSV()));
            FourierCommand.Subscribe(_ => Task.Run(() => FFT()));
            DifferentialCommand.Subscribe(_ => Task.Run(() => Differencial()));
        }

        private void DataLoadFromLocal()
        {
            try
            {
                _elapsedDataSource = DataCollections.ArduinoDatas.Select(x => x.Elapsed).ToList();
                _voltageDataSource = DataCollections.ArduinoDatas.Select(x => x.Voltage).ToList();

                //_arduinoDataList = DataCollections.ArduinoDatas.Select(x => x).ToList();

                FilePathContent.Value = "取得した測定データを解析対象にします。";
                IsEnableAnalysis.Value = true;
                ControlStatus.IsGetCSVDatas.Value--;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"データの参照に失敗しました。\n詳細:\n{ex}", "データ参照エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DataLoadFromCSV()
        {
            try
            {
                var dialog = new OpenFileDialog();
                dialog.Filter = "CSVファイル (*.csv)|*.csv";

                if (dialog.ShowDialog() == true)
                {
                    FilePathContent.Value = (dialog.FileName);
                }
                else return;
                //ここからファイルの中身を取得する処理
                using (StreamReader reader = new StreamReader(File.OpenRead(FilePathContent.Value)))
                {
                    reader.ReadLine(); //1行目は項目なので捨てる
                    var elapsed = new List<long>();
                    var voltage = new List<double>();
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        elapsed.Add(long.Parse(values[2]));
                        voltage.Add(double.Parse(values[3]));
                    }
                    _voltageDataSource = voltage;
                    _elapsedDataSource = elapsed;
                }
                IsEnableAnalysis.Value = true;
                ControlStatus.IsGetCSVDatas.Value++;
            }
            catch(Exception ex)
            {
                MessageBox.Show($"ファイル読み込み時にエラーが発生しました。\n詳細:\n{ex}", "ファイルオープンエラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // log_2 Nを取得し、2^Nを返す
        private int GetExpDatas(int dataCount)
        {
            var val = Math.Round(Math.Log2(dataCount));
            //MessageBox.Show($"2の{val}乗={Math.Pow(2, val)}");
            return (int)(Math.Pow(2, val));
        }

        private void FFT()
        {
            if(DataCollections.FFTDatas.LastOrDefault() != null)
            {
                DataCollections.FFTDatas.Clear();
            }

            var dataCount = GetExpDatas(_voltageDataSource.Count);
            // 不足データを0で埋めるとノイズがひどい
            var complex = new Complex[_voltageDataSource.Count];
            complex = _voltageDataSource.Select(x => new Complex(x, 0.0)).ToArray();
            // フーリエ変換の実部、虚部、位相、絶対値を取得
            Fourier.Forward(complex, FourierOptions.Default);
            // 周波数変換も行う
            for (int i = 0; i < complex.Length; i++)
            {
                DataCollections.FFTDatas.Add(new FFTData()
                {
                    Real = complex[i].Real,
                    Imaginary = complex[i].Imaginary,
                    Phase = complex[i].Phase,
                    Magnitude = complex[i].Magnitude,
                    //周波数 = 次数 * サンプリング周波数 / データ数 (2^N)
                    Frequency = i * _sampling / dataCount,
                    Amplitude = complex[i].Magnitude / (dataCount / 2)
                    //Frequency = i * _sampling / VoltageDataSource.Count,
                    //Amplitude = complex[i].Magnitude / (VoltageDataSource.Count / 2)
                });
            }
            MessageBox.Show("データを変換しました。", "", MessageBoxButton.OK, MessageBoxImage.Information);
            IsEnableAnalysis.Value = false;
        }

        private void Differencial()
        {
            if (DataCollections.DiffDatas.LastOrDefault() != null)
            {
                DataCollections.DiffDatas.Clear();
            }

            for (int i = 0; i < _elapsedDataSource.Count; i++)
            {
                if (i == _elapsedDataSource.Count - 1) break;
                
                var dx = _elapsedDataSource[i + 1] - _elapsedDataSource[i];
                var dy = _voltageDataSource[i + 1] - _voltageDataSource[i];
                
                DataCollections.DiffDatas.Add(new DiffData()
                {
                    Time = _elapsedDataSource[i],
                    Value = dy / dx
                });
            }

            MessageBox.Show("データを変換しました。", "", MessageBoxButton.OK, MessageBoxImage.Information);
            IsEnableAnalysis.Value = false;
        }
    }
}
