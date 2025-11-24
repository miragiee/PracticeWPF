using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для ShoppingCart.xaml
    /// </summary>
    public partial class ShoppingCart : Window
    {
        public Window cartParent { get; set; }
        public ShoppingCart()
        {
            InitializeComponent();
        }

        private void CloseShoppingCart(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MoveToPayment(object sender, RoutedEventArgs e)
        {
            Payment payment = new Payment();

            payment.Show();
            cartParent.Close();
            this.Close();

        }
    }
}
