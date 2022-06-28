using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace CrayfishMonitor_Desktop.Views
{
    public sealed partial class MainWindow : Window
    {
        ViewModels.MainWindowViewModel ViewModel { get; } = new ViewModels.MainWindowViewModel();
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void OnSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            _ = args.SelectedItemContainer.Tag.ToString() switch
            {
                "MonitorPage" => frame.Navigate(typeof(Views.MonitorPage)),
                "DataPage" => frame.Navigate(typeof(Views.DataPage)),
                "DataOpenPage" => frame.Navigate(typeof(Views.DataOpenPage)),
                _ => frame.Navigate(typeof(Views.MonitorPage))
            };
            if (args.IsSettingsSelected) frame.Navigate(typeof(Views.SettingsPage));
        }
    }
}
