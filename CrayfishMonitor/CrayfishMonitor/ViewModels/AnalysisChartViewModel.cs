using System.Linq;
using CrayfishMonitor.Models;
using OxyPlot;
using OxyPlot.Series;
using Reactive.Bindings;

namespace CrayfishMonitor.ViewModels
{
    public class AnalysisChartViewModel
    {
        public ReactiveProperty<PlotModel> PlotView { get; private set; } = new ReactiveProperty<PlotModel>();

        private PlotModel _plotModel = new PlotModel() { Background = OxyColors.White };
        private LineSeries _lineSeries = new LineSeries();
        private bool IsChartSetting = false;

        public AnalysisChartViewModel()
        {
            DataCollections.FFTDatas.CollectionChanged += (s, e) => DrowFFTChart();
            DataCollections.DiffDatas.CollectionChanged += (s, e) => DrowDiffChart();
            ControlStatus.IsGetCSVDatas.PropertyChanged += (s, e) => ChartClear();
        }

        private void ChartClear()
        {
            _plotModel = new PlotModel();
            _lineSeries = new LineSeries();
            PlotView.Value = _plotModel;
            IsChartSetting = false;
        }   

        private void DrowFFTChart()
        {
            if (!IsChartSetting)
            {
                PlotView.Value = _plotModel.FFTChartSetting(_lineSeries);
                IsChartSetting = true;
            }

            if (DataCollections.FFTDatas.LastOrDefault() != null)
            {
                var data = DataCollections.FFTDatas.Last();
                _lineSeries.Points.Add(new DataPoint(data.Frequency, data.Amplitude));
            }
            else
            {
                _lineSeries.Points.Clear();
            }
        }

        private void DrowDiffChart()
        {
            if (!IsChartSetting)
            {
                PlotView.Value = _plotModel.DiffChartSetting(_lineSeries);
                IsChartSetting = true;
            }

            if (DataCollections.DiffDatas.LastOrDefault() != null)
            {
                var data = DataCollections.DiffDatas.Last();
                _lineSeries.Points.Add(new DataPoint(data.Time, data.Value));
            }
            else
            {
                _lineSeries.Points.Clear();
            }
        }

    }
}
