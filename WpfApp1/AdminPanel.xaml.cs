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
    /// Логика взаимодействия для AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Window
    {
        public AdminPanel()
        {
            InitializeComponent();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        private void GoToEmployeeList(object sender, RoutedEventArgs e)
        {
            EmployeeList employeeList = new EmployeeList();
            employeeList.Show();
            this.Close();
        }

        private void AddEmployee(object sender, RoutedEventArgs e)
        {
            AddEmployee addEmployee = new AddEmployee();
            addEmployee.Show();
            this.Close();
        }

        private void ChangeEmployeeData(object sender, RoutedEventArgs e)
        {
            ChangeEmployeeData employeeData = new ChangeEmployeeData();
            employeeData.Show();
            this.Close();
        }

        private void GoToClientLost(object sender, RoutedEventArgs e)
        {
            ClientList clientList = new ClientList();
            clientList.Show();
            this.Close();
        }

        private void GoToOrderList(object sender, RoutedEventArgs e)
        {
            OrderList orderList = new OrderList();
            orderList.Show();
            this.Close();
        }

        private void GoToGoodsList(object sender, RoutedEventArgs e)
        {
            GoodsList goodsList = new GoodsList();
            goodsList.Show();
            this.Close();
        }

        private void GoToManageGoods(object sender, RoutedEventArgs e)
        {
            ManageGoods goodsList = new ManageGoods();
            goodsList.Show();
            this.Close();
        }
    }
}
