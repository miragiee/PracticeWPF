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

    public partial class ClientList : Window
    {
        public ObservableCollection<Client> Clients { get; set; }

        public ClientList()
        {

            InitializeComponent();

            Clients = new ObservableCollection<Client>()
            {
                new Client
                {
                    Id = 1,
                    RoleId = 3,
                    Login = "login",
                    Password = "password",
                    Name = "Name",
                    LastName = "LastName",
                    Patronymic = "Patronymic",
                    PhoneNumber = "1234567890",
                    Email = "userMail@mail.ru",
                    BirthDate = DateTime.Now,
                    Address = "Улица Пушкина"
                },

                new Client
                {
                    Id = 2,
                    RoleId = 3,
                    Login = "login1",
                    Password = "password1",
                    Name = "Name1",
                    LastName = "LastName1",
                    Patronymic = "Patronymic1",
                    PhoneNumber = "1234567890111",
                    Email = "userMai1l@mail.ru",
                    BirthDate = DateTime.Now,
                    Address = "Улица Пушкина1"
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
