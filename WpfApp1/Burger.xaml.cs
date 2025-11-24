using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
    /// Логика взаимодействия для Burger.xaml
    /// </summary>
    public partial class Burger : Window
    {
        public Window ParentWindow { get; set; }
        
        public Burger()
        {
            InitializeComponent();
        }

        private void Close_Burger(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Open_Profile(object sender, RoutedEventArgs e)
        {
            Profile profile = new Profile();
            profile.Show();
            this.Close();
            ParentWindow?.Close();
        }

        private void Open_Cart(object sender, RoutedEventArgs e)
        {
            ShoppingCart cart = new ShoppingCart();

            cart.cartParent = this;
            cart.ShowDialog();
            this.Close();
            
        }
    }
}
