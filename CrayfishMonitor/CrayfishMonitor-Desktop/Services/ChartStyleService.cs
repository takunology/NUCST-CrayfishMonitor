using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrayfishMonitor_Desktop.Services
{
    public static class ChartStyleService
    {
        public static PlotModel MeasurementStyle(this PlotModel plotModel, LineSeries lineSeries)
        {
            var axisX = new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                TickStyle = TickStyle.Inside,
                AbsoluteMinimum = 0,
                MajorGridlineStyle = LineStyle.Automatic,
                MinorGridlineStyle = LineStyle.Dash,
                TitleFontSize = 16,
                Title = "Time [s]"
            };

            var axisY = new LinearAxis()
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

            plotModel.Axes.Add(axisX);
            plotModel.Axes.Add(axisY);

            lineSeries.StrokeThickness = 1.5;
            lineSeries.Color = OxyColor.FromRgb(0, 100, 205);

            plotModel.Series.Add(lineSeries);

            return plotModel;
        }

        public static PlotModel DataViewStyle(this PlotModel plotModel, LineSeries lineSeries)
        {
            var axisX = new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                TickStyle = TickStyle.Inside,
                AbsoluteMinimum = 0,
                MajorGridlineStyle = LineStyle.Automatic,
                MinorGridlineStyle = LineStyle.Dash,
                TitleFontSize = 16,
                Title = "Time [s]"
            };

            var axisY = new LinearAxis()
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

            plotModel.Axes.Add(axisX);
            plotModel.Axes.Add(axisY);

            lineSeries.StrokeThickness = 1.5;
            lineSeries.Color = OxyColor.FromRgb(0, 100, 205);

            plotModel.Series.Add(lineSeries);

            return plotModel;
        }
    }
}
