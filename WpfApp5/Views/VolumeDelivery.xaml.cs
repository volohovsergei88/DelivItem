using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
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

namespace WpfApp5
{
    /// <summary>
    /// Interaction logic for VolumeDelivery.xaml
    /// </summary>
    public partial class VolumeDelivery : UserControl
    {
        //public ObservableCollection<DeliveryItem> deliveryItems { get; set; } /*= new ObservableCollection<DeliveryItem>();*/
      
     
        public VolumeDelivery()
        {
            InitializeComponent();
            // DataContext = new DeliveryItem();
           //DataContext = this;
           // deliveryItems = new ObservableCollection<DeliveryItem>();
           // Del.ItemsSource = deliveryItems;
          
        }
        //public void AddItem(List<string> items)
        //{
        //    items.Reverse();
        //    string concatenatedItems = string.Join(", ", items.ToArray());
        //    deliveryItems.Add(new DeliveryItem(concatenatedItems));
        //}


            //    //foreach (var item in items)
            //    //{
            //    //    deliveryItems.Add(new DeliveryItem(item));
            //    //}

            //}
        }
    }
