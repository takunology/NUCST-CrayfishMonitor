using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reactive.Bindings;

namespace CrayfishMonitor.Models
{
    public class FFTData
    {
        public double Real { get; set; }
        public double Imaginary { get; set; }
        public double Phase { get; set; }
        public double Magnitude { get; set; }
        public double Frequency { get; set; }
        public double Amplitude { get; set; }
    }
    public static class AnalysisDataCollection
    {
        public static ReactiveCollection<FFTData> AnalysisDatas { get; set; } = new ReactiveCollection<FFTData>();
    }

}
