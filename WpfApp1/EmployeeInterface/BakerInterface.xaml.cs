using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using WpfApp1.Models;
using WpfApp1.UserInterface;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для BakerInterface.xaml
    /// </summary>
    public partial class BakerInterface : Window
    {
        public ObservableCollection<Orders> Bakery { get; set; }
        public BakerInterface()
        {
            InitializeComponent();

            Bakery = new ObservableCollection<Orders>()
            {
                new Orders
                {
                    Id = 2,
                    Delivery = true,
                    OrderedGoodsID = [1, 2, 3, 4],
                    
                    CookingTime = new TimeSpan(1, 0, 0)
                },

                new Orders
                {
                    Id = 3,
                    Delivery = false,
                    OrderedGoodsID = [2, 2, 3, 5],
                    CookingTime = new TimeSpan (1, 0, 0)
                }
            };

            DataContext = this;

        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            WindowManager.SaveWindowStats(this);
            MainWindow mainWindow = new MainWindow();
            WindowManager.SetWindowStats(mainWindow);
            mainWindow.Show();
            this.Close();
        }
    }
}
