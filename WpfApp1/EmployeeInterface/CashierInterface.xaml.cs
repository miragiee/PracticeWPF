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
    /// Логика взаимодействия для CashierInterface.xaml
    /// </summary>
    public partial class CashierInterface : Window
    {
        public ObservableCollection<OrdersGoods>? OrdersGoods { get; set; }
        public CashierInterface()
        {
            InitializeComponent();

            OrdersGoods = new ObservableCollection<OrdersGoods>()
            {
                new OrdersGoods()
                {
                    Id = 1,
                    Name = "Лаваш",
                    OrderID = 2,
                    GoodsID = 2,
                    Price = 40,
                    Amount = 6,
                },

                new OrdersGoods()
                {
                    Id = 2,
                    Name = "Апельсин",
                    OrderID = 4,
                    GoodsID = 5,
                    Price = 30,
                    Amount = 2
                }
            };

            DataContext = this;

        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void GoToPayment(object sender, RoutedEventArgs e)
        {
            PaymentSuccessful paymentSuccessful = new PaymentSuccessful();
            paymentSuccessful.Show();
            this.Close();
        }
    }
}
