using System.Linq;
using System.Threading.Tasks;
using CrayfishMonitor.Models;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Reactive.Bindings;

namespace CrayfishMonitor.ViewModels
{
    public class ChartControlViewModel
    {
        public ReactiveProperty<PlotModel> PlotView { get; private set; } = new ReactiveProperty<PlotModel>();

        private PlotModel _PlotModel { get; } = new PlotModel(){ Background = OxyColors.White };
        private LineSeries _LineSeries = new LineSeries();

        public ChartControlViewModel()
        {
            GraphSetup();
            Task.Run(() =>
            {
                ArduinoDataCollection.ArduinoDatas.CollectionChanged += (s, e) =>
                {
                    if (ArduinoDataCollection.ArduinoDatas.LastOrDefault() != null)
                    {
                        var elapsed = ArduinoDataCollection.ArduinoDatas.LastOrDefault().Elapsed;
                        var voltage = ArduinoDataCollection.ArduinoDatas.LastOrDefault().Voltage;
                        Draw(elapsed, voltage);
                    }
                    else
                    {
                        PlotClear();
                    }
                };
            });
        }

        public void Draw(long elapsed, double voltage)
        {
            _LineSeries.Points.Add(new DataPoint(elapsed, voltage));
            // プロット数が 2000 超えたらデキュー
            if (_LineSeries.Points.Count >= 2000)
            {
                _LineSeries.Points.RemoveAt(0);
            }
            if (ArduinoDataCollection.ArduinoDatas.Count % 10 == 0)
            {
                _PlotModel.InvalidatePlot(true);
            }
        }

        private void GraphSetup()
        {
            var Axes_x = new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                TickStyle = TickStyle.Inside,
                AbsoluteMinimum = 0,
                MajorGridlineStyle = LineStyle.Automatic,
                MinorGridlineStyle = LineStyle.Dash,
                TitleFontSize = 16,
                Title = "Time [ms]"
            };

            var Axes_y = new LinearAxis()
            {
                Position = AxisPosition.Left,
                MajorTickSize = 10,
                Maximum = 5,
                Minimum = 0,
                TickStyle = TickStyle.Inside,
                AbsoluteMinimum = 0,
                MajorGridlineStyle = LineStyle.Automatic,
                MinorGridlineStyle = LineStyle.Dash,
                TitleFontSize = 16,
                Title = "Voltage [V]"
            };

            _PlotModel.Axes.Add(Axes_x);
            _PlotModel.Axes.Add(Axes_y);

            _LineSeries.StrokeThickness = 1.5;
            _LineSeries.Color = OxyColor.FromRgb(0, 100, 205);

            _PlotModel.Series.Add(_LineSeries);
            PlotView.Value = _PlotModel;
        }

        private void PlotClear()
        {
            _LineSeries.Points.Clear();
            _PlotModel.InvalidatePlot(true);
        }
    }
}
