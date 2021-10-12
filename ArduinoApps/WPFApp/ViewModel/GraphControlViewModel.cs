using System;
using System.Collections.Generic;
using System.Text;
using Reactive.Bindings;
using System.Reactive.Linq;

namespace WPFApp.ViewModel
{
    public class GraphControlViewModel
    {
        public ReactiveCommand Save { get; } = new ReactiveCommand();

    }
}
