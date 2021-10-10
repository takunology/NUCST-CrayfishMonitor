using Reactive.Bindings;
using System.Reactive.Linq;
using System.Linq;
using System.Collections.ObjectModel;
using WPFApp.Model;

namespace WPFApp.ViewModel
{
    public class MainWindowViewModel
    {
        private ObservableCollection<int> Rates { get; }

        public ReadOnlyReactiveCollection<string> DeviceList { get; set; }
        public ReadOnlyReactiveCollection<int> RateList { get; set; }
        public ReactiveProperty<string> SelectedDevice { get; }
        public ReactiveProperty<string> Status { get; set; }

        public ReadOnlyReactiveCollection<DataLogModel> DataLog { get; set; }

        public ReactiveCommand Connect { get; } = new ReactiveCommand();
        public ReactiveCommand DisConnect { get; } = new ReactiveCommand();

        public MainWindowViewModel()
        {
            SubscribeDevicesModel deviceModel = new SubscribeDevicesModel();
            this.DeviceList = deviceModel.Devices.ToReadOnlyReactiveCollection<string>();

            //Rates = new ObservableCollection<int>(new[] { 300, 1200, 2400, 4800, 9600, 19200, 38400, 57600, 74880, 115200, 230400, 250000, 1000000, 2000000});
            
            SelectedDevice = new ReactiveProperty<string>(DeviceList.First());

            this.Connect.Subscribe(() => this.Connection());

            this.DisConnect.Subscribe(() => this.DisConnection());
            
        }

        private void Connection()
        {
            ReceiveDataModel receiveDataModel = new ReceiveDataModel();
            receiveDataModel.Open(SelectedDevice.Value, 9600);
            this.DataLog = receiveDataModel.Data.ToReadOnlyReactiveCollection();
        }

        private void DisConnection()
        {
            ReceiveDataModel receiveDataModel = new ReceiveDataModel();
            receiveDataModel.Close();
        }
    }
}
