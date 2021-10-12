using System;
using System.Collections.Generic;
using System.Text;
using Reactive.Bindings;
using System.Reactive.Linq;

namespace WPFApp.ViewModel
{
    public class LoggerControlViewModel
    {
        public ReactiveCommand Save { get; } = new ReactiveCommand();
    }
}
