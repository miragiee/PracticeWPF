using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.EmployeeInterface;
using MySql.Data;
using MySqlConnector;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        // ЗАМЕНИТЕ НА ВАШУ СТРОКУ ПОДКЛЮЧЕНИЯ!
        private string connectionString = "Server=tompsons.beget.tech;Database=tompsons_stud21;Uid=tompsons_stud21;Pwd=123456Zz;Port=3306;SslMode=Preferred;CharSet=utf8;ConnectionTimeout=30;";

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginBox.Text.Trim();
            string password = PasswordBox.Text; // Для PasswordBox используем .Password

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Button loginButton = (Button)sender;
            object originalContent = loginButton.Content;

            try
            {
                loginButton.Content = "Проверка...";
                loginButton.IsEnabled = false;

                // ПРЯМОЙ ЗАПРОС С ПРАВИЛЬНЫМИ НАЗВАНИЯМИ СТОЛБЦОВ
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // Запрос 1: Проверяем пользователя
                    string query = @"
                        SELECT u.ID, u.RoleID, u.Login, u.Password 
                        FROM Users u 
                        WHERE u.Login = @Login AND u.Password = @Password";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Login", login);
                        command.Parameters.AddWithValue("@Password", password);

                        using (MySqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                // УСПЕШНЫЙ ВХОД!
                                int userId = reader.GetInt32(0);
                                int roleId = reader.GetInt32(1);
                                string dbLogin = reader.GetString(2);

                                MessageBox.Show($"Вход успешен!\nЛогин: {dbLogin}\nRoleID: {roleId}",
                                    "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                                // Закрываем первый DataReader
                                reader.Close();

                                // Получаем название роли из таблицы Role
                                string roleName = await GetRoleNameByIdAsync(connection, roleId);

                                // Открываем соответствующее окно
                                OpenUserInterface(roleName, roleId);
                            }
                            else
                            {
                                MessageBox.Show($"Неверный логин или пароль\n\nВведено:\nЛогин: {login}\nПароль: {password}",
                                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
            catch (MySqlException sqlEx)
            {
                MessageBox.Show($"Ошибка БД: {sqlEx.Message}", "Ошибка БД");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
            }
            finally
            {
                loginButton.Content = originalContent;
                loginButton.IsEnabled = true;
            }
        }

        // Метод для получения названия роли по ID
        private async Task<string> GetRoleNameByIdAsync(MySqlConnection connection, int roleId)
        {
            try
            {
                string query = "SELECT RoleName FROM Role WHERE RoleID = @RoleID";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoleID", roleId);
                    var result = await command.ExecuteScalarAsync();
                    return result?.ToString() ?? $"Роль #{roleId}";
                }
            }
            catch
            {
                return $"Роль #{roleId}";
            }
        }

        private void OpenUserInterface(string roleName, int roleId)
        {
            // Приоритет по roleName, если пусто - по roleId
            string role = (roleName?.ToLower() ?? "").Replace(" ", "");

            if (string.IsNullOrEmpty(role))
            {
                role = roleId.ToString();
            }

            switch (role)
            {
                case "1":
                case "администратор":
                case "admin":
                    try
                    {
                        AdminPanel adminPanel = new AdminPanel();
                        adminPanel.Show();
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Не удалось открыть AdminPanel: {ex.Message}", "Ошибка");
                    }
                    break;

                case "2":
                case "сотрудник":
                case "employee":
                    // Для сотрудника определяем конкретную роль по логину или другим данным
                    // Пока открываем кассира для теста
                    try
                    {
                        CashierInterface cashierInterface = new CashierInterface();
                        cashierInterface.Show();
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Не удалось открыть CashierInterface: {ex.Message}", "Ошибка");
                    }
                    break;

                case "3":
                case "пользователь":
                case "user":
                    try
                    {
                        GoodsMain goodsMain = new GoodsMain();
                        goodsMain.Show();
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Не удалось открыть GoodsMain: {ex.Message}", "Ошибка");
                    }
                    break;

                default:
                    MessageBox.Show($"Неизвестная роль: {roleName} (ID: {roleId})", "Ошибка");
                    break;
            }
        }

        private void MoveToRegister(object sender, RoutedEventArgs e)
        {
            try
            {
                Register register = new Register();
                register.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось открыть Register: {ex.Message}", "Ошибка");
            }
        }

        // Добавим тестовую кнопку для проверки данных
        private void TestDataButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Тестовые данные из БД:\n\n" +
                          "1. Логин: Admin, Пароль: 123456 (Администратор)\n" +
                          "2. Логин: Cashier2, Пароль: 123456 (Кассир)\n" +
                          "3. Логин: Cashier1, Пароль: 123456 (Кассир)\n\n" +
                          "Роли в таблице Role:\n" +
                          "1 - Администратор\n" +
                          "2 - Сотрудник\n" +
                          "3 - Пользователь",
                          "Тестовые данные");
        }

        // Метод для тестирования подключения к БД
        private void TestConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Проверяем таблицу Users
                    string query = "SELECT COUNT(*) FROM Users";
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        int count = (int)cmd.ExecuteScalar();
                        MessageBox.Show($"Подключение успешно!\nВ таблице Users: {count} записей", "Успех");
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения: {ex.Message}\n\nСтрока подключения: {connectionString}",
                    "Ошибка подключения");
            }
        }

        private void Login_Focused(object sender, RoutedEventArgs e)
        {
            if (LoginBox.Text == "Почта | Логин")
            {
                LoginBox.Text = string.Empty;
                LoginBox.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void LoginNotFocused(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(LoginBox.Text))
            {
                LoginBox.Text = "Почта | Логин";
                LoginBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        private void Password_Focused(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Text == "Пароль")
            {
                PasswordBox.Text = string.Empty;
                PasswordBox.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void PasswordNotFocused(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordBox.Text))
            {
                PasswordBox.Text = "Пароль";
                PasswordBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }
    }
}