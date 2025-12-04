using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using WpfApp1.Models;
using WpfApp1.Services;
using WpfApp1.UserInterface;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace WpfApp1
{
    public partial class EmployeeList : Window, INotifyPropertyChanged
    {
        private DatabaseService dbService = new DatabaseService();
        private ObservableCollection<Users> _employees;

        public ObservableCollection<Users> Employees
        {
            get => _employees;
            set
            {
                _employees = value;
                OnPropertyChanged(nameof(Employees));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public EmployeeList()
        {
            InitializeComponent();
            Employees = new ObservableCollection<Users>();
            DataContext = this;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadEmployees();
        }

        private async Task LoadEmployees()
        {
            try
            {
                // Сначала проверяем подключение
                bool isConnected = await dbService.TestConnectionAsync();
                if (!isConnected)
                {
                    MessageBox.Show("Ошибка подключения к базе данных. Будут загружены тестовые данные. Для исправления ошибки проверьте:\n" +
                                  "1. Доступность интернета\n" +
                                  "2. Настройки подключения\n" +
                                  "3. Разрешения удаленного доступа к БД",
                                  "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Error);
                    LoadTestData();
                }

                // Загружаем сотрудников с RoleId = 2
                var employees = await dbService.GetUsersByRoleAsync(2);

                // Очищаем и заполняем коллекцию
                Employees.Clear();
                foreach (var employee in employees)
                {
                    Employees.Add(employee);
                }

                // Обновляем ItemsSource
                EmployeeListDataGrid.ItemsSource = null;
                EmployeeListDataGrid.ItemsSource = Employees;

                // Если данных нет, показываем сообщение
                if (Employees.Count == 0)
                {
                    MessageBox.Show("В базе данных нет сотрудников с ролью 2",
                        "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Показываем количество загруженных записей
                    MessageBox.Show($"Загружено {Employees.Count} сотрудников",
                        "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (MySqlException mysqlEx)
            {
                string errorMessage = $"Ошибка MySQL ({mysqlEx.Number}):\n{mysqlEx.Message}";

                if (mysqlEx.Number == 1042) // Ошибка подключения
                {
                    errorMessage += "\n\nПроверьте:\n" +
                                   "1. Доступность сервера lolek.beget.com\n" +
                                   "2. Правильность логина и пароля\n" +
                                   "3. Разрешения на удаленное подключение";
                }

                MessageBox.Show(errorMessage,
                    "Ошибка базы данных", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки сотрудников: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadTestData()
        {
            Employees.Clear();

            // Добавляем тестовые данные с более короткими значениями для отладки
            Employees.Add(new Users
            {
                Id = 1,
                RoleId = 2,
                Login = "ivanov_ii",
                Name = "Иван",
                LastName = "Иванов",
                Patronymic = "Иванович",
                PhoneNumber = "+7 (495) 123-45-67",
                Email = "ivanov@example.com",
                BirthDate = new DateTime(1985, 5, 15),
                Address = "Москва, ул. Ленина, 10"
            });

            Employees.Add(new Users
            {
                Id = 2,
                RoleId = 2,
                Login = "petrova_ms",
                Name = "Мария",
                LastName = "Петрова",
                Patronymic = "Сергеевна",
                PhoneNumber = "+7 (812) 987-65-43",
                Email = "petrova@example.ru",
                BirthDate = new DateTime(1990, 8, 22),
                Address = "Санкт-Петербург, Невский пр-т, 25"
            });

            Employees.Add(new Users
            {
                Id = 3,
                RoleId = 2,
                Login = "sidorov_ap",
                Name = "Алексей",
                LastName = "Сидоров",
                Patronymic = "Петрович",
                PhoneNumber = "+7 (343) 456-78-90",
                Email = "sidorov@company.com",
                BirthDate = new DateTime(1978, 11, 30),
                Address = "Екатеринбург, ул. Мира, 15"
            });

            // Обновляем DataGrid
            EmployeeListDataGrid.ItemsSource = null;
            EmployeeListDataGrid.ItemsSource = Employees;
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            WindowManager.SaveWindowStats(this);
            AdminPanel adminPanel = new AdminPanel();
            WindowManager.SetWindowStats(adminPanel);
            adminPanel.Show();
            this.Close();
        }

        private void DataGrid_AutoGeneratingColumn(object sender, System.Windows.Controls.DataGridAutoGeneratingColumnEventArgs e)
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
                (e.Column as System.Windows.Controls.DataGridTextColumn).Binding.StringFormat = "dd.MM.yyyy";
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
            else
            {
                // Скрываем только пароль, остальные колонки оставляем
                if (e.PropertyName == "Password")
                {
                    e.Cancel = true;
                }
            }
        }
    }
}