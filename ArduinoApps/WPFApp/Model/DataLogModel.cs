using System;
using System.Collections.Generic;
using System.Text;

namespace WPFApp.Model
{
    public class DataLogModel
    {
        public DateTime Date { get; set; }
        public DateTime Time {  get; set; }
        public DateTime MiliSec {  get; set; }
        public int AnalogData { get; set; }
        public double DigitalData { get; set; }
    }
}
