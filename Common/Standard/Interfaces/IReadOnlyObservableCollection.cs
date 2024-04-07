using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;

namespace Common.Standard.Interfaces
{
    public interface IReadOnlyObservableCollection<T> :
        IEnumerable<T>,
        IEnumerable,
        IReadOnlyCollection<T>,
        IReadOnlyList<T>,
     //   ICollection,
       // IList,
        INotifyCollectionChanged,
        INotifyPropertyChanged
    {
        int IndexOf(object value);
        void Remove(object value);
    }

    public class ReadOnlyObservableList<T, Tsource> : IReadOnlyObservableCollection<T>
      //  where T : class
        where Tsource : T
    {
        private readonly ObservableCollection<Tsource> obslist;
        private readonly IList list;
        private readonly ICollection collection;
        private readonly INotifyPropertyChanged notifyyPropertyChanged;
        public ReadOnlyObservableList(ObservableCollection<Tsource> list)
        {
            if(list is null) throw new ArgumentNullException(nameof(list));

            obslist = list;
            this.list = list;
            collection = list;
            notifyyPropertyChanged = list;
        }

        public T this[int index] => obslist[index];
        //    object? IList.this[int index] { get => ((IReadOnlyList<T>)obslist)[index]; set => throw new NotImplementedException(); }
        public int IndexOf(object value) => list.IndexOf(value);


        public int Count => obslist.Count;
        public bool IsFixedSize => true; //list.IsFixedSize;
        public bool IsReadOnly => true;//list.IsReadOnly;
        public bool IsSynchronized => collection.IsSynchronized;
        public object SyncRoot => collection.SyncRoot;

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add => obslist.CollectionChanged += value;
            remove => obslist.CollectionChanged -= value;
        }

        public event PropertyChangedEventHandler PropertyChanged
        {
            add => notifyyPropertyChanged.PropertyChanged += value;
            remove => notifyyPropertyChanged.PropertyChanged -= value;
        }

        public int Add(object value)
        {
            list.Add(value);
            return obslist.Count;
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(object value) => list.Contains(value);

         public void CopyTo(Array array, int index) => collection.CopyTo(array, index);

        public IEnumerator<T> GetEnumerator()
        {
            foreach (Tsource item in obslist)
            {
                yield return item;
            }
        }


        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public void Remove(object value) => list.Remove(value);

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
