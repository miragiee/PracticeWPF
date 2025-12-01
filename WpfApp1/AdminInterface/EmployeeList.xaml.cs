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

    public partial class EmployeeList : Window
    {
        public ObservableCollection<Users> Employees { get; set; }

        public EmployeeList()
        {

            InitializeComponent();

            Employees = new ObservableCollection<Users>()
            {
                new Users
                {
                    Id = 1,
                    RoleId = 1,
                    Login = "Admin",
                    Password = "123456",
                    Name = "Name",
                    LastName = "Lastname",
                    Patronymic = "Patro",
                    PhoneNumber = "1234567890",
                    Email = "bestmail@mail.ru",
                    BirthDate = DateTime.Now,
                    Address = "Улица Пушкина, Дом Колотушкина"

                },

                new Users
                {
                    Id = 2,
                    RoleId = 2,
                    Login = "NotAdmin",
                    Password = "123456",
                    Name = "Name",
                    LastName = "Lastname",
                    Patronymic = "Patro",
                    PhoneNumber = "1234567890",
                    Email = "bestmail@mail.ru",
                    BirthDate = DateTime.Now,
                    Address = "Улица Пушкина, Дом Колотушкина"
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
