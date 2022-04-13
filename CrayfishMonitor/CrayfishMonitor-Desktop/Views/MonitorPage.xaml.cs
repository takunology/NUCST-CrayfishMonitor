using Microsoft.UI.Xaml.Controls;
using Reactive.Bindings;

namespace CrayfishMonitor_Desktop.Views
{
    public sealed partial class MonitorPage : Page
    {
        ViewModels.MonitorPageViewModel ViewModel { get; } = new ViewModels.MonitorPageViewModel();
        public MonitorPage()
        {
            this.InitializeComponent();
        }
    }
}
