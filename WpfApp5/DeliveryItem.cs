using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp5
{
    public class DeliveryItem : INotifyPropertyChanged
    {
        private string _namedelivery;
        //public DeliveryItem()
        //{

        //}
        public DeliveryItem()
        {
                
        }
        public DeliveryItem(string _namedelivery)
        {
            // DeliveryItems = new ObservableCollection<DeliveryItem>(); // Инициализация DeliveryItems
           
            Namedelivery= _namedelivery;
        }

        //public ObservableCollection<DeliveryItem> DeliveryItems { get; set; }
        //public ObservableCollection<DeliveryItem> DeliveryItems { get; } = new ObservableCollection<DeliveryItem>();

        public event PropertyChangedEventHandler PropertyChanged;

        public string Namedelivery
        {
            get { return _namedelivery; }
            set
            {
                _namedelivery = value;
                OnPropertyChanged("Namedelivery");
            }
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        //public void AddItem(List<string> items)
        //{
        //    foreach (var item in items)
        //    {
        //        DeliveryItems.Add(new DeliveryItem ( item));
        //    }
            
        //}
    }
}
