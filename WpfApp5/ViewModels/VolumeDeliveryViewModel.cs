using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp5.ViewModels
{
    public class VolumeDeliveryViewModel:INotifyPropertyChanged
    {
        private ObservableCollection<Delivery> _delivery;
        public ObservableCollection<Delivery> Delivery
        {
            get { return _delivery; }
            set
            {
                _delivery = value;
                OnPropertyChanged();
            }
        }
        public VolumeDeliveryViewModel()
        {
            Delivery = new ObservableCollection<Delivery>();
            

        }
        public void AddItem(List<string> items)
        {
            items.Reverse();
            string concatenatedItems = string.Join(", ", items.ToArray());
            Delivery.Add(new Delivery(concatenatedItems));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
