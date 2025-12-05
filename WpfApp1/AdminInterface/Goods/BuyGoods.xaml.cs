using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Models;
using WpfApp1.Services;
using WpfApp1.UserInterface;

namespace WpfApp1
{
    public partial class BuyGoods : Window
    {
        private DatabaseService _dbService;
        private List<Goods> _allGoods;
        private List<Category> _categories;
        private Goods _selectedGood;
        private Category _selectedCategory;

        public BuyGoods()
        {
            InitializeComponent();
            _dbService = new DatabaseService();
            LoadDataAsync();
        }

        private async void LoadDataAsync()
        {
            try
            {
                _allGoods = await _dbService.GetAllGoodsAsync();
                _categories = await _dbService.GetAllCategoriesAsync();

                cmbCategory.ItemsSource = _categories;
                cmbCategory.DisplayMemberPath = "Name";
                cmbCategory.SelectedValuePath = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Category_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCategory.SelectedItem is Category selectedCategory)
            {
                _selectedCategory = selectedCategory;

                var filteredGoods = _allGoods.Where(g => g.CategoryId == selectedCategory.Id).ToList();
                cmbGoods.ItemsSource = filteredGoods;
                cmbGoods.DisplayMemberPath = "Name";
                cmbGoods.SelectedValuePath = "Id";
                cmbGoods.SelectedIndex = -1;

                _selectedGood = null;
            }
        }

        private void Goods_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbGoods.SelectedItem is Goods selectedGood)
            {
                _selectedGood = selectedGood;
            }
        }

        private async void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedGood == null)
                {
                    MessageBox.Show("Выберите товар!", "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
                {
                    MessageBox.Show("Введите корректное количество (больше 0)!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var order = new Orders
                {
                    ClientID = GetCurrentUserId(),
                    TotalCost = _selectedGood.Price * quantity,
                    Weight = 0,
                    CookingTime = TimeSpan.FromMinutes(30),
                    DeliveryTime = TimeSpan.FromHours(1)
                };

                var orderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        GoodsId = _selectedGood.Id,
                        Amount = quantity,
                        Price = _selectedGood.Price
                    }
                };

                bool success = await _dbService.CreateOrderWithGoodsAsync(order, orderItems);

                if (success)
                {
                    MessageBox.Show($"Заказ успешно создан!\n" +
                                  $"Товар: {_selectedGood.Name}\n" +
                                  $"Количество: {quantity}\n" +
                                  $"Сумма: {_selectedGood.Price * quantity:C}",
                                  "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Ошибка при создании заказа!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
            MessageBox.Show("Заказ отменен.", "Информация",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ClearForm()
        {
            cmbCategory.SelectedIndex = -1;
            cmbGoods.ItemsSource = null;
            _selectedGood = null;
            _selectedCategory = null;
            txtQuantity.Text = "1";
        }

        private int GetCurrentUserId()
        {
            // Здесь должен быть код для получения ID текущего пользователя
            // Например, из сессии, настроек или контекста приложения

            // Временное решение - возвращаем ID 1 (администратор)
            // В реальном приложении нужно реализовать систему аутентификации
            return 1;
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