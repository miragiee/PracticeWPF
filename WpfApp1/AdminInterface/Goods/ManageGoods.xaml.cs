using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp1.Models;
using WpfApp1.Services;
using WpfApp1.UserInterface;

namespace WpfApp1
{
    public partial class ManageGoods : Window
    {
        private readonly DatabaseService _dbService;
        private List<Goods> _allGoods;
        private List<Category> _allCategories;
        private Goods _selectedGoods;

        public ManageGoods()
        {
            InitializeComponent();
            _dbService = new DatabaseService();
            _allGoods = new List<Goods>();
            _allCategories = new List<Category>();
            _selectedGoods = null;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                ShowStatus("З А Г Р У З К А   Д А Н Н Ы Х . . .", false);

                // Загружаем данные из базы
                var goodsTask = _dbService.GetAllGoodsAsync();
                var categoriesTask = _dbService.GetAllCategoriesAsync();

                await Task.WhenAll(goodsTask, categoriesTask);

                _allGoods = await goodsTask;
                _allCategories = await categoriesTask;

                // Логируем для отладки
                Console.WriteLine($"Загружено товаров: {_allGoods.Count}");
                Console.WriteLine($"Загружено категорий: {_allCategories.Count}");

                foreach (var goods in _allGoods)
                {
                    Console.WriteLine($"Товар: {goods.ID} - {goods.Name} - {goods.Price}");
                }

                Dispatcher.Invoke(() =>
                {
                    // Загружаем товары в ComboBox
                    GoodsComboBox.ItemsSource = null;
                    if (_allGoods.Any())
                    {
                        GoodsComboBox.ItemsSource = _allGoods;
                        Console.WriteLine("Товары загружены в ComboBox");
                    }
                    else
                    {
                        Console.WriteLine("Список товаров пуст!");
                    }

                    // Загружаем категории в ComboBox
                    CategoryComboBox.ItemsSource = null;
                    CategoryComboBox.ItemsSource = _allCategories;

                    if (_allCategories.Any())
                    {
                        CategoryComboBox.SelectedIndex = 0;
                        Console.WriteLine($"Выбрана категория: {_allCategories[0].Name}");
                    }

                    ClearForm();
                    HideStatus();

                    // Показываем информацию о загрузке
                    ShowStatus($"З А Г Р У Ж Е Н О: {_allGoods.Count} т о в а р о в, {_allCategories.Count} к а т е г о р и й", true);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки: {ex.Message}");
                ShowStatus($"О Ш И Б К А З А Г Р У З К И: {ex.Message}", false);
            }
        }

        private void GoodsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GoodsComboBox.SelectedItem is Goods selectedGoods)
            {
                _selectedGoods = selectedGoods;
                LoadGoodsData(selectedGoods);
                UpdateButtonsState();
                ShowStatus($"В Ы Б Р А Н: {selectedGoods.Name}", true);
            }
        }

        private void LoadGoodsData(Goods goods)
        {
            try
            {
                NameTextBox.Text = goods.Name;
                PriceTextBox.Text = goods.Price.ToString("F2");

                // Находим соответствующую категорию
                if (_allCategories.Any())
                {
                    var category = _allCategories.FirstOrDefault(c => c.Id == goods.CategoryID);
                    if (category != null)
                    {
                        CategoryComboBox.SelectedItem = category;
                    }
                    else
                    {
                        CategoryComboBox.SelectedIndex = 0;
                    }
                }

                Console.WriteLine($"Загружены данные товара: {goods.Name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки данных товара: {ex.Message}");
            }
        }

        private async void SaveGoods_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                int categoryId;
                if (CategoryComboBox.SelectedValue is int id)
                {
                    categoryId = id;
                }
                else if (CategoryComboBox.SelectedItem is Category selectedCategory)
                {
                    categoryId = selectedCategory.Id;
                }
                else
                {
                    ShowStatus("В Ы Б Е Р И Т Е   К А Т Е Г О Р И Ю", false);
                    return;
                }

                var goods = new Goods
                {
                    ID = _selectedGoods?.ID ?? 0,
                    Name = NameTextBox.Text.Trim(),
                    Price = decimal.Parse(PriceTextBox.Text),
                    CategoryID = categoryId,
                };

                bool success;
                string message;

                if (_selectedGoods == null || _selectedGoods.ID == 0)
                {
                    // Добавление нового товара
                    success = await _dbService.AddGoodsAsync(goods);
                    message = success ? "Т О В А Р   Д О Б А В Л Е Н" : "О Ш И Б К А   Д О Б А В Л Е Н И Я";
                }
                else
                {
                    // Обновление существующего товара
                    goods.ID = _selectedGoods.ID;
                    success = await _dbService.UpdateGoodsAsync(goods);
                    message = success ? "Т О В А Р   О Б Н О В Л Е Н" : "О Ш И Б К А   О Б Н О В Л Е Н И Я";
                }

