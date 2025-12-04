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
    public partial class GoodsList : Window, INotifyPropertyChanged
    {
        private DatabaseService dbService = new DatabaseService();
        private ObservableCollection<Goods> _goods;

        public ObservableCollection<Goods> Goods
        {
            get => _goods;
            set
            {
                _goods = value;
                OnPropertyChanged(nameof(Goods));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public GoodsList()
        {
            InitializeComponent();
            Goods = new ObservableCollection<Goods>();
            DataContext = this;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadGoods();
        }

        private async Task LoadGoods()
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

                // Загружаем товары
                var goods = await dbService.GetAllGoodsAsync();

                // Очищаем и заполняем коллекцию
                Goods.Clear();
                foreach (var item in goods)
                {
                    Goods.Add(item);
                }

                // Обновляем ItemsSource
                GoodsDataGrid.ItemsSource = null;
                GoodsDataGrid.ItemsSource = Goods;

                // Если данных нет, показываем сообщение
                if (Goods.Count == 0)
                {
                    MessageBox.Show("В базе данных нет товаров",
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
                MessageBox.Show($"Ошибка загрузки товаров: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                LoadTestData();
            }
        }

        private void LoadTestData()
        {
            Goods.Clear();

            // Тестовые данные
            Goods.Add(new Goods
            {
                Id = 1,
                Name = "Лаваш",
                Price = 60,
                CategoryId = 1,
            });

            Goods.Add(new Goods
            {
                Id = 4,
                Name = "Апельсин",
                Price = 100,
                CategoryId = 2
            });

            Goods.Add(new Goods
            {
                Id = 5,
                Name = "Бананы",
                Price = 80,
                CategoryId = 2
            });

            Goods.Add(new Goods
            {
                Id = 6,
                Name = "Молоко",
                Price = 70,
                CategoryId = 3
            });

            Goods.Add(new Goods
            {
                Id = 7,
                Name = "Хлеб",
                Price = 40,
                CategoryId = 4
            });

            // Обновляем DataGrid
            GoodsDataGrid.ItemsSource = null;
            GoodsDataGrid.ItemsSource = Goods;
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