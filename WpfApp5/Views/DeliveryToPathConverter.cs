using Dto;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace WpfApp5
{
    /// <summary>Получает путь к элементу из коллекции <see cref="Deliveries"/>.
    /// Путь возвращается включая сам элемент.</summary>
    [ValueConversion(typeof(Delivery), typeof(string))]
    public class DeliveryToPathConverter : IValueConverter
    {
        /// <summary>Коллекция из которой нужно получить путь к элементу.</summary>
        public IEnumerable<Delivery> Deliveries { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Delivery? curr = value as Delivery?;
            var path = Enumerable.Empty<Delivery>();
            while (curr is Delivery delivery && delivery.Id > 0)
            {
                path = path.Prepend(delivery);
                curr = Deliveries.FirstOrDefault(dlv => dlv.Id == delivery.ParentId);
            }

            return string.Join("; ", path.Select(dlv => dlv.NameDelivery));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
