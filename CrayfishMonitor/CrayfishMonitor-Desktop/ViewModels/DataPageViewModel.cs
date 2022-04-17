using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrayfishMonitor_Desktop.Models;
using Reactive.Bindings;

namespace CrayfishMonitor_Desktop.ViewModels
{
    public class DataPageViewModel
    {
        public List<MeasurementSaveData> SaveDataItems { get; private set; } = DataCollections.SaveDataList;
        public DataPageViewModel()
        {
             
        }
    }
}