                if (success)
                {
                    ShowStatus(message, true);
                    await LoadDataAsync();
                }
                else
                {
                    ShowStatus(message, false);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сохранения: {ex.Message}");
                ShowStatus($"О Ш И Б К А: {ex.Message}", false);
            }
        }

        private async void DeleteGoods_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedGoods == null || _selectedGoods.ID == 0)
            {
                ShowStatus("В Ы Б Е Р И Т Е   Т О В А Р   Д Л Я   У Д А Л Е Н И Я", false);
                return;
            }

            var result = MessageBox.Show(
                $"В Ы   У В Е Р Е Н Ы,   Ч Т О   Х О Т И Т Е   У Д А Л И Т Ь   Т О В А Р   \"{_selectedGoods.Name}\"?",
                "П О Д Т В Е Р Ж Д Е Н И Е   У Д А Л Е Н И Я",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    bool success = await _dbService.DeleteGoodsAsync(_selectedGoods.ID);

                    if (success)
                    {
                        ShowStatus("Т О В А Р   У Д А Л Е Н", true);
                        await LoadDataAsync();
                        ClearForm();
                    }
                    else
                    {
                        ShowStatus("О Ш И Б К А   У Д А Л Е Н И Я   Т О В А Р А", false);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка удаления: {ex.Message}");
                    ShowStatus($"О Ш И Б К А: {ex.Message}", false);
                }
            }
        }

        private void NewGoods_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
            _selectedGoods = new Goods
            {
                ID = 0,
                Name = "",
                Price = 0,
                CategoryID = _allCategories.FirstOrDefault()?.Id ?? 0,
                ImagePath = ""
            };

            if (_allCategories.Any())
            {
                CategoryComboBox.SelectedIndex = 0;
            }

            UpdateButtonsState();
            ShowStatus("Г О Т О В О   К   Д О Б А В Л Е Н И Ю   Н О В О Г О   Т О В А Р А", true);
        }

        private void ClearForm()
        {
            NameTextBox.Text = "";
            PriceTextBox.Text = "";

            if (_allCategories.Any())
            {
                CategoryComboBox.SelectedIndex = 0;
            }

            GoodsComboBox.SelectedItem = null;
            _selectedGoods = null;
            UpdateButtonsState();
            HideStatus();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                ShowStatus("В В Е Д И Т Е   Н А З В А Н И Е   Т О В А Р А", false);
                NameTextBox.Focus();
                return false;
            }

            if (!decimal.TryParse(PriceTextBox.Text, out decimal price) || price < 0)
            {
                ShowStatus("В В Е Д И Т Е   К О Р Р Е К Т Н У Ю   Ц Е Н У", false);
                PriceTextBox.Focus();
                return false;
            }

            if (CategoryComboBox.SelectedItem == null)
            {
                ShowStatus("В Ы Б Е Р И Т Е   К А Т Е Г О Р И Ю", false);
                CategoryComboBox.Focus();
                return false;
            }

            return true;
        }

        private void UpdateButtonsState()
        {
            bool hasSelection = _selectedGoods != null;
            bool hasValidInput = !string.IsNullOrWhiteSpace(NameTextBox.Text) &&
                                !string.IsNullOrWhiteSpace(PriceTextBox.Text) &&
                                CategoryComboBox.SelectedItem != null;

            SaveButton.IsEnabled = hasValidInput;
            DeleteButton.IsEnabled = hasSelection;
        }

        private void ShowStatus(string message, bool isSuccess)
        {
            Dispatcher.Invoke(() =>
            {
                StatusBorder.Visibility = Visibility.Visible;
                StatusTextBlock.Text = message;

                if (isSuccess)
                {
                    StatusBorder.BorderBrush = System.Windows.Media.Brushes.Green;
                    StatusTextBlock.Foreground = System.Windows.Media.Brushes.Green;
                }
                else
                {
                    StatusBorder.BorderBrush = System.Windows.Media.Brushes.Red;
                    StatusTextBlock.Foreground = System.Windows.Media.Brushes.Red;
                }

                var timer = new System.Windows.Threading.DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(3);
                timer.Tick += (s, e) =>
                {
                    HideStatus();
                    timer.Stop();
                };
                timer.Start();
            });
        }

        private void HideStatus()
        {
            Dispatcher.Invoke(() =>
            {
                StatusBorder.Visibility = Visibility.Collapsed;
            });
        }

        private void Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateButtonsState();
        }

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsState();
        }

        private void PriceTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0) && e.Text != ".")
            {
                e.Handled = true;
            }

            if (e.Text == "." && ((TextBox)sender).Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private async void RefreshGoods_Click(object sender, RoutedEventArgs e)
        {
            await LoadDataAsync();
            ShowStatus("С П И С О К   Т О В А Р О В   О Б Н О В Л Е Н", true);
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