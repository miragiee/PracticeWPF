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
using WpfApp1.UserInterface;
using WpfApp1.UserInterface.Categories;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Burger.xaml
    /// </summary>
    public partial class Burger : Window
    {
        private Window _parentWindow;
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
            WindowManager.SetWindowStats(profile);
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

        private void SwitchCategoryToBread(object sender, RoutedEventArgs e)
        {
            BreadCat cat = new BreadCat();
            cat.Show();
            this.Close();
            ParentWindow?.Close();
        }

        private void SwitchCategoryToFruit(object sender, RoutedEventArgs e)
        {
            FruitCat fruit = new FruitCat();
            fruit.Show();
            this.Close();
            ParentWindow?.Close();
        }

        private void SwitchCategoryToMeat(object sender, RoutedEventArgs e)
        {
            MeatCat meat = new MeatCat();
            meat.Show();
            this.Close();
            ParentWindow?.Close();
        }

        private void SwitchCategoryToDrinks(object sender, RoutedEventArgs e)
        {
            DrinksCat cat = new DrinksCat();
            cat.Show();
            this.Close();
            ParentWindow?.Close();
        }

        private void SwitchCategoryToReadyMeals(object sender, RoutedEventArgs e)
        {
            ReadyMealCat cat = new ReadyMealCat();
            cat.Show();
            this.Close();
            ParentWindow?.Close();
        }

        private void SwitchCategoryToSweets(object sender, RoutedEventArgs e)
        {
            SweetsCat cat = new SweetsCat();
            cat.Show();
            this.Close();
            ParentWindow?.Close();
        }

        private void SwitchCategoryToHygiene(object sender, RoutedEventArgs e)
        {
            HygienaCat cat = new HygienaCat();
            cat.Show();
            this.Close();
            ParentWindow?.Close();
        }
    }
}
