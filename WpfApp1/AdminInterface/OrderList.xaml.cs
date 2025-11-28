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

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для EmployeeList.xaml
    /// </summary>

    public partial class OrderList : Window
    {
        public ObservableCollection<Order> Orders { get; set; }

        public OrderList()
        {

            InitializeComponent();

            Orders = new ObservableCollection<Order>()
            {
                new Order
                {
                   Id = 1,
                   ClientId = 1,
                   totalCost = 390,
                   Delivery = true,
                   CookingTime = new TimeSpan(1, 0, 0),
                   OrderedGoodsID = [1, 2, 3, 4]
                },

                new Order
                {
                   Id = 3,
                   ClientId= 3,
                   totalCost = 2730,
                   Delivery = false,
                   CookingTime = new TimeSpan(1, 0, 0),
                   OrderedGoodsID = [1, 2, 3, 4]
                }

            };

            DataContext = this;
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            AdminPanel adminPanel = new AdminPanel();
            adminPanel.Show();
            this.Close();
        }
    }
}
