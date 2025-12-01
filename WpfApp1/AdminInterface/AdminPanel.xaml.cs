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
using WpfApp1.UserInterface;

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
            WindowManager.SaveWindowStats(this);
            MainWindow mw = new MainWindow();
            WindowManager.SetWindowStats(mw);
            mw.Show();
            this.Close();
        }

        private void GoToEmployeeList(object sender, RoutedEventArgs e)
        {
            WindowManager.SaveWindowStats(this);
            EmployeeList employeeList = new EmployeeList();
            WindowManager.SetWindowStats(employeeList);
            employeeList.Show();
            this.Close();
        }

        private void AddEmployee(object sender, RoutedEventArgs e)
        {
            WindowManager.SaveWindowStats(this);
            AddEmployee addEmployee = new AddEmployee();
            WindowManager.SetWindowStats(addEmployee);
            addEmployee.Show();
            this.Close();
        }

        private void ChangeEmployeeData(object sender, RoutedEventArgs e)
        {
            WindowManager.SaveWindowStats(this);
            ChangeEmployeeData employeeData = new ChangeEmployeeData();
            WindowManager.SetWindowStats(employeeData);
            employeeData.Show();
            this.Close();
        }

        private void GoToClientLost(object sender, RoutedEventArgs e)
        {
            WindowManager.SaveWindowStats(this);
            ClientList clientList = new ClientList();
            WindowManager.SetWindowStats(clientList);
            clientList.Show();
            this.Close();
        }

        private void GoToOrderList(object sender, RoutedEventArgs e)
        {
            WindowManager.SaveWindowStats(this);
            OrderList orderList = new OrderList();
            WindowManager.SetWindowStats(orderList);
            orderList.Show();
            this.Close();
        }

        private void GoToGoodsList(object sender, RoutedEventArgs e)
        {
            WindowManager.SaveWindowStats(this);
            GoodsList goodsList = new GoodsList();
            WindowManager.SetWindowStats(goodsList);
            goodsList.Show();
            this.Close();
        }

        private void GoToManageGoods(object sender, RoutedEventArgs e)
        {
            WindowManager.SaveWindowStats(this);
            ManageGoods manageGoods = new ManageGoods();
            WindowManager.SetWindowStats(manageGoods);
            manageGoods.Show();
            this.Close();
        }

        private void GoToBuyGoods(object sender, RoutedEventArgs e)
        {
            WindowManager.SaveWindowStats(this);
            BuyGoods buyGoods = new BuyGoods();
            WindowManager.SetWindowStats(buyGoods);
            buyGoods.Show();
            this.Close();
        }

        private void GoToManageOrders(object sender, RoutedEventArgs e)
        {
            WindowManager.SaveWindowStats(this);
            ManageOrders manageOrders = new ManageOrders();
            WindowManager.SetWindowStats(manageOrders);
            manageOrders.Show();
            this.Close();
        }
    }
}
