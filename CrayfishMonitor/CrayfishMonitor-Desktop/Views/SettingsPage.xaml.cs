using Microsoft.UI.Xaml.Controls;
using CrayfishMonitor_Desktop.ViewModels;

namespace CrayfishMonitor_Desktop.Views
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsPageViewModel ViewModel { get; set; }
        public SettingsPage()
        {
            this.InitializeComponent();
            ViewModel = new SettingsPageViewModel();
        }
    }
}
