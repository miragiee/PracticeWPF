using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Models;
using WpfApp1.Services;
using WpfApp1.UserInterface;

namespace WpfApp1
{
    public partial class BakerInterface : Window
    {
        public ObservableCollection<Orders> Bakery { get; set; }
        private DatabaseService _databaseService;
        private int _currentOrderId = 0;

        public BakerInterface()
        {
            InitializeComponent();

            Bakery = new ObservableCollection<Orders>();
            _databaseService = new DatabaseService();

            DataContext = this;

            // Загружаем данные при инициализации
            LoadOrdersData();
        }

        private async void LoadOrdersData()
        {
            try
            {
                var orders = await _databaseService.GetAllOrdersForBakerAsync();

                // Очищаем и заполняем коллекцию
                Bakery.Clear();
                foreach (var order in orders)
                {
                    // Получаем товары для заказа
                    var orderGoods = await _databaseService.GetOrderGoodsAsync(order.ID);
                    if (orderGoods != null && orderGoods.Any())
                    {
                        // Формируем строку с товарами
                        var goodsList = orderGoods.Select(g => $"{g.Name} x{g.Amount}");
                        // Сохраняем в дополнительное свойство или поле
                    }

                    Bakery.Add(order);
                }

                // Обновляем DataGrid
                EmployeeListDataGrid.ItemsSource = Bakery;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
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

        // Обработчик выбора заказа в списке
        private void EmployeeListDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EmployeeListDataGrid.SelectedItem is Orders selectedOrder)
            {
                _currentOrderId = selectedOrder.ID;
            }
        }

        // Обработчик кнопки "Сдать заказ"
        private async void CompleteOrder_Click(object sender, RoutedEventArgs e)
        {
            if (_currentOrderId > 0)
            {
                try
                {
                    // Обновляем статус заказа
                    bool success = await _databaseService.UpdateOrderStatusAsync(_currentOrderId, "Приготовлен");

                    if (success)
                    {
                        MessageBox.Show($"Заказ #{_currentOrderId} отмечен как приготовленный",
                            "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Обновляем данные
                        LoadOrdersData();
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
            else
            {
                MessageBox.Show("Выберите заказ из списка", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Обновление данных при загрузке окна
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EmployeeListDataGrid.ItemsSource = Bakery;
        }

        // Обновление выпадающего списка
        private void UpdateComboBox_Click(object sender, RoutedEventArgs e)
        {
            LoadOrdersData();
        }
    }
}