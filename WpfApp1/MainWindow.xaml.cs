using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.EmployeeInterface;
using WpfApp1.UserInterface;
using WpfApp1.Models; // Добавьте эту директиву
using MySql.Data.MySqlClient;

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
                    r.RoleName
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
                                int userId = userReader.GetInt32(0);
                                int roleId = userReader.GetInt32(1);
                                string userName = userReader.IsDBNull(3) ? "" : userReader.GetString(3);
                                string userLastName = userReader.IsDBNull(4) ? "" : userReader.GetString(4);
                                string roleName = userReader.IsDBNull(5) ? "" : userReader.GetString(5);

                                userReader.Close();

                                // 2. Определяем интерфейс в зависимости от роли
                                switch (roleId)
                                {
                                    case 1: // Администратор
                                        OpenAdminInterface();
                                        break;

                                    case 2: // Сотрудник
                                            // Определяем должность сотрудника
                                        await OpenEmployeeInterfaceByPositionAsync(connection, userId, userName);
                                        break;

                                    case 3: // Пользователь
                                        OpenUserInterface();
                                        break;

                                    default:
                                        MessageBox.Show($"Неизвестная роль: {roleName}", "Ошибка");
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

        // Новый метод для определения интерфейса по должности
        // Новый метод для определения интерфейса по должности
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

        // Метод для получения списка должностей
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
                            positions.Add(reader.GetString(0));
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Игнорируем ошибки
            }

            return positions;
        }

        // Метод для определения должности по логину (если нужно)
        private string DeterminePositionByLogin(int userId, MySqlConnection connection)
        {
            // Здесь можно добавить логику определения должности по логину
            // Например, если логин содержит "baker", то пекарь и т.д.

            // Пока возвращаем пустую строку
            return "";
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

        private async Task OpenEmployeeInterfaceAsync(MySqlConnection connection, string positions, int userId)
        {
            // Если есть конкретные должности
            if (!string.IsNullOrEmpty(positions))
            {
                var positionsList = positions.Split(',').Select(p => p.Trim().ToLower()).ToList();

                // Проверяем должности в порядке приоритета
                if (positionsList.Contains("Пекарь"))
                {
                    OpenBakerInterface();
                    return;
                }
                else if (positionsList.Contains("Кассир"))
                {
                    OpenCashierInterface();
                    return;
                }
                else if (positionsList.Contains("Доставщик"))
                {
                    OpenDeliveryInterface();
                    return;
                }
            }

            // Если должности не указаны или не распознаны, определяем по логину
            string mainPosition = await GetMainPositionForUserAsync(connection, userId);

            switch (mainPosition?.ToLower())
            {
                case "Пекарь":
                    OpenBakerInterface();
                    break;

                case "Кассир":
                    OpenCashierInterface();
                    break;

                case "Доставщик":
                    OpenDeliveryInterface();
                    break;
            }
        }

        private async Task<string> GetMainPositionForUserAsync(MySqlConnection connection, int userId)
        {
            try
            {
                string query = @"
                    SELECT p.Name 
                    FROM UserPost up
                    JOIN Postlist p ON up.PostID = p.Id
                    WHERE up.UserID = @UserID
                    LIMIT 1";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);
                    var result = await command.ExecuteScalarAsync();
                    return result?.ToString();
                }
            }
            catch
            {
                return null;
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