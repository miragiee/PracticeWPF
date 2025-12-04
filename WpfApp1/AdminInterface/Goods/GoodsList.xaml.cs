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
using WpfApp1.UserInterface;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для EmployeeList.xaml
    /// </summary>

    public partial class GoodsList : Window
    {
        public ObservableCollection<Goods> Goods { get; set; }

        public GoodsList()
        {

            InitializeComponent();

            Goods = new ObservableCollection<Goods>()
            {
                new Goods
                {
                    Id = 1,
                    Name = "Лаваш",
                    Price = 60,
                    CategoryId = 1,
                },

                new Goods
                {
                    Id= 4,
                    Name = "Апельсин",
                    Price = 100,
                    CategoryId = 2
                }

            };
            
            DataContext = this;
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            WindowManager.SaveWindowStats(this);
            AdminPanel adminPanel = new AdminPanel();
            WindowManager.SetWindowStats(adminPanel);
            adminPanel.Show();
            this.Close();
        }
    }
}
