using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.EmployeeInterface;
using WpfApp1.Models;
using WpfApp1.UserInterface;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private string connectionString = "Server=tompsons.beget.tech;Database=tompsons_stud21;Uid=tompsons_stud21;Pwd=123456Zz;Port=3306;SslMode=Preferred;CharSet=utf8;ConnectionTimeout=30;";

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginBox.Text.Trim();
            string password = PasswordBox.Text;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Button loginButton = (Button)sender;
            string originalContent = loginButton.Content.ToString();

            try
            {
                loginButton.Content = "Проверка...";
                loginButton.IsEnabled = false;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // 1. Находим пользователя и его роль
                    string userQuery = @"
                        SELECT 
                            u.ID, 
                            u.RoleID, 
                            u.Login,
                            u.Name,
                            u.LastName,
                            r.RoleName,
                            u.Email,
                            u.PhoneNumber,
                            u.BirthDate,
                            u.Address
                        FROM Users u
                        LEFT JOIN Role r ON u.RoleID = r.RoleID
                        WHERE u.Login = @Login AND u.Password = @Password";

                    using (MySqlCommand userCmd = new MySqlCommand(userQuery, connection))
                    {
                        userCmd.Parameters.AddWithValue("@Login", login);
                        userCmd.Parameters.AddWithValue("@Password", password);

                        using (MySqlDataReader userReader = (MySqlDataReader)await userCmd.ExecuteReaderAsync())
                        {
                            if (await userReader.ReadAsync())
                            {
                                // Создаем объект пользователя
                                Users user = new Users
                                {
                                    ID = userReader.GetInt32(0),
                                    RoleID = userReader.GetInt32(1),
                                    Login = userReader.GetString(2),
                                    Name = userReader.IsDBNull(3) ? "" : userReader.GetString(3),
                                    LastName = userReader.IsDBNull(4) ? "" : userReader.GetString(4),
                                    Email = userReader.IsDBNull(6) ? "" : userReader.GetString(6),
                                    PhoneNumber = userReader.IsDBNull(7) ? "" : userReader.GetString(7),
                                    Address = userReader.IsDBNull(9) ? "" : userReader.GetString(9)
                                };

                                if (!userReader.IsDBNull(5))
                                {
                                    user.Role = new Role
                                    {
                                        RoleName = userReader.GetString(5)
                                    };
                                }

                                if (!userReader.IsDBNull(8))
                                {
                                    user.BirthDate = userReader.GetDateTime(8);
                                }

                                // Закрываем DataReader перед новым запросом
                                userReader.Close();

                                // Сохраняем пользователя в статическом классе
                                AppState.CurrentUser = user;

                                // 2. Определяем интерфейс в зависимости от роли
                                switch (user.RoleID)
                                {
                                    case 1: // Администратор
                                        OpenAdminInterface();
                                        break;

                                    case 2: // Сотрудник
                                        // Определяем должность сотрудника
                                        await OpenEmployeeInterfaceByPositionAsync(connection, user.ID, user.Name);
                                        break;

                                    case 3: // Пользователь
                                        OpenUserInterface();
                                        break;

                                    default:
                                        MessageBox.Show($"Неизвестная роль: {user.RoleName}", "Ошибка");
                                        break;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Неверный логин или пароль", "Ошибка",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
            catch (MySqlException sqlEx)
            {
                MessageBox.Show($"Ошибка MySQL: {sqlEx.Message}", "Ошибка БД");
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

        // Метод для определения интерфейса по должности
        private async Task OpenEmployeeInterfaceByPositionAsync(MySqlConnection connection, int userId, string userName)
        {
            // 1. Получаем все должности пользователя
            List<string> positions = await GetUserPositionsListAsync(connection, userId);

            // 2. Если должности есть, определяем по ним
            if (positions.Count > 0)
            {
                // Проверяем в порядке приоритета
                foreach (var position in positions)
                {
                    string lowerPosition = position.ToLower();

                    // Проверяем русские названия должностей
                    if (lowerPosition.Contains("пекарь") || lowerPosition.Contains("4") || lowerPosition.Contains("baker"))
                    {
                        MessageBox.Show($"Открываю интерфейс пекаря для {userName}", "Информация");
                        OpenBakerInterface();
                        return;
                    }
                    else if (lowerPosition.Contains("кассир") || lowerPosition.Contains("2") || lowerPosition.Contains("cashier"))
                    {
                        MessageBox.Show($"Открываю интерфейс кассира для {userName}", "Информация");
                        OpenCashierInterface();
                        return;
                    }
                    else if (lowerPosition.Contains("доставщик") || lowerPosition.Contains("3") || lowerPosition.Contains("delivery"))
                    {
                        MessageBox.Show($"Открываю интерфейс доставки для {userName}", "Информация");
                        OpenDeliveryInterface();
                        return;
                    }
                }

                // Если ни одна стандартная должность не найдена
                MessageBox.Show($"У сотрудника {userName} назначены нестандартные должности: {string.Join(", ", positions)}\nОткрываю интерфейс кассира по умолчанию.", "Информация");
                OpenCashierInterface();
            }
            else
            {
                // Если нет записей в UserPost, открываем кассира по умолчанию
                MessageBox.Show($"Для сотрудника {userName} не назначены должности.\nОткрываю интерфейс кассира по умолчанию.", "Информация");
                OpenCashierInterface();
            }
        }

        // Метод для получения списка должностей пользователя
        private async Task<List<string>> GetUserPositionsListAsync(MySqlConnection connection, int userId)
        {
            var positions = new List<string>();

            try
            {
                string query = @"
                    SELECT p.Name
                    FROM UserPost up
                    JOIN Postlist p ON up.PostID = p.Id
                    WHERE up.UserID = @UserID";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);

                    using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                positions.Add(reader.GetString(0));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении должностей: {ex.Message}");
            }

            return positions;
        }

        // Новый метод для получения ID пользователя по логину (если понадобится)
        private async Task<int> GetUserIdByLoginAsync(string login)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"SELECT ID FROM Users WHERE Login = @Login OR Email = @Login";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Login", login);

                        object result = await command.ExecuteScalarAsync();
                        if (result != null && result != DBNull.Value)
                        {
                            return Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении ID пользователя: {ex.Message}");
            }

            return 0;
        }

        private void OpenAdminInterface()
        {
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
        }

        private void OpenBakerInterface()
        {
            try
            {
                BakerInterface bakerInterface = new BakerInterface();
                bakerInterface.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось открыть BakerInterface: {ex.Message}", "Ошибка");
            }
        }

        private void OpenCashierInterface()
        {
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
        }

        private void OpenDeliveryInterface()
        {
            try
            {
                DeliveryInterface deliveryInterface = new DeliveryInterface();
                deliveryInterface.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось открыть DeliveryInterface: {ex.Message}", "Ошибка");
            }
        }

        private void OpenUserInterface()
        {
            try
            {
                // Теперь GoodsMain будет знать, какой пользователь авторизован
                GoodsMain goodsMain = new GoodsMain();
                goodsMain.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось открыть GoodsMain: {ex.Message}", "Ошибка");
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