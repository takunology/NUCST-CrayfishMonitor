using CrayfishMonitor_Desktop.Models;
using CrayfishMonitor_Desktop.Services;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using OxyPlot;
using OxyPlot.Series;
using Reactive.Bindings;

namespace CrayfishMonitor_Desktop.ViewModels
{
    public class ChartViewerViewModel : Page
    {
        public PlotModel ChartModel { get; private set; }
        public ReactiveCommand BackCommand { get; } = new ReactiveCommand();

        private PlotModel _plotModel = new PlotModel();
        private LineSeries _lineSeries = new LineSeries();
        private Views.ChartViewerPage _chartViewPage;

        public ChartViewerViewModel(Views.ChartViewerPage chartViewPage)
        {
            this._chartViewPage = chartViewPage;

            BackCommand.Subscribe(() => BackToDataPage());

            ChartModel = _plotModel.DataViewStyle(_lineSeries);
            var savedata = DataCollections.SaveDataList[DataCollections.SelectedListIndex];
            if (savedata.Measurements.Count > 0)
            {
                foreach (var measurement in savedata.Measurements)
                {
                    _lineSeries.Points.Add(new DataPoint((double)measurement.Elapsed, measurement.Voltage));
                }
                _plotModel.InvalidatePlot(true);
            }
        }

        private void BackToDataPage()
        {
            this._chartViewPage.Frame.Navigate(typeof(Views.DataPage), null, new SlideNavigationTransitionInfo()
            {
                Effect = SlideNavigationTransitionEffect.FromLeft
            });
        }        
    }
}
