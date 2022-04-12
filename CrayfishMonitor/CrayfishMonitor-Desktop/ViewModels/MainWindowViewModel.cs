using Microsoft.UI.Xaml.Controls;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CrayfishMonitor_Desktop.ViewModels
{
    public class MainWindowViewModel
    {
        public ReactivePropertySlim<Page> PageContent { get; set; } = new ReactivePropertySlim<Page>(new Views.MonitorPage());

        public MainWindowViewModel()
        {

        }
    }
}
