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

namespace WpfApp1.UserInterface.Categories
{
    /// <summary>
    /// Логика взаимодействия для SweetsCat.xaml
    /// </summary>
    public partial class SweetsCat : Window
    {
        public SweetsCat()
        {
            InitializeComponent();
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
    }
}
