using System.Windows;
using ViewModels;

namespace WpfApp5
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            //< vm:DeliveriesViewModel x:Key = "vm" />
            //< local:DeliveryToPathConverter x:Key = "deliveryToPath" />
            DeliveriesViewModel vm = (DeliveriesViewModel)FindResource("vm");
            DeliveryToPathConverter deliveryToPath = (DeliveryToPathConverter)FindResource("deliveryToPath");
            deliveryToPath.Deliveries = vm.Deliveries;
        }
    }
}
