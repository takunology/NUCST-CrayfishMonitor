using Microsoft.UI.Xaml.Controls;

namespace CrayfishMonitor_Desktop.Views
{
    public sealed partial class DataOpenPage : Page
    {
        public ViewModels.DataOpenPageViewModel ViewModel { get; set; }
        public DataOpenPage()
        {
            this.InitializeComponent();
            ViewModel = new ViewModels.DataOpenPageViewModel(this);
        }
    }
}
