using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace CrayfishMonitor.ViewModels
{
    public static class ChartStyle
    {
        public static PlotModel ArduinoChartSetting(this PlotModel plotModel, LineSeries lineSeries)
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

            plotModel.Axes.Add(Axes_x);
            plotModel.Axes.Add(Axes_y);

            lineSeries.StrokeThickness = 1.5;
            lineSeries.Color = OxyColor.FromRgb(0, 100, 205);

            plotModel.Series.Add(lineSeries);

            return plotModel;
        }

        public static PlotModel FFTChartSetting(this PlotModel plotModel, LineSeries lineSeries)
        {
            plotModel = new PlotModel();
            var Axes_x = new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                TickStyle = TickStyle.Inside,
                AbsoluteMinimum = 0,
                Maximum = 100,
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
                Maximum = 0.000005,
                Minimum = 0,
                AbsoluteMinimum = 0,
                TickStyle = TickStyle.Inside,
                MajorGridlineStyle = LineStyle.Automatic,
                MinorGridlineStyle = LineStyle.Dash,
                TitleFontSize = 16,
                Title = "Amplitude"
            };

            plotModel.Axes.Add(Axes_x);
            plotModel.Axes.Add(Axes_y);

            lineSeries.StrokeThickness = 1.5;
            lineSeries.Color = OxyColor.FromRgb(0, 100, 205);

            plotModel.Series.Add(lineSeries);

            return plotModel;
        }

        public static PlotModel DiffChartSetting(this PlotModel plotModel, LineSeries lineSeries)
        {
            plotModel = new PlotModel();
            var Axes_x = new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                TickStyle = TickStyle.Inside,
                AbsoluteMinimum = 0,
                Maximum = 10000,
                MajorGridlineStyle = LineStyle.Automatic,
                MinorGridlineStyle = LineStyle.Dash,
                TitleFontSize = 16,
                Title = "Time [ms]"
            };

            var Axes_y = new LinearAxis()
            {
                Position = AxisPosition.Left,
                MajorTickSize = 10,
                Maximum = 0.1,
                Minimum = -0.1,
                TickStyle = TickStyle.Inside,
                MajorGridlineStyle = LineStyle.Automatic,
                MinorGridlineStyle = LineStyle.Dash,
                TitleFontSize = 16,
                Title = "Differential Value"
            };

            plotModel.Axes.Add(Axes_x);
            plotModel.Axes.Add(Axes_y);

            lineSeries.StrokeThickness = 1.5;
            lineSeries.Color = OxyColor.FromRgb(0, 100, 205);

            plotModel.Series.Add(lineSeries);

            return plotModel;
        }

    }
}
