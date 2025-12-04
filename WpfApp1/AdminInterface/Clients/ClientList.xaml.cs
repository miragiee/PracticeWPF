using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WpfApp1.Models;
using WpfApp1.Services;
using WpfApp1.UserInterface;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WpfApp1
{
    public partial class ClientList : Window, INotifyPropertyChanged
    {
        private DatabaseService dbService = new DatabaseService();
        private ObservableCollection<Users> _clients;

        public ObservableCollection<Users> Clients
        {
            get => _clients;
            set
            {
                _clients = value;
                OnPropertyChanged(nameof(Clients));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ClientList()
        {
            InitializeComponent();
            Clients = new ObservableCollection<Users>();
            DataContext = this;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadClients();
        }

        private async Task LoadClients()
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

                // Загружаем клиентов с RoleId = 3
                var clients = await dbService.GetUsersByRoleAsync(3);

                // Очищаем и заполняем коллекцию
                Clients.Clear();
                foreach (var client in clients)
                {
                    Clients.Add(client);
                }

                // Обновляем ItemsSource
                ClientListDataGrid.ItemsSource = null;
                ClientListDataGrid.ItemsSource = Clients;

                // Если данных нет, показываем сообщение
                if (Clients.Count == 0)
                {
                    MessageBox.Show("В базе данных нет клиентов с ролью 3",
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
                MessageBox.Show($"Ошибка загрузки клиентов: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                LoadTestData();
            }
        }

        private void LoadTestData()
        {
            Clients.Clear();

            // Тестовые данные
            Clients.Add(new Users
            {
                Id = 1,
                RoleId = 3,
                Login = "ivanov_client",
                Name = "Иван",
                LastName = "Иванов",
                Patronymic = "Иванович",
                PhoneNumber = "+7 (495) 123-45-67",
                Email = "ivanov@client.com",
                BirthDate = new DateTime(1985, 5, 15),
                Address = "Москва, ул. Ленина, 10"
            });

            Clients.Add(new Users
            {
                Id = 2,
                RoleId = 3,
                Login = "petrova_client",
                Name = "Мария",
                LastName = "Петрова",
                Patronymic = "Сергеевна",
                PhoneNumber = "+7 (812) 987-65-43",
                Email = "petrova@client.ru",
                BirthDate = new DateTime(1990, 8, 22),
                Address = "Санкт-Петербург, Невский пр-т, 25"
            });

            // Обновляем DataGrid
            ClientListDataGrid.ItemsSource = null;
            ClientListDataGrid.ItemsSource = Clients;
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            WindowManager.SaveWindowStats(this);
            AdminPanel adminPanel = new AdminPanel();
            WindowManager.SetWindowStats(adminPanel);
            adminPanel.Show();
            this.Close();
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            // Переименовываем заголовки колонок
            if (e.PropertyName == "Id")
            {
                e.Column.Header = "Номер";
                e.Column.Width = 110;
            }
            else if (e.PropertyName == "LastName")
            {
                e.Column.Header = "Фамилия";
                e.Column.Width = 240;
            }
            else if (e.PropertyName == "Name")
            {
                e.Column.Header = "Имя";
                e.Column.Width = 240;
            }
            else if (e.PropertyName == "Patronymic")
            {
                e.Column.Header = "Отчество";
                e.Column.Width = 240;
            }
            else if (e.PropertyName == "PhoneNumber")
            {
                e.Column.Header = "Телефон";
                e.Column.Width = 240;
            }
            else if (e.PropertyName == "Email")
            {
                e.Column.Header = "Email";
                e.Column.Width = 300;
            }
            else if (e.PropertyName == "BirthDate")
            {
                e.Column.Header = "Дата рождения";
                e.Column.Width = 200;
                // Форматируем дату
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd.MM.yyyy";
            }
            else if (e.PropertyName == "Address")
            {
                e.Column.Header = "Адрес";
                e.Column.Width = 400;
            }
            else if (e.PropertyName == "Login")
            {
                e.Column.Header = "Логин";
                e.Column.Width = 170;
            }
            else if (e.PropertyName == "RoleId")
            {
                e.Column.Header = "Роль";
                e.Column.Width = 100;
            }
            else
            {
                // Скрываем только пароль
                if (e.PropertyName == "Password")
                {
                    e.Cancel = true;
                }
            }
        }
    }
}