using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Models;
using WpfApp1.Services;
using WpfApp1.UserInterface;

namespace WpfApp1
{
    public partial class Profile : Window
    {
        private Users currentUser;
        private DatabaseService dbService = new DatabaseService();

        public Profile()
        {
            InitializeComponent();

            if (AppState.IsAuthenticated)
            {
                LoadUserData(AppState.CurrentUser.ID);
            }
            else
            {
                MessageBox.Show("Пользователь не авторизован", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        // Конструктор с параметром userId
        public Profile(int userId) : this()
        {
            // Этот конструктор теперь будет использовать логику основного конструктора
            // userId передается в LoadUserData через AppState
        }

        private async void LoadUserData(int userId)
        {
            try
            {
                currentUser = await dbService.GetUserByIdAsync(userId);

                if (currentUser != null)
                {
                    Dispatcher.Invoke(() =>
                    {
                        txtName.Text = currentUser.Name ?? "";
                        txtLastName.Text = currentUser.LastName ?? "";
                        // Убрал txtPatronymic, если его нет в XAML
                        // txtPatronymic.Text = currentUser.Patronymic ?? "";
                        txtEmail.Text = currentUser.Email ?? "";
                        txtPhone.Text = currentUser.PhoneNumber ?? "";
                        txtAddress.Text = currentUser.Address ?? "";
                        // Убрал txtLogin, если его нет в XAML
                        // txtLogin.Text = currentUser.Login ?? "";

                        if (currentUser.BirthDate != DateTime.MinValue)
                        {
                            txtBirthDate.Text = currentUser.BirthDate.ToString("dd-MM-yyyy");
                        }

                        // Убрал txtRole, если его нет в XAML
                        // txtRole.Text = currentUser.RoleName;
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void Save_Changes(object sender, RoutedEventArgs e)
        {
            try
            {
                if (currentUser == null)
                {
                    MessageBox.Show("Пользователь не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string newPassword = txtNewPassword.Text;
                string confirmPassword = txtConfirmPassword.Text;

                if (!string.IsNullOrEmpty(newPassword))
                {
                    if (newPassword != confirmPassword)
                    {
                        MessageBox.Show("Пароли не совпадают", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (newPassword.Length < 6)
                    {
                        MessageBox.Show("Пароль должен содержать минимум 6 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                // Обновляем данные пользователя
                currentUser.Name = txtName.Text.Trim();
                currentUser.LastName = txtLastName.Text.Trim();
                currentUser.Email = txtEmail.Text.Trim();
                currentUser.PhoneNumber = txtPhone.Text.Trim();
                currentUser.Address = txtAddress.Text.Trim();

                // Парсим дату рождения
                if (DateTime.TryParse(txtBirthDate.Text, out DateTime birthDate))
                {
                    currentUser.BirthDate = birthDate;
                }

                // Обновляем пароль, если он был изменен
                if (!string.IsNullOrEmpty(newPassword))
                {
                    bool passwordUpdated = await dbService.UpdatePasswordAsync(currentUser.ID, newPassword);
                    if (!passwordUpdated)
                    {
                        MessageBox.Show("Не удалось обновить пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }

                // Обновляем профиль пользователя
                bool profileUpdated = await dbService.UpdateUserProfileAsync(currentUser);

                if (profileUpdated)
                {
                    MessageBox.Show("Данные успешно сохранены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Сохраняем обновленные данные в AppState
                    AppState.CurrentUser = currentUser;

                    // Возвращаемся на предыдущую страницу
                    WindowManager.SaveWindowStats(this);
                    GoodsMain goods = new GoodsMain();
                    WindowManager.SetWindowStats(goods);
                    goods.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Не удалось сохранить данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Метод для возврата назад
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            WindowManager.SaveWindowStats(this);
            GoodsMain goods = new GoodsMain();
            WindowManager.SetWindowStats(goods);
            goods.Show();
            this.Close();
        }

        private void PhoneTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Можно добавить маску для телефона
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                string digits = new string(textBox.Text.Where(char.IsDigit).ToArray());
                if (digits.Length >= 11)
                {
                    textBox.Text = $"+7 ({digits.Substring(1, 3)}) {digits.Substring(4, 3)}-{digits.Substring(7, 2)}-{digits.Substring(9)}";
                    textBox.CaretIndex = textBox.Text.Length;
                }
            }
        }

        private void BirthDateTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Можно добавить маску для даты
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                string digits = new string(textBox.Text.Where(char.IsDigit).ToArray());
                if (digits.Length >= 8)
                {
                    textBox.Text = $"{digits.Substring(0, 2)}-{digits.Substring(2, 2)}-{digits.Substring(4, 4)}";
                    textBox.CaretIndex = textBox.Text.Length;
                }
            }
        }
    }
}