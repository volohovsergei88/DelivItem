using Dto;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WpfApp5
{
    public class DeliveriesViewSource : CollectionViewSource
    {
        /// <summary>Задаёт идентификатор для которого должны быть получены
        /// из <see cref="CollectionViewSource.Source"/> дочерние элементы.
        /// По умолчанию свойство привязано к свойству Id текущего Контекста Данных.</summary>
        public int DeliveryId
        {
            get { return (int)GetValue(DeliveryIdProperty); }
            set { SetValue(DeliveryIdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DeliveryId.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DeliveryIdProperty =
            DependencyProperty.Register(nameof(DeliveryId),
                                        typeof(int),
                                        typeof(DeliveriesViewSource),
                                        new PropertyMetadata(int.MinValue, OnDeliveryIdChanged));

        /// <summary>Создание фильтра при изменении родительского Id.</summary>
        private static void OnDeliveryIdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DeliveriesViewSource dvs = (DeliveriesViewSource)d;
            dvs.Filter -= filterHandler;
            int id = (int)e.NewValue;
            filterHandler = (s, args) =>
            {
                Delivery dlv = (Delivery)args.Item;
                args.Accepted = dlv.ParentId == id;
            };
            dvs.Filter += filterHandler;
        }

        private static FilterEventHandler filterHandler;

        public DeliveriesViewSource()
        {
            // Создание привязки по умолчанию.
            BindingOperations.SetBinding(this, DeliveryIdProperty, DeliveryIdBinding);
        }
        private static readonly Binding DeliveryIdBinding = new Binding(nameof(Delivery.Id));



        public static IEnumerable<Delivery> GetDeliveriesSource(ItemsControl ic)
        {
            return (IEnumerable<Delivery>)ic.GetValue(DeliveriesSourceProperty);
        }

        public static void SetDeliveriesSource(ItemsControl ic, int value)
        {
            ic.SetValue(DeliveriesSourceProperty, value);
        }

        // Using a DependencyProperty as the backing store for DeliverySource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DeliveriesSourceProperty =
            DependencyProperty.RegisterAttached("DeliveriesSource",
                                                typeof(IEnumerable<Delivery>),
                                                typeof(DeliveriesViewSource),
                                                new PropertyMetadata(null, OnDeliveriesSourceChanged));

        private static void OnDeliveriesSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is ItemsControl ic))
            {
                throw new NotImplementedException("Реализовано только для ItemsControl");
            }

            DeliveriesViewSource deliverieswSource = ic.Resources["childrenSource"] as DeliveriesViewSource;
            if (deliverieswSource is null)
            {
                deliverieswSource = new DeliveriesViewSource();
                ic.Resources["childrenSource"] = deliverieswSource;
            }

            deliverieswSource.Source = e.NewValue;
            Binding binding = new Binding() { Source = deliverieswSource };
            ic.SetBinding(ItemsControl.ItemsSourceProperty, binding);
        }
    }
}
