using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Wpf
{
    [ContentProperty(nameof(Templates))]
    public class TypeDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplateCollection Templates { get; set; } = new DataTemplateCollection();

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DataTemplate template = null;
            if (!(item is null))
            {
                template = Templates[item.GetType()];
            }
            if (!(template is null))
            {
                return template;
            }
            return base.SelectTemplate(item, container);
        }
    }

    public partial class DataTemplateCollection : IList
    {
        //    public DataTemplate this[int index] { get => templates[index].Template; set => templates[index] = new DataTypeTemplate(value); }
        //    //object? IList.this[int index] { get => this[index]; set => this[index] = (DataTemplate?) value; }

        //    public bool IsSynchronized => false;
        //    public object SyncRoot { get; } = new object();
        //    public bool IsFixedSize { get; }

        //    public int Add(object? value)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public bool Contains(object? value)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public void CopyTo(Array array, int index)
        //    {
        //        for (int i = 0; i < templates.Count; i++, index++)
        //            array.SetValue(templates[i].Template, index);
        //    }

        //    public int IndexOf(object? value)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public void Insert(int index, DataTemplate item)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public void Insert(int index, object? value)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public void Remove(object? value)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public void RemoveAt(int index)
        //    {
        //        throw new NotImplementedException();
        //    }
        public object this[int index] { get => this[index]; set => this[index] = (DataTemplate)value; }

        public bool IsFixedSize { get; } = false;
        public bool IsSynchronized { get; } = false;
        public object SyncRoot { get; } = new object();

        public int Add(object value) => PrivateAdd((DataTemplate)value);

        public bool Contains(object value)
            => (value is DataTemplate template && Contains(template)) ||
               (value is Type type && Contains(type));

        public void CopyTo(Array array, int index)
        {
            for (int i = 0; i < templates.Count; i++, index++)
                array.SetValue(templates[i].Template, index);
        }

        public int IndexOf(object value)
        {
            if (value is DataTemplate template)
                return IndexOf(template);
            if (value is Type type)
                return IndexOf(type);
            return -1;
        }

        public void Insert(int index, object value)
        {
            PrivateAdd((DataTemplate)value);
        }

        public void Remove(object value)
        {
            if (value is DataTemplate template)
                Remove(template);
            if (value is Type type)
                Remove(type);
        }

        public void RemoveAt(int index)
        {
            templates.RemoveAt(index);
        }
    }

    public partial class DataTemplateCollection : IAddChild
    {
        public void AddChild(object value)
        {
            Add((DataTemplate)value);
        }

        public void AddText(string text)
        {
            DataTemplate template = (DataTemplate)XamlReader.Parse(text);
        }
    }
    public partial class DataTemplateCollection
    {
        private readonly struct DataTypeTemplate : IComparable<DataTypeTemplate>
        {
            public Type Type { get; }
            public DataTemplate Template { get; }

            public DataTypeTemplate(DataTemplate template)
            {
                if (!(template.DataType is Type dataType))
                {
                    dataType = typeof(object);
                }

                Type = dataType;
                Template = template;
            }

            public int CompareTo(DataTypeTemplate other)
            {
                return other.Type.IsAssignableFrom(Type)
                    ? 1
                    : Type.IsAssignableFrom(other.Type)
                        ? -1
                        : 0;
            }
        }
    }
    public partial class DataTemplateCollection : ICollection<DataTemplate>
    {
        private readonly List<DataTypeTemplate> templates = new List<DataTypeTemplate>();

        public int Count => ((ICollection<DataTemplate>)templates).Count;

        public bool IsReadOnly => ((ICollection<DataTemplate>)templates).IsReadOnly;

        public void Add(DataTemplate item) => PrivateAdd(item);
        private int PrivateAdd(DataTemplate item)
        {
            if (item is null)
                return -1;

            DataTypeTemplate dataType = new DataTypeTemplate(item);

            int index;
            for (index = 0; index < templates.Count; index++)
            {
                if (dataType.CompareTo(templates[index]) == -1)
                {
                    break;
                }
            }
            if (index < templates.Count && dataType.Type == templates[index].Type)
            {
                templates[index] = dataType;
            }
            else
            {
                templates.Insert(index, dataType);
            }
            return index;
        }

        public void Clear()
        {
            templates.Clear();
        }

        public bool Contains(Type type)
        {
            int index = IndexOf(type);
            return index >= 0;
        }

        public bool Contains(DataTemplate item)
        {
            int index = IndexOf(item);
            return index >= 0;
        }

        public void CopyTo(DataTemplate[] array, int arrayIndex)
        {
            CopyTo((Array)array, arrayIndex);
            //for (int i = 0; i < templates.Count; i++, index++)
            //    array[index] = templates[i].Template;

        }

        public IEnumerator<DataTemplate> GetEnumerator()
        {
            foreach (var item in templates)
            {
                yield return item.Template;
            }
        }

        public int IndexOf(Type type)
        {
            for (int i = 0; i < templates.Count; i++)
            {
                if (templates[i].Type == type)
                    return i;
                if (type.IsAssignableFrom(templates[i].Type))
                    break;
            }
            return -1;
        }
        public int IndexOf(DataTemplate item)
        {
            DataTypeTemplate dataType = new DataTypeTemplate(item);
            int index = IndexOf(dataType.Type);

            if (index >= 0 && dataType.Template == templates[index].Template)
            {
                return index;
            }
            return -1;
        }

        public bool Remove(Type type)
        {
            int index = IndexOf(type);
            if (index < 0)
            {
                return false;
            }
            templates.RemoveAt(index);
            return true;
        }

        public bool Remove(DataTemplate item)
        {
            int index = IndexOf(item);
            if (index >= 0)
            {
                templates.RemoveAt(index);
                return true;
            }
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public DataTemplate this[Type type]
        {
            get
            {
                int index = -1;
                for (int i = 0; i < templates.Count; i++)
                {
                    if (type == templates[i].Type)
                        return templates[i].Template;
                    if (templates[i].Type.IsAssignableFrom(type))
                    {
                        index = i;
                    }
                    if (type.IsAssignableFrom(templates[i].Type))
                    {
                        index = -1;
                        break;
                    }
                }
                if (index >= 0)
                {
                    return templates[index].Template;
                }
                return null;
            }
        }

    }
}
