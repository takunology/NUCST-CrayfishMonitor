using CrayfishMonitor_Desktop.Models;
using CrayfishMonitor_Desktop.Services;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using OxyPlot;
using OxyPlot.Series;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;


namespace CrayfishMonitor_Desktop.ViewModels
{
    public class MeasurementChartControlViewModel
    {
        public PlotModel MeasurementChart { get; private set; }

        private PlotModel _plotModel = new PlotModel();
        private LineSeries _lineSeries = new LineSeries();
        // 別スレッドをキューに格納する
        DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();


        public MeasurementChartControlViewModel()
        {
            MeasurementChart = _plotModel.MeasurementStyle(_lineSeries);
            DataCollections.Measurements.CollectionChanged += (s, e) =>
            {
                if (DataCollections.Measurements.Count > 0)
                {
                    var elapsed = DataCollections.Measurements.Last().Elapsed;
                    var voltage = DataCollections.Measurements.Last().Voltage;
                    DrawChart(elapsed, voltage);
                }
                else
                {
                    CrearChart();
                }
            };
        }

        private void DrawChart(long dataX, double dataY)
        {
            _lineSeries.Points.Add(new DataPoint(dataX, dataY));
            if (_lineSeries.Points.Count > 1000)
            {
                _lineSeries.Points.RemoveAt(0);
            }
            if (DataCollections.Measurements.Count % 10 == 0)
            {
                dispatcherQueue.TryEnqueue(() => _plotModel.InvalidatePlot(true));
            }
        }

        private void CrearChart()
        {
            _lineSeries.Points.Clear();
            dispatcherQueue.TryEnqueue(() => _plotModel.InvalidatePlot(true));
        }
    }
}
