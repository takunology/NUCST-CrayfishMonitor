﻿using Microsoft.UI.Xaml.Controls;
using Reactive.Bindings;

namespace CrayfishMonitor_Desktop.Views
{
    public sealed partial class MonitorPage : Page
    {
        ViewModels.MonitorPageViewModel ViewModel { get; }
        public MonitorPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Microsoft.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            ViewModel = new ViewModels.MonitorPageViewModel(this);
        }
    }
}
