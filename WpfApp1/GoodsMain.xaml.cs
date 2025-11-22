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

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для GoodsMain.xaml
    /// </summary>
    public partial class GoodsMain : Window
    {
        public GoodsMain()
        {
            InitializeComponent();
        }

        private void OpenProfile(object sender, RoutedEventArgs e)
        {
            Profile profile= new Profile();
            profile.Show();
            this.Close();
        }

        private void OpenCart(object sender, RoutedEventArgs e)
        {
            ShoppingCart cart= new ShoppingCart();
            cart.Show();
            this.Close();
        }

        private void OpenBurger(object sender, RoutedEventArgs e)
        {
            Burger burger= new Burger();
            burger.Show(); 
            this.Close();
        }
    }
}
