using System.Linq;
using System.Threading.Tasks;
using CrayfishMonitor.Models;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Reactive.Bindings;

namespace CrayfishMonitor.ViewModels
{
    public class ArduinoChartViewModel
    {
        public ReactiveProperty<PlotModel> PlotView { get; private set; } = new ReactiveProperty<PlotModel>();

        private PlotModel _plotModel { get; } = new PlotModel(){ Background = OxyColors.White };
        private LineSeries _lineSeries = new LineSeries();

        public ArduinoChartViewModel()
        {
            PlotView.Value = _plotModel.ArduinoChartSetting(_lineSeries);
            DataCollections.ArduinoDatas.CollectionChanged += (s, e) =>
            {
                if (DataCollections.ArduinoDatas.LastOrDefault() != null)
                {
                    var elapsed = DataCollections.ArduinoDatas.Last().Elapsed;
                    var voltage = DataCollections.ArduinoDatas.Last().Voltage;
                    Draw(elapsed, voltage);
                }
                else
                {
                    PlotClear();
                }
            };
        }

        public void Draw(long elapsed, double voltage)
        {
            _lineSeries.Points.Add(new DataPoint(elapsed, voltage));
            // プロット数が 2000 超えたらデキュー
            if (_lineSeries.Points.Count >= 2000)
            {
                _lineSeries.Points.RemoveAt(0);
            }
            if (DataCollections.ArduinoDatas.Count % 10 == 0)
            {
                _plotModel.InvalidatePlot(true);
            }
        }

        private void PlotClear()
        {
            _lineSeries.Points.Clear();
            _plotModel.InvalidatePlot(true);
        }
    }
}
