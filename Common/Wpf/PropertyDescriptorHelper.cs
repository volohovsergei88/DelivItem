using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Wpf
{
    public static partial class PropertyDescriptorHelper
    {
        private static readonly Dictionary<Type, PropertyDescriptorCollection> types = new Dictionary<Type, PropertyDescriptorCollection>();

        private static PropertyDescriptorCollection GetProperties(Type targetType)
        {
            if (!types.TryGetValue(targetType, out var properties))
            {
                properties = TypeDescriptor.GetProperties(targetType);
                types.Add(targetType, properties);
            }
            return properties;
        }

        public static void SetValue<T>(this T target, string propertyName, object value)
            where T : class
        {
            GetProperties(target.GetType())[propertyName].SetValue(target, value);
        }
        public static void SetValue<T>(this T target, PropertyDescriptor propertyDescriptor, object value)
            where T : class
        {
            propertyDescriptor.SetValue(target, value);
        }

        public static object GetValue<T>(this T target, string propertyName)
            where T : class
        {
            return GetProperties(target.GetType())[propertyName].GetValue(target);
        }
        public static object GetValue<T>(this T target, PropertyDescriptor propertyDescriptor)
            where T : class
        {
            return propertyDescriptor.GetValue(target);
        }
    }
    public static partial class PropertyDescriptorHelper
    {
        /// <summary>Присоединение прослушки изменения свойства.</summary>
        /// <typeparam name="T">Тип прослушиваемоего объекта. <see cref="DependencyObject"/> или производный от него.</typeparam>
        /// <param name="source">Прослушиваемый объект.</param>
        /// <param name="dependencyProperty">Прослушиваемое свойство.</param>
        /// <param name="listener">Метод прослушки.</param>
        public static void AddPropertyChanged<T>(this T source, DependencyProperty dependencyProperty, DependencyPropertyChangedHandler listener)
            where T : DependencyObject
        {
            var descriptor = DependencyPropertyDescriptor.FromProperty(dependencyProperty, source.GetType());
            var propertyHandler = AddPropertyHandler(source, listener, dependencyProperty);
            descriptor.AddValueChanged(source, propertyHandler.Raise);
        }

        /// <summary>Отсоединение прослушки изменения свойства.</summary>
        /// <typeparam name="T">Тип прослушиваемоего объекта. <see cref="DependencyObject"/> или производный от него.</typeparam>
        /// <param name="source">Прослушиваемый объект.</param>
        /// <param name="dependencyProperty">Прослушиваемое свойство.</param>
        /// <param name="listener">Метод прослушки.</param>
        public static void RemovePropertyChanged<T>(this T source, DependencyProperty dependencyProperty, DependencyPropertyChangedHandler listener)
            where T : DependencyObject
        {
            var descriptor = DependencyPropertyDescriptor.FromProperty(dependencyProperty, source.GetType());
            var propertyHandler = RemovePropertyHandler(source, listener, dependencyProperty);
            if (propertyHandler != null)
                descriptor.RemoveValueChanged(source, propertyHandler.Raise);
        }

        private static readonly ConditionalWeakTable<DependencyObject, Dictionary<(DependencyPropertyChangedHandler handler, DependencyProperty property), PropertyHandler>> handlers
            = new ConditionalWeakTable<DependencyObject, Dictionary<(DependencyPropertyChangedHandler handler, DependencyProperty property), PropertyHandler>>();

        //private static readonly Dictionary<(DependencyObject source, DependencyPropertyChangedHandler handler, DependencyProperty property), PropertyHandler> handlers = new();

        private static PropertyHandler AddPropertyHandler(DependencyObject source, DependencyPropertyChangedHandler handler, DependencyProperty property)
        {

            if (!handlers.TryGetValue(source, out var properties))
            {
                properties = new Dictionary<(DependencyPropertyChangedHandler handler, DependencyProperty property), PropertyHandler>();
                handlers.Add(source, properties);
            }

            if (properties.TryGetValue((handler, property), out var pHandler))
            {
                pHandler.Count++;
            }
            else
            {
                pHandler = new PropertyHandler(handler, property, source.GetValue(property));
                properties.Add((handler, property), pHandler);
            }
            return pHandler;
        }

        private static PropertyHandler RemovePropertyHandler(DependencyObject source, DependencyPropertyChangedHandler handler, DependencyProperty property)
        {
            PropertyHandler pHandler = null;
            if (handlers.TryGetValue(source, out var properties))
            {
                if (properties.TryGetValue((handler, property), out pHandler))
                {
                    pHandler.Count--;
                    if (pHandler.Count == 0)
                    {
                        properties.Remove((handler, property));
                    }
                    if (properties.Count == 0)
                    {
                        handlers.Remove(source);
                    }
                }
            }

            return pHandler;
        }


        private class PropertyHandler
        {
            public DependencyPropertyChangedHandler Handler { get; }

            public int Count { get; set; }

            public DependencyProperty Property { get; }

            public object OldValue { get; private set; }

            public void Raise(object sender, EventArgs e)
            {
                if (!(sender is DependencyObject dObj))
                {
                    throw new InvalidCastException("sender может быть только DependencyObject.");
                }

                object oldValue = OldValue;
                object newValue = dObj.GetValue(Property);

                Handler(dObj, new DependencyPropertyChangedEventArgs(Property, oldValue, newValue));

                OldValue = newValue;
            }

            public PropertyHandler(DependencyPropertyChangedHandler handler, DependencyProperty property, object oldValue)
            {
                Handler = handler;
                Property = property;
                Count = 1;
                OldValue = oldValue;
            }
        }
    }
    /// <summary>Делегат метода прослушки изменения <see cref="DependencyProperty"/>.</summary>
    /// <param name="sender">Объект в котором изменилось свойство.</param>
    /// <param name="args">Аргументы изменения.</param>
    public delegate void DependencyPropertyChangedHandler(DependencyObject sender, DependencyPropertyChangedEventArgs args);
}
