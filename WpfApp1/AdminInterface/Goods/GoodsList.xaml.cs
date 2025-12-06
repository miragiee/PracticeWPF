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
using System.Diagnostics;

namespace WpfApp1
{
    public partial class GoodsList : Window, INotifyPropertyChanged
    {
        private DatabaseService dbService = new DatabaseService();
        private ObservableCollection<Goods> _goods;

        public ObservableCollection<Goods> Goodss
        {
            get => _goods;
            set
            {
                if (_goods != value)
                {
                    _goods = value;
                    OnPropertyChanged(nameof(Goodss));
                    Debug.WriteLine($"Свойство Goods изменено: {_goods?.Count ?? 0} элементов");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public GoodsList()
        {
            InitializeComponent();

            // Инициализируем коллекцию ПЕРЕД установкой DataContext
            Goodss = new ObservableCollection<Goods>();

            // Устанавливаем DataContext на самого себя
            DataContext = this;

            // Для отладки
            Debug.WriteLine($"Конструктор GoodsList: DataContext установлен = {DataContext != null}");
            Debug.WriteLine($"Конструктор GoodsList: Goods коллекция создана = {_goods != null}");
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            Debug.WriteLine($"Событие PropertyChanged вызвано для: {propertyName}");
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Window_Loaded начал выполнение");
            await LoadGoods();
        }

        private async Task LoadGoods()
        {
            try
            {
                Debug.WriteLine("Начало загрузки товаров...");

                // Проверяем подключение
                bool isConnected = await dbService.TestConnectionAsync();
                Debug.WriteLine($"Подключение к БД: {isConnected}");

                if (!isConnected)
                {
                    MessageBox.Show("Ошибка подключения к базе данных. Будут загружены тестовые данные.",
                                  "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Error);
                    LoadTestData();
                    return;
                }

                // Загружаем товары
                var goods = await dbService.GetAllGoodsAsync();
                Debug.WriteLine($"Загружено товаров из БД: {goods?.Count ?? 0}");

                // Создаем новую коллекцию (это вызовет PropertyChanged)
                var newCollection = new ObservableCollection<Goods>();

                if (goods != null && goods.Count > 0)
                {
                    foreach (var item in goods)
                    {
                        Debug.WriteLine($"Добавляем товар: ID={item.ID}, Name={item.Name}");
                        newCollection.Add(item);
                    }

                    // Меняем всю коллекцию - это вызовет PropertyChanged
                    Goodss = newCollection;

                    Debug.WriteLine($"Установлена новая коллекция с {Goodss.Count} элементами");
                }
                else
                {
                    MessageBox.Show("В базе данных нет товаров",
                        "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadTestData();
                }
            }
            catch (MySqlException mysqlEx)
            {
                Debug.WriteLine($"MySQL Error: {mysqlEx.Message}");

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
                Debug.WriteLine($"Ошибка загрузки товаров: {ex.Message}");
                MessageBox.Show($"Ошибка загрузки товаров: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                LoadTestData();
            }
        }

        private void LoadTestData()
        {
            Debug.WriteLine("Загрузка тестовых данных...");

            var testCollection = new ObservableCollection<Goods>();

            // Тестовые данные
            testCollection.Add(new Goods
            {
                ID = 1,
                Name = "Лаваш",
                Price = 60,
                CategoryID = 1,
            });

            testCollection.Add(new Goods
            {
                ID = 4,
                Name = "Апельсин",
                Price = 100,
                CategoryID = 2
            });

            testCollection.Add(new Goods
            {
                ID = 5,
                Name = "Бананы",
                Price = 80,
                CategoryID = 2
            });

            testCollection.Add(new Goods
            {
                ID = 6,
                Name = "Молоко",
                Price = 70,
                CategoryID = 3
            });

            testCollection.Add(new Goods
            {
                ID = 7,
                Name = "Хлеб",
                Price = 40,
                CategoryID = 4
            });

            // Меняем всю коллекцию
            Goodss = testCollection;

            Debug.WriteLine($"Загружено тестовых товаров: {Goodss.Count}");
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