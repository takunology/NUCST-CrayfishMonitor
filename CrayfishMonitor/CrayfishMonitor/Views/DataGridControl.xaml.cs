using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CrayfishMonitor.Views
{
    /// <summary>
    /// DataGridControl.xaml の相互作用ロジック
    /// </summary>
    public partial class DataGridControl : UserControl
    {
        public DataGridControl()
        {
            InitializeComponent();
        }

            /*ArduinoDataGrid.Dispatcher.Invoke(() =>
            {
                if (ArduinoDataGrid.Items.Count > 0)
                {
                    ArduinoDataGrid.Dispatcher.Invoke(() =>
                    ArduinoDataGrid.ScrollIntoView(ArduinoDataGrid.Items.GetItemAt(ArduinoDataGrid.Items.Count - 1)));
                }
            });*/
        
    }
}
