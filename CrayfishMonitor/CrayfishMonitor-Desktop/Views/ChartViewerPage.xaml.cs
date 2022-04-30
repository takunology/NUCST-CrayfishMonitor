using Microsoft.UI.Xaml.Controls;
using CrayfishMonitor_Desktop.ViewModels;

namespace CrayfishMonitor_Desktop.Views
{
    public sealed partial class ChartViewerPage : Page
    {
        public ChartViewerViewModel ViewModel { get; }
        public ChartViewerPage()
        {
            this.InitializeComponent();
            ViewModel = new ChartViewerViewModel(this);
        }
    }
}
