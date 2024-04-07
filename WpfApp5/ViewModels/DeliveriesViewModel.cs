using Dto;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Wpf;

namespace ViewModels
{
    /// <summary>Предоставляет линейную коллекцию <see cref="Delivery"/>
    /// из которой нужно динамически формировать дерево.
    /// Такая структура очень удобна тем, что легко стыкуется с БД.</summary>
    public class DeliveriesViewModel : ViewModelBase
    {
        public ReadOnlyObservableCollection<Delivery> Deliveries { get; }
        private readonly ObservableCollection<Delivery> DeliveriesList = new ObservableCollection<Delivery>();

        public ReadOnlyObservableCollection<Delivery> SelectedDeliveries { get; }
        private readonly ObservableCollection<Delivery> SelectedDeliveriesList = new ObservableCollection<Delivery>();


        // Это эмуляция полей БД. 
        string[] levelClaster = { "Поставка ПО", "Обучение", "Моделирование" };
        string[] levelLearn = { "Группа (до 5 чел.)", "Группа (более 5 чел.)", "Индивидуально" };
        string[] levelModels = { "Поверхность", "Сети", "Полный комплект" };
        string[] typeDeliver = { "Новая поставка", "Обновление" };
        string[] levelProduct = { "ПС Профиль" };
        string[] levelMdl = { "Сети НВК", "Теслоснабжение", "Газ", "Кабели", "Все сети" };
        string[] levelTimeAll = { "12", "24" };
        string[] levelTime = { "12", "24", "1", "3", "6" };

        private static int deliveryId = 0;
        private static Delivery Create(ICollection<Delivery> deliveries, string namedelivery, int parentId = 0)
        {
            deliveryId++;
            var dlv = new Delivery(deliveryId, namedelivery, parentId);
            deliveries.Add(dlv);
            return dlv;
        }

        public DeliveriesViewModel()
        {
            Deliveries = new ReadOnlyObservableCollection<Delivery>(DeliveriesList);
            SelectedDeliveries = new ReadOnlyObservableCollection<Delivery>(SelectedDeliveriesList);

            for (int i = 0; i < levelClaster.Length; i++)
            {
                Delivery GL = Create(DeliveriesList, levelClaster[i]);

                if (levelClaster[i] == "Обучение")
                {
                    foreach (string ln in levelLearn)
                    {
                        Delivery learnMenuItem = Create(DeliveriesList, ln, GL.Id);
                    }
                }
                else if (levelClaster[i] == "Моделирование")
                {
                    foreach (string ln in levelModels)
                    {
                        Delivery modelMenuItem = Create(DeliveriesList, ln, GL.Id);
                    }
                }

                else if (levelClaster[i] == "Поставка ПО")
                {
                    Delivery postavMenuItem = Create(DeliveriesList, levelClaster[i], GL.Id);

                    foreach (string item in typeDeliver)
                    {
                        Delivery typeMenuItem = Create(DeliveriesList, item, postavMenuItem.Id);

                        foreach (string mdl in levelMdl)
                        {
                            Delivery mdlMenuItem = Create(DeliveriesList, mdl, typeMenuItem.Id);

                            if (mdl == "Все сети")
                            {
                                foreach (string time in levelTime)
                                {
                                    Delivery timeMenuItem = Create(DeliveriesList, time, mdlMenuItem.Id);
                                }
                            }
                            else
                            {
                                foreach (string timeAll in levelTimeAll)
                                {
                                    Delivery timeAllMenuItem = Create(DeliveriesList, timeAll, mdlMenuItem.Id);

                                }
                            }
                        }
                    }
                }
            }

        }

        public Delivery? SelectedDelivery { get => Get<Delivery?>(); private set => Set(value); }

        public RelayCommand SelectDelivery => GetCommand<Delivery>(dlv => SelectedDeliveriesList.Add((SelectedDelivery = dlv).Value));
        public RelayCommand UnselectDelivery => GetCommand<Delivery>(dlv =>
        {
            if (SelectedDeliveriesList.Remove(dlv) && SelectedDelivery?.Id == dlv.Id)
            {
                if (SelectedDeliveriesList.Count == 0)
                {
                    SelectedDelivery = null;
                }
                else
                {
                    SelectedDelivery = SelectedDeliveriesList[SelectedDeliveriesList.Count - 1];
                }
            }
        });
    }

}
