using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Mysqlx.Crud;
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
using System.Windows.Shapes;
using WpfApp1.Models;

namespace WpfApp1.EmployeeInterface
{
    /// <summary>
    /// Логика взаимодействия для DeliveryInterface.xaml
    /// </summary>
    public partial class DeliveryInterface : Window
    {
        public ObservableCollection<Delivery>? Delivery { get; set; }
        public ObservableCollection<Orders>? Order { get; set; }

        public DeliveryInterface()
        {
            InitializeComponent();

            Delivery = new ObservableCollection<Delivery>()
            {
                new Delivery()
                {
                    Id = 1,
                    DeliveryAddress = "Улица Пушкина",
                    PickUpAddress = "Магазин",
                    DeliveryTime = new TimeSpan(1, 0, 0),
                    EmployeeID = 5,
                    OrderID = 228,
                    
                },
                new Delivery()
                {
                    Id = 2,
                    DeliveryAddress = "Улица Лермонтова",
                    PickUpAddress = "Магазин",
                    DeliveryTime = new TimeSpan(0, 30, 0),
                    EmployeeID = 5,
                    OrderID = 1337
                }

            };

            Order = new ObservableCollection<Orders>()
            {
                new Orders
                {
                    Id = 228,
                    ClientId = 2,
                    Delivery = true,
                    CookingTime = new TimeSpan(0, 20, 0),
                    OrderedGoodsID = [1, 2, 2, 2, 4],
                    Weight = 10.5,
                    DeliveryAddress = "Улица Пушкина",
                    DeliveryTime = new TimeSpan(1, 0, 0)
                },

                new Orders
                {
                    Id = 1337,
                    ClientId = 1,
                    Delivery = true,
                    CookingTime = new TimeSpan(0, 10, 0),
                    OrderedGoodsID = [1, 5],
                    Weight = 0.5,
                    DeliveryAddress = "Улица Лермонтова",
                    DeliveryTime = new TimeSpan(0, 30, 0)
                },
            };
            
            DataContext = this;
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

    }
}
