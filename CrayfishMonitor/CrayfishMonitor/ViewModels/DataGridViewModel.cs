using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using CrayfishMonitor.Models;
using Reactive.Bindings;

namespace CrayfishMonitor.ViewModels
{  
    public class DataGridViewModel
    {
        public ReactiveCollection<ArduinoData> ArduinoDatas { get; private set; } = new ReactiveCollection<ArduinoData>();

        public DataGridViewModel()
        {
            BindingOperations.EnableCollectionSynchronization(ArduinoDatas, new object());
            
            Task.Run(() =>
            {
                ArduinoDataCollection.ArduinoDatas.CollectionChanged += (s, e) =>
                {
                    if (ArduinoDataCollection.ArduinoDatas.LastOrDefault() != null)
                    {
                        var data = ArduinoDataCollection.ArduinoDatas.LastOrDefault();
                        ArduinoDatas.Add(data);
                    }
                    else if(ArduinoDataCollection.ArduinoDatas.LastOrDefault() == null)
                    {
                        ArduinoDatas.Clear();
                    }
                };
            });
        }

    }
}
