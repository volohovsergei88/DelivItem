using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        string[] levelClaster = { "Поставка ПО", "Обучение", "Моделирование" };
        string[] levelLearn = { "Группа (до 5 чел.)", "Группа (более 5 чел.)", "Индивидуально" };
        string[] levelModels = { "Поверхность", "Сети", "Полный комплект" };
        string[] typeDeliver = { "Новая поставка", "Обновление" };
        string[] levelProduct = { "ПС Профиль" };
        string[] levelMdl = { "Сети НВК", "Теслоснабжение", "Газ", "Кабели", "Все сети" };
        string[] levelTimeAll = { "12", "24" };
        string[] levelTime = { "12", "24", "1", "3", "6" };

        private void ContextMenuDel(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            ContextMenu contextMenu = new ContextMenu();
            for (int i = 0; i < levelClaster.Length; i++)
            {
                MenuItem GL = new MenuItem();
                GL.Header = levelClaster[i];

                if (levelClaster[i] == "Обучение")
                {
                    foreach (string ln in levelLearn)
                    {
                        MenuItem learnMenuItem = new MenuItem();
                        learnMenuItem.Header = ln;
                        learnMenuItem.Click += (s, ev) =>
                        {
                            HandleMenuItemClick(s, ev);
                        };
                        GL.Items.Add(learnMenuItem);
                    }
                }
                else if (levelClaster[i] == "Моделирование")
                {
                    foreach (string ln in levelModels)
                    {
                        MenuItem modelMenuItem = new MenuItem();
                        modelMenuItem.Header = ln;
                        modelMenuItem.Click += (s, ev) =>
                        {
                            HandleMenuItemClick(s, ev);
                        };
                        GL.Items.Add(modelMenuItem);
                    }
                }

                else if (levelClaster[i] == "Поставка ПО")
                {
                    MenuItem postavMenuItem = new MenuItem();
                    postavMenuItem.Header = "Поставка ПО";

                    foreach (string item in typeDeliver)
                    {
                        MenuItem typeMenuItem = new MenuItem();
                        typeMenuItem.Header = item;
                        foreach (string mdl in levelMdl)
                        {
                            MenuItem mdlMenuItem = new MenuItem();
                            mdlMenuItem.Header = mdl;
                            if (mdl == "Все сети")
                            {
                                foreach (string time in levelTime)
                                {
                                    MenuItem timeMenuItem = new MenuItem();
                                    timeMenuItem.Header = time;
                                    timeMenuItem.Click += (s, ev) =>
                                    {
                                        HandleMenuItemClick(s, ev);
                                    };
                                    mdlMenuItem.Items.Add(timeMenuItem);
                                }
                            }
                            else
                            {
                                foreach (string timeAll in levelTimeAll)
                                {
                                    MenuItem timeAllMenuItem = new MenuItem();
                                    timeAllMenuItem.Header = timeAll;
                                    timeAllMenuItem.Click += (s, ev) =>
                                    {
                                        HandleMenuItemClick(s, ev);
                                        //MenuItem selectedItem = (MenuItem)s;
                                        //List<string> parents = new List<string>();
                                        //GetParents(selectedItem, parents);
                                        //VolumeDelivery volumeDelivery = new VolumeDelivery();
                                        //volumeDelivery.AddItem(parents);
                                        //DeliveryItem volumeDelivery = new DeliveryItem();
                                        //volumeDelivery.AddItem(parents);
                                    };
                                    mdlMenuItem.Items.Add(timeAllMenuItem);
                                }
                            }
                            typeMenuItem.Items.Add(mdlMenuItem);
                        }
                        postavMenuItem.Items.Add(typeMenuItem);
                    }
                    GL.Items.Add(postavMenuItem);
                }
                contextMenu.Items.Add(GL);
                button.ContextMenu = contextMenu;
            }
        }

        private void GetParents(MenuItem item, List<string> parents)
        {
            parents.Add(item.Header.ToString());
            if (item.Parent is MenuItem parentItem)
            {
                GetParents(parentItem, parents);
            }
        }

        private void HandleMenuItemClick(object sender, RoutedEventArgs e)
        {
            DeliveriesViewModel vm = (DeliveriesViewModel)FindResource("dlvVM");
            MenuItem selectedItem = (MenuItem)sender;
            List<string> parents = new List<string>();
            GetParents(selectedItem, parents);
            VolumeDelivery volumeDelivery = new VolumeDelivery();
            vm.AddDelivery.Execute(new DeliveryItem(string.Join(", ", parents)));
            //volumeDelivery.AddItem(parents);
            //DeliveryItem volumeDelivery = new DeliveryItem();
            //volumeDelivery.AddItem(parents);
        }
    }
}
