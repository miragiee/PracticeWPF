using System;
using System.Windows;
using WpfApp1.Models;
using WpfApp1.Services;
using WpfApp1.UserInterface;

namespace WpfApp1
{
    public partial class Register : Window
    {
        private DatabaseService _databaseService;

        public Register()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Валидация данных
                if (string.IsNullOrEmpty(LoginBox.Text) ||
                    string.IsNullOrEmpty(EmailBox.Text) ||
                    string.IsNullOrEmpty(NameBox.Text) ||
                    string.IsNullOrEmpty(LastNameBox.Text))
                {
                    MessageBox.Show("Заполните все обязательные поля", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(PasswordBox.Text))
                {
                    MessageBox.Show("Введите пароль", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (PasswordBox.Text != ConfirmPasswordBox.Text)
                {
                    MessageBox.Show("Пароли не совпадают", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (PasswordBox.Text.Length < 6)
                {
                    MessageBox.Show("Пароль должен содержать минимум 6 символов", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Проверка существования пользователя
                bool userExists = await _databaseService.UserExistsAsync(LoginBox.Text, EmailBox.Text);
                if (userExists)
                {
                    MessageBox.Show("Пользователь с таким логином или email уже существует", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Получаем роль "User" (обычный пользователь)
                var userRole = await _databaseService.GetRoleByNameAsync("User");
                if (userRole == null)
                {
                    // Если роли "User" нет, создаем нового пользователя с RoleId = 3 (судя по данным из image.png)
                    userRole = new Role { ID = 3, RoleName = "User" };
                }

                // Создаем объект пользователя
                var newUser = new Users
                {
                    Login = LoginBox.Text.Trim(),
                    Email = EmailBox.Text.Trim(),
                    Password = PasswordBox.Text,
                    Name = NameBox.Text.Trim(),
                    LastName = LastNameBox.Text.Trim(),
                    Patronymic = "",
                    PhoneNumber = PhoneBox.Text.Trim(),
                    BirthDate = DateTime.Now.AddYears(-18), // По умолчанию 18 лет
                    Address = "",
                    RoleID = userRole.ID
                };

                // Сохраняем пользователя в базе данных
                bool success = await _databaseService.CreateUserAsync(newUser);

                if (success)
                {
                    MessageBox.Show("Регистрация прошла успешно! Теперь вы можете войти в систему.", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    // Возвращаемся к окну входа
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Ошибка при регистрации. Попробуйте позже.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}