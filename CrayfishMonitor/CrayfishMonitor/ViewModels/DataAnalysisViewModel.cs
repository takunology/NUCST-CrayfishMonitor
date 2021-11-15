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
    public class DataAnalysisViewModel
    {
        public ReactivePropertySlim<string> FilePathContent { get; private set; } = new ReactivePropertySlim<string>("未選択");
        public ReactivePropertySlim<bool> IsEnableFFT { get; private set; } = new ReactivePropertySlim<bool>(false);

        public ReactiveCommand DataLoadFromLocalCommand { get; private set; } = new ReactiveCommand();
        public ReactiveCommand DataLoadFromCSVCommand { get; private set; } = new ReactiveCommand();
        public ReactiveCommand FourierCommand { get; set; } = new ReactiveCommand();

        //グラフプロット処理（ここはいつか分離したい）
        public ReactiveProperty<PlotModel> PlotView { get; private set; } = new ReactiveProperty<PlotModel>();

        private PlotModel _PlotModel { get; } = new PlotModel() { Background = OxyColors.White };
        private StemSeries _StemSeries = new StemSeries();
        private LineSeries _LineSeries = new LineSeries();

        private double _sampling = 200; //1秒間に何回データ測定できるか
        private List<double> VoltageDataSource = new List<double>(); 

        public DataAnalysisViewModel()
        {
            GraphSetup();
            DataLoadFromLocalCommand.Subscribe(_ => DataLoadFromLocal());
            DataLoadFromCSVCommand.Subscribe(_ => Task.Run(() => DataLoadFromCSV()));
            FourierCommand.Subscribe(_ => Task.Run(() => FFT()));
        }

        private void DataLoadFromLocal()
        {
            try
            {
                VoltageDataSource = ArduinoDataCollection.ArduinoDatas.Select(x => x.Voltage).ToList();
                FilePathContent.Value = "取得した測定データを解析対象にします。";
                IsEnableFFT.Value = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"データの参照に失敗しました。\n詳細:\n{ex}", "データ参照エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void DataLoadFromCSV()
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
                    var Voltage = new List<double>();
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        Voltage.Add(double.Parse(values[3]));
                    }
                    VoltageDataSource = Voltage;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"ファイル読み込み時にエラーが発生しました。\n詳細:\n{ex}", "ファイルオープンエラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void FFT()
        {
            AnalysisDataCollection.AnalysisDatas.Clear();
            PlotClear();

            var complex = new Complex[VoltageDataSource.Count];
            complex = VoltageDataSource.Select(x => new Complex(x, 0.0)).ToArray();    
            
            // フーリエ変換の実部、虚部、位相、絶対値を取得
            Fourier.Forward(complex, FourierOptions.Default);
            
            // 周波数変換も行う
            for(int i = 0; i < complex.Length; i++)
            {
                AnalysisDataCollection.AnalysisDatas.Add(new FFTData()
                {
                    Real = complex[i].Real,
                    Imaginary = complex[i].Imaginary,
                    Phase = complex[i].Phase,
                    Magnitude = complex[i].Magnitude,
                    //周波数 = 次数 * サンプリング周波数 / データ数 (2^N)
                    Frequency = i * _sampling / VoltageDataSource.Count,
                    Amplitude = complex[i].Magnitude / (VoltageDataSource.Count / 2)
                });
            }
            MessageBox.Show("解析が完了しました。", "", MessageBoxButton.OK, MessageBoxImage.Information);
            IsEnableFFT.Value = false;
            Draw();
        }

        public void Draw()
        {
            foreach (var data in AnalysisDataCollection.AnalysisDatas)
            {
                _StemSeries.Points.Add(new DataPoint(data.Frequency, data.Amplitude));
                _LineSeries.Points.Add(new DataPoint(data.Frequency, data.Amplitude));
            }
            _PlotModel.InvalidatePlot(true);
        }

        private void GraphSetup()
        {
            var Axes_x = new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                TickStyle = TickStyle.Inside,
                AbsoluteMinimum = 0,
                Maximum = 10,
                Minimum = 0,
                MajorGridlineStyle = LineStyle.Automatic,
                MinorGridlineStyle = LineStyle.Dash,
                TitleFontSize = 16,
                Title = "Frequency [Hz]"
            };

            var Axes_y = new LinearAxis()
            {
                Position = AxisPosition.Left,
                MajorTickSize = 10,
                Maximum = 0.00005,
                Minimum = 0,
                AbsoluteMinimum = 0,
                TickStyle = TickStyle.Inside,
                MajorGridlineStyle = LineStyle.Automatic,
                MinorGridlineStyle = LineStyle.Dash,
                TitleFontSize = 16,
                Title = "Amplitude"
            };

            _PlotModel.Axes.Add(Axes_x);
            _PlotModel.Axes.Add(Axes_y);

            //棒グラフ
            _StemSeries.StrokeThickness = 2;
            _StemSeries.Color = OxyColor.FromRgb(0, 180, 50);

            //折れ線グラフ
            _LineSeries.StrokeThickness = 1.5;
            _LineSeries.Color = OxyColor.FromRgb(0, 100, 205);

            _PlotModel.Series.Add(_LineSeries);
            _PlotModel.Series.Add(_StemSeries);
            PlotView.Value = _PlotModel;
        }

        private void PlotClear()
        {
            _StemSeries.Points.Clear();
            _LineSeries.Points.Clear();
            _PlotModel.InvalidatePlot(true);
        }
    }
}
