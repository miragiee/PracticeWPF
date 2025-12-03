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
using WpfApp1.Classes;
using WpfApp1.Managers;
using WpfApp1.Models;
using WpfApp1.UserInterface;
namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для ShoppingCart.xaml
    /// </summary>
    public partial class ShoppingCart : Window
    {
        public Window? cartParent { get; set; }
        public ShoppingCart()
        {
            InitializeComponent();

            DataContext = CartManager.Instance;

            CartManager.Instance.CartChanged += CartWasChanged;
        }

        private void CartWasChanged(object sender, EventArgs e)
        {
            UpdateTotal();
        }

        private void UpdateTotal()
        {
            if (FindName("TotalBlock") is TextBlock TotalBlock)
            {
                TotalBlock.Text = $"С У М М А: {CartManager.Instance.TotalPrice:C}";
            }
        }


        private void CloseShoppingCart(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MoveToPayment(object sender, RoutedEventArgs e)
        {
            Payment payment = new Payment();
            WindowManager.SaveWindowStats(this);
            payment.Show();
            WindowManager.SetWindowStats(payment);
            cartParent?.Close();
            this.Close();

        }

        private void IncreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is CartItem item)
            {
                CartManager.Instance.AddProduct(item.Product, 1);
            }
        }

        private void DecreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is CartItem item)
            {
                CartManager.Instance.DeleteProduct(item.Product, 1);
            }
        }
    }
}
