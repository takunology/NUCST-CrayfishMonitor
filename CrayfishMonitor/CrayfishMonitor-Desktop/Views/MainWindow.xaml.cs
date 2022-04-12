using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

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
                _ => frame.Navigate(typeof(Views.MonitorPage))
            };
            if (args.IsSettingsSelected) frame.Navigate(typeof(Views.SettingsPage));
        }
    }
}
