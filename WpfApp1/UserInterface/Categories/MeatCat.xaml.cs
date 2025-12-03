using System;
using System.Collections.Generic;
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
using WpfApp1.Managers;
using WpfApp1.Models;
using WpfApp1.ViewModels;

namespace WpfApp1.UserInterface
{
    /// <summary>
    /// Логика взаимодействия для MeatCat.xaml
    /// </summary>
    public partial class MeatCat : Window
    {
        public MeatCat()
        {
            InitializeComponent();
            DataContext = new MeatCatViewModel();
        }

        private void OpenProfile(object sender, RoutedEventArgs e)
        {
            WindowManager.SaveWindowStats(this);
            Profile profile = new Profile();
            WindowManager.SetWindowStats(profile);
            profile.Show();
            this.Close();
        }

        private void OpenCart(object sender, RoutedEventArgs e)
        {
            ShoppingCart cart = new ShoppingCart();
            cart.cartParent = this;
            cart.ShowDialog();
        }

        private void OpenBurger(object sender, RoutedEventArgs e)
        {
            Burger burger = new Burger();
            burger.ParentWindow = this;
            burger.ShowDialog();
        }

        private void BuyButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Goods goods)
            {
                CartManager.Instance.AddProduct(goods, 1);
                MessageBox.Show($"Товар \"{goods.Name}\" добавлен в корзину!",
                               "Корзина", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}