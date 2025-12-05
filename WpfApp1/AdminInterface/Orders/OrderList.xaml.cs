using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Models;
using WpfApp1.Services;
using WpfApp1.UserInterface;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WpfApp1
{
    public partial class OrderList : Window, INotifyPropertyChanged
    {
        private DatabaseService dbService = new DatabaseService();
        private ObservableCollection<Orders> _orders;

        public ObservableCollection<Orders> Orders
        {
            get => _orders;
            set
            {
                _orders = value;
                OnPropertyChanged(nameof(Orders));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public OrderList()
        {
            InitializeComponent();
            Orders = new ObservableCollection<Orders>();
            DataContext = this;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadOrders();
        }

        private async Task LoadOrders()
        {
            try
            {
                // Проверяем подключение
                bool isConnected = await dbService.TestConnectionAsync();
                if (!isConnected)
                {
                    MessageBox.Show("Ошибка подключения к базе данных. Будут загружены тестовые данные.",
                                  "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Error);
                    LoadTestData();
                    return;
                }

                // Загружаем заказы
                var orders = await dbService.GetAllOrdersAsync();

                // Очищаем и заполняем коллекцию
                Orders.Clear();
                foreach (var order in orders)
                {
                    Orders.Add(order);
                }

                // Обновляем ItemsSource
                OrdersDataGrid.ItemsSource = null;
                OrdersDataGrid.ItemsSource = Orders;

                // Если данных нет, показываем сообщение
                if (Orders.Count == 0)
                {
                    MessageBox.Show("В базе данных нет заказов",
                        "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (MySqlException mysqlEx)
            {
                string errorMessage = $"Ошибка MySQL ({mysqlEx.Number}):\n{mysqlEx.Message}";

                if (mysqlEx.Number == 1042) // Ошибка подключения
                {
                    errorMessage += "\n\nПроверьте:\n" +
                                   "1. Доступность сервера\n" +
                                   "2. Правильность логина и пароля\n" +
                                   "3. Разрешения на удаленное подключение";
                }

                MessageBox.Show(errorMessage,
                    "Ошибка базы данных", MessageBoxButton.OK, MessageBoxImage.Error);
                LoadTestData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки заказов: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                LoadTestData();
            }
        }

        private void LoadTestData()
        {
            Orders.Clear();

            // Тестовые данные
            Orders.Add(new Orders
            {
                ID = 1,
                ClientID = 1,
                TotalCost = 390,
                Delivery = true,
                CookingTime = new TimeSpan(1, 0, 0)
            });

            Orders.Add(new Orders
            {
                ID = 2,
                ClientID = 2,
                TotalCost = 1250,
                Delivery = false,
                CookingTime = new TimeSpan(0, 45, 0)
            });

            Orders.Add(new Orders
            {
                ID = 3,
                ClientID = 3,
                TotalCost = 2730,
                Delivery = false,
                CookingTime = new TimeSpan(1, 30, 0)
            });

            Orders.Add(new Orders
            {
                ID = 4,
                ClientID = 4,
                TotalCost = 890,
                Delivery = true,
                CookingTime = new TimeSpan(0, 30, 0)
            });

            Orders.Add(new Orders
            {
                ID = 5,
                ClientID = 5,
                TotalCost = 1560,
                Delivery = true,
                CookingTime = new TimeSpan(1, 15, 0)
            });

            // Обновляем DataGrid
            OrdersDataGrid.ItemsSource = null;
            OrdersDataGrid.ItemsSource = Orders;
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