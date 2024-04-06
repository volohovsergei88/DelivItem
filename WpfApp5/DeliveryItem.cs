using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Wpf;

namespace WpfApp5
{
    public class DeliveryItem : ViewModelBase
    {
        //private string _namedelivery;
        //public DeliveryItem()
        //{

        //}
        public DeliveryItem()
        {

        }
        public DeliveryItem(string namedelivery)
        {
            // DeliveryItems = new ObservableCollection<DeliveryItem>(); // Инициализация DeliveryItems

            Namedelivery = namedelivery;
        }

        //public ObservableCollection<DeliveryItem> DeliveryItems { get; set; }
        //public ObservableCollection<DeliveryItem> DeliveryItems { get; } = new ObservableCollection<DeliveryItem>();

        //public event PropertyChangedEventHandler PropertyChanged;

        public string Namedelivery { get => Get<string>(); set => Set(value ?? string.Empty); }
        //{
        //    get { return _namedelivery; }
        //    set
        //    {
        //        _namedelivery = value;
        //        OnPropertyChanged("Namedelivery");
        //    }
        //}

        //public void OnPropertyChanged([CallerMemberName] string prop = "")
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        //}

        //public void AddItem(List<string> items)
        //{
        //    foreach (var item in items)
        //    {
        //        DeliveryItems.Add(new DeliveryItem ( item));
        //    }

        //}
    }

    public class DeliveriesViewModel : ViewModelBase
    {
        public ObservableCollection<DeliveryItem> Deliveries { get; } = new ObservableCollection<DeliveryItem>();

        public RelayCommand AddDelivery => GetCommand<DeliveryItem>(Deliveries.Add);

        public RelayCommand RemoveDelivery => GetCommand<DeliveryItem>(dlv => Deliveries.Remove(dlv), Deliveries.Contains);
    }
}
