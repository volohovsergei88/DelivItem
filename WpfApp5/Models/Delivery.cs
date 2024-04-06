using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Controls;
using Wpf;

namespace WpfApp5
{
    public class Delivery : ViewModelBase, IEnumerable<Delivery>
    {
        public Delivery()
        {
            Children.CollectionChanged += OnChildrenChanged;
            PropertyChanged += OnDeliveryChanged;
        }

        private void OnDeliveryChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is nameof(Parent))
            {
                foreach (Delivery item in Children)
                {
                    item.RaisePropertyChanged(ParentChangedEventArgs);
                }
            }
        }

        private static readonly PropertyChangedEventArgs ParentChangedEventArgs = new PropertyChangedEventArgs(nameof(Parent));

        private void OnChildrenChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Reset:
                    foreach (Delivery item in e.OldItems ?? Array.Empty<Delivery>())
                    {
                        item.Parent = null;
                    }
                    foreach (Delivery item in e.NewItems ?? Array.Empty<Delivery>())
                    {
                        if (item.Parent is null)
                        {
                            item.Parent = this;
                        }
                        else
                        {
                            throw new Exception($"Этот Delivery {{{item.Namedelivery}}} принадлежит {{{item.Parent}}}");
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                default:
                    break;
            }
        }

        public IEnumerator<Delivery> GetEnumerator() => Children.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public Delivery(string namedelivery)
            : this()
        {
            Namedelivery = namedelivery;
        }

        public string Namedelivery { get => Get<string>(); set => Set(value ?? string.Empty); }

        public ObservableCollection<Delivery> Children { get; } = new ObservableCollection<Delivery>();

        public Delivery Parent { get => Get<Delivery>(); private set => Set(value); }

        public IEnumerable<Delivery> Path
        {
            get
            {
                Delivery current = this;
                while (!(current is null))
                {
                    yield return current;
                    current = current.Parent;
                }
            }
        }
    }

    public class DeliveriesViewModel : ViewModelBase
    {
        public Delivery MainDelivery { get; } = new Delivery();


        string[] levelClaster = { "Поставка ПО", "Обучение", "Моделирование" };
        string[] levelLearn = { "Группа (до 5 чел.)", "Группа (более 5 чел.)", "Индивидуально" };
        string[] levelModels = { "Поверхность", "Сети", "Полный комплект" };
        string[] typeDeliver = { "Новая поставка", "Обновление" };
        string[] levelProduct = { "ПС Профиль" };
        string[] levelMdl = { "Сети НВК", "Теслоснабжение", "Газ", "Кабели", "Все сети" };
        string[] levelTimeAll = { "12", "24" };
        string[] levelTime = { "12", "24", "1", "3", "6" };


        public DeliveriesViewModel()
        {
            for (int i = 0; i < levelClaster.Length; i++)
            {
                Delivery GL = new Delivery(levelClaster[i]);

                if (levelClaster[i] == "Обучение")
                {
                    foreach (string ln in levelLearn)
                    {
                        Delivery learnMenuItem = new Delivery(ln);
                        GL.Children.Add(learnMenuItem);
                    }
                }
                else if (levelClaster[i] == "Моделирование")
                {
                    foreach (string ln in levelModels)
                    {
                        Delivery modelMenuItem = new Delivery(ln);
                        GL.Children.Add(modelMenuItem);
                    }
                }

                else if (levelClaster[i] == "Поставка ПО")
                {
                    Delivery postavMenuItem = new Delivery(levelClaster[i]);

                    foreach (string item in typeDeliver)
                    {
                        Delivery typeMenuItem = new Delivery(item);

                        foreach (string mdl in levelMdl)
                        {
                            Delivery mdlMenuItem = new Delivery(mdl);

                            if (mdl == "Все сети")
                            {
                                foreach (string time in levelTime)
                                {
                                    Delivery timeMenuItem = new Delivery(time);
                                    mdlMenuItem.Children.Add(timeMenuItem);
                                }
                            }
                            else
                            {
                                foreach (string timeAll in levelTimeAll)
                                {
                                    Delivery timeAllMenuItem = new Delivery(timeAll);


                                    mdlMenuItem.Children.Add(timeAllMenuItem);
                                }
                            }
                            typeMenuItem.Children.Add(mdlMenuItem);
                        }
                        postavMenuItem.Children.Add(typeMenuItem);
                    }
                    GL.Children.Add(postavMenuItem);
                }
                MainDelivery.Children.Add(GL);
            }

        }

        public Delivery SelectedDelivery {  get => Get<Delivery>(); set => Set(value); }

        public RelayCommand SelectDelivery => GetCommand<Delivery>(dlv => SelectedDelivery = dlv);
    }
}
