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
    public partial class CashierInterface : Window
    {
        public ObservableCollection<OrdersGoods> OrdersGoods { get; set; }
        private DatabaseService _databaseService;
        private decimal _totalAmount = 0;
        private int _selectedOrderId = 0;

        public CashierInterface()
        {
            InitializeComponent();

            OrdersGoods = new ObservableCollection<OrdersGoods>();
            _databaseService = new DatabaseService();

            DataContext = this;

            // Загружаем данные при инициализации
            LoadOrderData();
        }

        private async void LoadOrderData()
        {
            try
            {
                // Получаем активные заказы
                var orders = await _databaseService.GetActiveOrdersForCashierAsync();

                if (orders.Any())
                {
                    _selectedOrderId = orders.First().ID;

                    // Получаем товары для первого заказа
                    await LoadOrderGoods(_selectedOrderId);
                }
                else
                {
                    OrdersGoods.Clear();
                    MessageBox.Show("Нет активных заказов для обработки", "Информация",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadOrderGoods(int orderId)
        {
            try
            {
                var orderGoods = await _databaseService.GetOrderGoodsForCashierAsync(orderId);

                // Очищаем и заполняем коллекцию
                OrdersGoods.Clear();
                _totalAmount = 0;

                foreach (var item in orderGoods)
                {
                    OrdersGoods.Add(item);
                    _totalAmount += item.TotalPrice;
                }

                // Обновляем DataGrid
                EmployeeListDataGrid.ItemsSource = OrdersGoods;

                // Обновляем отображение общей суммы
                UpdateTotalDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки товаров: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateTotalDisplay()
        {
            // Здесь можно обновить TextBlock с общей суммой
            // Например, если есть элемент с именем TotalTextBlock:
            // TotalTextBlock.Text = $"Итого: {_totalAmount:C}";
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            WindowManager.SaveWindowStats(this);
            MainWindow mainWindow = new MainWindow();
            WindowManager.SetWindowStats(mainWindow);
            mainWindow.Show();
            this.Close();
        }

        private void GoToPayment(object sender, RoutedEventArgs e)
        {
            if (OrdersGoods.Count == 0)
            {
                MessageBox.Show("Нет товаров для оплаты", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Здесь можно добавить логику оплаты
                MessageBox.Show($"Сумма к оплате: {_totalAmount:C}", "Оплата",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                WindowManager.SaveWindowStats(this);
                PaymentSuccessful paymentSuccessful = new PaymentSuccessful();
                WindowManager.SetWindowStats(paymentSuccessful);
                paymentSuccessful.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при переходе к оплате: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Обработчик кнопки "Сдать заказ"
        private async void CompleteOrder_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedOrderId > 0)
            {
                try
                {
                    // Обновляем статус заказа как оплаченный
                    bool success = await _databaseService.UpdateOrderStatusAsync(_selectedOrderId, "Оплачен");

                    if (success)
                    {
                        MessageBox.Show($"Заказ #{_selectedOrderId} успешно оплачен",
                            "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Загружаем следующий заказ
                        LoadOrderData();
                    }
                    else
                    {
                        MessageBox.Show("Не удалось обновить статус заказа",
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Обработчик кнопки "Отменить покупку"
        private void CancelPurchase_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersGoods.Count > 0)
            {
                var result = MessageBox.Show("Вы уверены, что хотите отменить текущий заказ?",
                    "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    OrdersGoods.Clear();
                    _totalAmount = 0;
                    UpdateTotalDisplay();
                }
            }
        }

        // Обновление выпадающего списка
        private void UpdateComboBox_Click(object sender, RoutedEventArgs e)
        {
            LoadOrderData();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EmployeeListDataGrid.ItemsSource = OrdersGoods;
        }
    }
}