using System;
using System.Collections.Generic;
using System.Text;

namespace WPFApp.Model
{
    public class DataLogModel
    {
        public DateTime Date { get; set; }
        public TimeSpan Time {  get; set; }
        public int MiliSec {  get; set; }
        //public int AnalogData { get; set; }
        public double DigitalData { get; set; }
    }
}
