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
    /// Логика взаимодействия для AddEmployee.xaml
    /// </summary>
    public partial class AddEmployee : Window
    {
        public ObservableCollection<Users> users;
        public AddEmployee()
        {
            InitializeComponent();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            WindowManager.SaveWindowStats(this);
            AdminPanel adminPanel = new AdminPanel();
            WindowManager.SetWindowStats(adminPanel);
            adminPanel.Show();
            this.Close();
        }

        public void AddButton(object sender, RoutedEventArgs e)
        {
            users = new ObservableCollection<Users>();

            new Users
            {
                RoleId = 2,
                Name = nameBox.Text,
                LastName = lastnameBox.Text,
                Patronymic = patronymicBox.Text,
                PhoneNumber = phoneBox.Text,
                Email = emailBox.Text,
                BirthDate = DateTime.Parse(birthDateBox.Text),
                Login = $"{nameBox.Text + lastnameBox.Text.FirstOrDefault()}",
                Password = $"{nameBox.Text.FirstOrDefault()}.{lastnameBox.Text}"
            };

            nameBox.Text = "Имя";
            lastnameBox.Text = "Фамилия";
            patronymicBox.Text = "Отчество";
            birthDateBox.Text = "Дата рождения";
            emailBox.Text = "Почта";
            phoneBox.Text = "Телефон";
            salaryBox.Text = "Зарплата";
            postBox.Text = "Должность";
        }
    }
}
