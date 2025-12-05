using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp1.Models;
using WpfApp1.Services;
using WpfApp1.UserInterface;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для AddEmployee.xaml
    /// </summary>
    public partial class AddEmployee : Window
    {
        private DatabaseService _databaseService;
        public AddEmployee()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            WindowManager.SaveWindowStats(this);
            AdminPanel adminPanel = new AdminPanel();
            WindowManager.SetWindowStats(adminPanel);
            adminPanel.Show();
            this.Close();
        }

        public async void AddButton(object sender, RoutedEventArgs e)
        {
            try
            {
                // Валидация данных
                if (string.IsNullOrWhiteSpace(nameBox.Text) || nameBox.Text == "И  М  Я")
                {
                    MessageBox.Show("Введите имя сотрудника", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(lastnameBox.Text) || lastnameBox.Text == "Ф  А  М  И  Л  И  Я")
                {
                    MessageBox.Show("Введите фамилию сотрудника", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!DateTime.TryParse(birthDateBox.Text, out DateTime birthDate) || birthDateBox.Text == "Д  А  Т  А      Р  О  Ж  Д  Е  Н  И  Я")
                {
                    MessageBox.Show("Введите корректную дату рождения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Определяем RoleId (предположим, что сотрудник имеет роль 2, но лучше получить из базы)
                // В вашей базе данных нужно уточнить ID роли для сотрудников
                int roleId = 2; // ID роли "Employee" - нужно уточнить по вашей базе данных

                // Создаем объект пользователя
                var newUser = new Users
                {
                    RoleID = roleId,
                    Name = nameBox.Text.Trim(),
                    LastName = lastnameBox.Text.Trim(),
                    Patronymic = patronymicBox.Text == "О  Т  Ч  Е  С  Т  В  О" ? null : patronymicBox.Text.Trim(),
                    PhoneNumber = phoneBox.Text == "Т  Е  Л  Е  Ф  О  Н" ? null : phoneBox.Text.Trim(),
                    Email = emailBox.Text == "П  О  Ч  Т  А" ? null : emailBox.Text.Trim(),
                    BirthDate = birthDate,
                    // Генерируем логин и пароль (можно настроить по-другому)
                    Login = GenerateLogin(nameBox.Text.Trim(), lastnameBox.Text.Trim()),
                    Password = GeneratePassword(),
                    Address = string.Empty // Или можно добавить поле для адреса в форме
                };

                // Добавляем пользователя в базу данных
                bool success = await _databaseService.CreateUserAsync(newUser);

                if (success)
                {
                    MessageBox.Show("Сотрудник успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Очищаем поля
                    ClearFields();

                    // Если нужно, получаем все должности из базы и добавляем связь
                    // Это зависит от вашей структуры базы данных
                    await AddPostAndSalaryInfo(newUser, postBox.Text, salaryBox.Text);
                }
                else
                {
                    MessageBox.Show("Ошибка при добавлении сотрудника", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GenerateLogin(string firstName, string lastName)
        {
            // Генерация логина на основе имени и фамилии
            // Например: ivanov.i
            return $"{lastName.ToLower()}.{firstName.Substring(0, 1).ToLower()}";
        }

        private string GeneratePassword()
        {
            // Генерация случайного пароля (можно заменить на более сложную логику)
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private async Task AddPostAndSalaryInfo(Users user, string postName, string salaryText)
        {
            try
            {
                // Здесь нужно добавить логику для добавления должности и зарплаты
                // Это зависит от структуры ваших таблиц Postlist и UserPost

                // Примерная логика:
                // 1. Получить все должности из базы
                // 2. Найти нужную должность или создать новую
                // 3. Создать связь между пользователем и должностью в таблице UserPost

                // Для этого вам понадобятся дополнительные методы в DatabaseService
                // и, возможно, создание отдельного сервиса для работы с должностями

                // Временное решение - просто сохраняем информацию
                Console.WriteLine($"Добавлен сотрудник: {user.LastName} {user.Name}, Должность: {postName}, Зарплата: {salaryText}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении информации о должности: {ex.Message}", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ClearFields()
        {
            nameBox.Text = "И  М  Я";
            lastnameBox.Text = "Ф  А  М  И  Л  И  Я";
            patronymicBox.Text = "О  Т  Ч  Е  С  Т  В  О";
            birthDateBox.Text = "Д  А  Т  А      Р  О  Ж  Д  Е  Н  И  Я";
            postBox.Text = "Д  О  Л  Ж  Н  О  С  Т  Ь";
            salaryBox.Text = "З  А  Р  П  Л  А  Т  А";
            emailBox.Text = "П  О  Ч  Т  А";
            phoneBox.Text = "Т  Е  Л  Е  Ф  О  Н";
        }
    }
}