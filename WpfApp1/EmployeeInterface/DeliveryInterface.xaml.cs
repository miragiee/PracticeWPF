using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Models;
using WpfApp1.Services;
using WpfApp1.UserInterface;

namespace WpfApp1.EmployeeInterface
{
    public partial class DeliveryInterface : Window
    {
        public ObservableCollection<Delivery> Delivery { get; set; }
        public ObservableCollection<Orders> Order { get; set; }
        private DatabaseService _databaseService;
        private int _currentEmployeeId = 5; // ID текущего сотрудника

        public DeliveryInterface()
        {
            InitializeComponent();

            Delivery = new ObservableCollection<Delivery>();
            Order = new ObservableCollection<Orders>();
            _databaseService = new DatabaseService();

            DataContext = this;

            // Загружаем данные при инициализации
            LoadDeliveryData();
            LoadOrdersData();
        }

        private async void LoadDeliveryData()
        {
            try
            {
                var deliveries = await _databaseService.GetActiveDeliveriesAsync();

                Delivery.Clear();
                foreach (var delivery in deliveries)
                {
                    Delivery.Add(delivery);
                }

                EmplGirdData.ItemsSource = Delivery;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки доставок: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void LoadOrdersData()
        {
            try
            {
                var orders = await _databaseService.GetOrdersForDeliveryAsync();

                Order.Clear();
                foreach (var order in orders)
                {
                    Order.Add(order);
                }

                EmployeeListDataGrid.ItemsSource = Order;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки заказов: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            WindowManager.SaveWindowStats(this);
            MainWindow mainWindow = new MainWindow();
            WindowManager.SetWindowStats(mainWindow);
            mainWindow.Show();
            this.Close();
        }

        // Обработчик кнопки "Взять заказ"
        private async void TakeOrder_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeeListDataGrid.SelectedItem is Orders selectedOrder)
            {
                try
                {
                    // Создаем запись о доставке
                    bool success = await _databaseService.CreateDeliveryForOrderAsync(selectedOrder.ID, _currentEmployeeId);

                    if (success)
                    {
                        MessageBox.Show($"Заказ #{selectedOrder.ID} взят в доставку",
                            "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Обновляем данные
                        LoadOrdersData();
                        LoadDeliveryData();
                    }
                    else
                    {
                        MessageBox.Show("Не удалось взять заказ в доставку",
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите заказ из списка", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Обработчик кнопки "Сдать заказ"
        private async void CompleteDelivery_Click(object sender, RoutedEventArgs e)
        {
            if (EmplGirdData.SelectedItem is Delivery selectedDelivery)
            {
                try
                {
                    // Обновляем статус заказа
                    bool success = await _databaseService.UpdateOrderStatusAsync(selectedDelivery.OrderID, "Доставлен");

                    if (success)
                    {
                        // Можно также удалить запись о доставке или изменить ее статус
                        MessageBox.Show($"Доставка #{selectedDelivery.Id} завершена",
                            "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Обновляем данные
                        LoadDeliveryData();
                    }
                    else
                    {
                        MessageBox.Show("Не удалось завершить доставку",
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите доставку из списка", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Обновление выпадающего списка
        private void UpdateComboBox_Click(object sender, RoutedEventArgs e)
        {
            LoadOrdersData();
            LoadDeliveryData();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EmployeeListDataGrid.ItemsSource = Order;
            EmplGirdData.ItemsSource = Delivery;
        }

        private void EmployeeListDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Логика при изменении выбора заказа
        }

        private void EmplGirdData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Логика при изменении выбора доставки
        }
    }
}