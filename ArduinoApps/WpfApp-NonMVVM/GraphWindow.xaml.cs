using System.Collections.ObjectModel;
using System.Windows;
using OxyPlot;
using OxyPlot.Wpf;
using OxyPlot.Series;
using OxyPlot.Axes;
using System.IO;
using System.Windows.Media.Imaging;
using System;
using Microsoft.Win32;
using System.Windows.Media;

namespace WpfApp_NonMVVM
{
    /// <summary>
    /// Interaction logic for GraphWindow.xaml
    /// </summary>
    public partial class GraphWindow : Window
    {
        PlotModel plotModel { get; } = new PlotModel();
        LineSeries lineSeries = new LineSeries();
        public ObservableCollection<Data> Datas = new ObservableCollection<Data>();
        public GraphWindow(Window window, ObservableCollection<Data> Datas)
        {
            InitializeComponent();
            this.Datas = Datas;
            GraphSetup();
            DrawGraph();
        }
        private void DrawGraph()
        {
            for(int i = 0; i < Datas.Count; i++)
            {
                lineSeries.Points.Add(new DataPoint(Datas[i].Elapsed, Datas[i].Voltage));
            }  
            plotModel.InvalidatePlot(true);
        }
        private void GraphSetup()
        {
            var Axes_x = new OxyPlot.Axes.LinearAxis()
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
                TickStyle = TickStyle.Inside,
                AbsoluteMinimum = 0,
                MajorGridlineStyle = LineStyle.Automatic,
                MinorGridlineStyle = LineStyle.Dash,
                TitleFontSize = 16,
                Title = "Voltage [V]"
            };

            plotModel.Axes.Add(Axes_x);
            plotModel.Axes.Add(Axes_y);

            lineSeries.MarkerSize = 2;
            lineSeries.MarkerType = MarkerType.Circle;
            lineSeries.StrokeThickness = 1;
            lineSeries.MarkerFill = OxyColor.FromRgb(0, 0, 225);
            lineSeries.Color = OxyColor.FromRgb(0, 0, 225);

            plotModel.Series.Add(lineSeries);
            GraphView.Model = plotModel;
        }
        private void GraphSave_Button(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog
            {
                Filter = "PNG形式|*.png",
                DefaultExt = ".png"
            };
            if (dlg.ShowDialog(this).Value)
            {
                var ext = Path.GetExtension(dlg.FileName).ToLower();
                switch (ext)
                {
                    case ".png":
                        GraphView.SaveBitmap(dlg.FileName, 0, 0);
                        break;
                }
            }
        }
    }
}
