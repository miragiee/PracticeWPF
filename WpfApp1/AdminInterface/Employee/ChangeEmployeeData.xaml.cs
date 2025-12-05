using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Models;
using WpfApp1.Services;
using WpfApp1.UserInterface;

namespace WpfApp1
{
    public partial class ChangeEmployeeData : Window
    {
        private DatabaseService _databaseService;
        private List<Users> _employees;
        private Users _selectedEmployee;
        private List<Postlist> _posts;

        // Изменяем логику: true = поле будет изменяться, false = поле не будет изменяться
        private bool _changeName = true;
        private bool _changeLastName = true;
        private bool _changePatronymic = true;
        private bool _changePost = true;
        private bool _changeSalary = true;

        public ChangeEmployeeData()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
            Loaded += ChangeEmployeeData_Loaded;
        }

        private async void ChangeEmployeeData_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                // Загружаем только сотрудников (RoleId = 2)
                _employees = await GetEmployeesOnlyAsync();

                if (_employees == null || _employees.Count == 0)
                {
                    MessageBox.Show("Список сотрудников пуст", "Информация",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }

                await LoadPostsAsync();

                EmployeesComboBox.ItemsSource = _employees;
                EmployeesComboBox.SelectedValuePath = "Id";

                PostComboBox.ItemsSource = _posts;
                PostComboBox.DisplayMemberPath = "Name";
                PostComboBox.SelectedValuePath = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Метод для загрузки только сотрудников (RoleId = 2)
        private async Task<List<Users>> GetEmployeesOnlyAsync()
        {
            var employees = new List<Users>();

            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();
                    string query = "SELECT * FROM Users WHERE RoleId = 2"; // RoleId = 2 - сотрудники

                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            employees.Add(new Users
                            {
                                ID = reader.GetInt32("Id"),
                                RoleID = reader.GetInt32("RoleId"),
                                Login = GetSafeString(reader, "Login"),
                                Password = GetSafeString(reader, "Password"),
                                Name = GetSafeString(reader, "Name"),
                                LastName = GetSafeString(reader, "LastName"),
                                Patronymic = GetSafeString(reader, "Patronymic"),
                                PhoneNumber = GetSafeString(reader, "PhoneNumber"),
                                Email = GetSafeString(reader, "Email"),
                                BirthDate = reader.GetDateTime("BirthDate"),
                                Address = GetSafeString(reader, "Address")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetEmployeesOnlyAsync error: {ex.Message}");
                return new List<Users>();
            }

            return employees;
        }

        private async Task LoadPostsAsync()
        {
            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();
                    string query = "SELECT * FROM Postlist";

                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        _posts = new List<Postlist>();
                        while (await reader.ReadAsync())
                        {
                            _posts.Add(new Postlist
                            {
                                Id = reader.GetInt32("Id"),
                                Name = reader["Name"]?.ToString(),
                                Salary = reader.IsDBNull(reader.GetOrdinal("Salary")) ?
                                    (decimal?)null : reader.GetDecimal("Salary")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки должностей: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void EmployeesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EmployeesComboBox.SelectedValue != null && EmployeesComboBox.SelectedValue is int employeeId)
            {
                await LoadEmployeeDataAsync(employeeId);
            }
        }

        private async Task LoadEmployeeDataAsync(int employeeId)
        {
            try
            {
                _selectedEmployee = _employees?.FirstOrDefault(u => u.ID == employeeId);
                if (_selectedEmployee == null)
                {
                    // Загружаем сотрудника из базы данных с проверкой RoleId
                    using (var connection = DatabaseContext.GetConnection())
                    {
                        await connection.OpenAsync();

                        string userQuery = "SELECT * FROM Users WHERE Id = @Id AND RoleId = 2"; // Проверяем, что это сотрудник
                        using (var userCommand = new MySqlCommand(userQuery, connection))
                        {
                            userCommand.Parameters.AddWithValue("@Id", employeeId);

                            using (var userReader = await userCommand.ExecuteReaderAsync())
                            {
                                if (await userReader.ReadAsync())
                                {
                                    _selectedEmployee = new Users
                                    {
                                        ID = userReader.GetInt32("Id"),
                                        RoleID = userReader.GetInt32("RoleId"),
                                        Login = GetSafeString(userReader, "Login"),
                                        Password = GetSafeString(userReader, "Password"),
                                        Name = GetSafeString(userReader, "Name"),
                                        LastName = GetSafeString(userReader, "LastName"),
                                        Patronymic = GetSafeString(userReader, "Patronymic"),
                                        PhoneNumber = GetSafeString(userReader, "PhoneNumber"),
                                        Email = GetSafeString(userReader, "Email"),
                                        BirthDate = userReader.GetDateTime("BirthDate"),
                                        Address = GetSafeString(userReader, "Address")
                                    };

                                    // Добавляем в список сотрудников
                                    if (_employees == null)
                                        _employees = new List<Users>();
                                    _employees.Add(_selectedEmployee);

                                    // Обновляем ComboBox
                                    EmployeesComboBox.ItemsSource = null;
                                    EmployeesComboBox.ItemsSource = _employees;
                                }
                                else
                                {
                                    MessageBox.Show("Выбранный пользователь не является сотрудником или не найден", "Ошибка",
                                        MessageBoxButton.OK, MessageBoxImage.Warning);
                                    return;
                                }
                            }
                        }

                        if (_selectedEmployee != null)
                        {
                            await LoadEmployeePostAndSalaryAsync(connection, employeeId);
                        }
                    }
                }
                else
                {
                    // Проверяем, что выбранный пользователь - сотрудник
                    if (_selectedEmployee.RoleID != 2)
                    {
                        MessageBox.Show("Выбранный пользователь не является сотрудником", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                        _selectedEmployee = null;
                        return;
                    }

                    // Загружаем должность и зарплату сотрудника
                    using (var connection = DatabaseContext.GetConnection())
                    {
                        await connection.OpenAsync();
                        await LoadEmployeePostAndSalaryAsync(connection, employeeId);
                    }
                }

                if (_selectedEmployee != null)
                {
                    FirstNameTextBox.Text = _selectedEmployee.Name ?? "";
                    LastNameTextBox.Text = _selectedEmployee.LastName ?? "";
                    PatronymicTextBox.Text = _selectedEmployee.Patronymic ?? "";

                    ResetChangeFlags();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных сотрудника: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadEmployeePostAndSalaryAsync(MySqlConnection connection, int employeeId)
        {
            try
            {
                string postQuery = @"
                    SELECT p.* FROM UserPost up
                    LEFT JOIN Postlist p ON up.PostID = p.Id
                    WHERE up.UserID = @UserId";

                using (var postCommand = new MySqlCommand(postQuery, connection))
                {
                    postCommand.Parameters.AddWithValue("@UserId", employeeId);

                    using (var postReader = await postCommand.ExecuteReaderAsync())
                    {
                        if (await postReader.ReadAsync())
                        {
                            var post = new Postlist
                            {
                                Id = postReader.GetInt32("Id"),
                                Name = GetSafeString(postReader, "Name"),
                                Salary = postReader.IsDBNull(postReader.GetOrdinal("Salary")) ?
                                    (decimal?)null : postReader.GetDecimal("Salary")
                            };

                            // Ищем должность в списке и устанавливаем ее выбранной
                            foreach (var item in PostComboBox.Items)
                            {
                                if (item is Postlist pItem && pItem.Id == post.Id)
                                {
                                    PostComboBox.SelectedItem = item;
                                    break;
                                }
                            }
                            SalaryTextBox.Text = post.Salary?.ToString() ?? "";
                        }
                        else
                        {
                            PostComboBox.SelectedIndex = -1;
                            SalaryTextBox.Text = "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки должности: {ex.Message}");
                PostComboBox.SelectedIndex = -1;
                SalaryTextBox.Text = "";
            }
        }

        private string GetSafeString(DbDataReader reader, string columnName)
        {
            try
            {
                int columnIndex = reader.GetOrdinal(columnName);
                if (reader.IsDBNull(columnIndex))
                {
                    return null;
                }
                return reader.GetString(columnIndex);
            }
            catch
            {
                return null;
            }
        }

        private void ResetChangeFlags()
        {
            // По умолчанию ВСЕ поля будут изменяться
            _changeName = true;
            _changeLastName = true;
            _changePatronymic = true;
            _changePost = true;
            _changeSalary = true;

            // Устанавливаем текст кнопок в "НЕ МЕНЯТЬ" (т.е. поле БУДЕТ изменяться)
            NameChangeButton.Content = "Н Е   М Е Н Я Т Ь";
            LastNameChangeButton.Content = "Н Е   М Е Н Я Т Ь";
            PatronymicChangeButton.Content = "Н Е   М Е Н Я Т Ь";
            PostChangeButton.Content = "Н Е   М Е Н Я Т Ь";
            SalaryChangeButton.Content = "Н Е   М Е Н Я Т Ь";
        }

        private void NameDoNotChange_Click(object sender, RoutedEventArgs e)
        {
            // Инвертируем состояние: если было "НЕ МЕНЯТЬ" (true) -> станет "ИЗМЕНИТЬ" (false)
            _changeName = !_changeName;
            NameChangeButton.Content = _changeName ? "Н Е   М Е Н Я Т Ь" : "И З М Е Н И Т Ь";
        }

        private void LastNameDoNotChange_Click(object sender, RoutedEventArgs e)
        {
            _changeLastName = !_changeLastName;
            LastNameChangeButton.Content = _changeLastName ? "Н Е   М Е Н Я Т Ь" : "И З М Е Н И Т Ь";
        }

        private void PatronymicDoNotChange_Click(object sender, RoutedEventArgs e)
        {
            _changePatronymic = !_changePatronymic;
            PatronymicChangeButton.Content = _changePatronymic ? "Н Е   М Е Н Я Т Ь" : "И З М Е Н И Т Ь";
        }

        private void PostDoNotChange_Click(object sender, RoutedEventArgs e)
        {
            _changePost = !_changePost;
            PostChangeButton.Content = _changePost ? "Н Е   М Е Н Я Т Ь" : "И З М Е Н И Т Ь";
        }

        private void SalaryDoNotChange_Click(object sender, RoutedEventArgs e)
        {
            _changeSalary = !_changeSalary;
            SalaryChangeButton.Content = _changeSalary ? "Н Е   М Е Н Я Т Ь" : "И З М Е Н И Т Ь";
        }

        private async void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedEmployee == null)
            {
                MessageBox.Show("Выберите сотрудника для изменения", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Проверяем, что это сотрудник
            if (_selectedEmployee.RoleID != 2)
            {
                MessageBox.Show("Выбранный пользователь не является сотрудником", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();

                    using (var transaction = await connection.BeginTransactionAsync())
                    {
                        try
                        {
                            bool changesMade = false;

                            // Обновляем данные пользователя
                            if (_changeName || _changeLastName || _changePatronymic)
                            {
                                string updateUserQuery = @"
                                    UPDATE Users SET 
                                    Name = @Name,
                                    LastName = @LastName,
                                    Patronymic = @Patronymic
                                    WHERE Id = @Id AND RoleId = 2"; // Защита от изменения не-сотрудников

                                using (var command = new MySqlCommand(updateUserQuery, connection, transaction))
                                {
                                    command.Parameters.AddWithValue("@Id", _selectedEmployee.ID);
                                    command.Parameters.AddWithValue("@Name",
                                        _changeName ? FirstNameTextBox.Text : _selectedEmployee.Name);
                                    command.Parameters.AddWithValue("@LastName",
                                        _changeLastName ? LastNameTextBox.Text : _selectedEmployee.LastName);
                                    command.Parameters.AddWithValue("@Patronymic",
                                        _changePatronymic ? PatronymicTextBox.Text : _selectedEmployee.Patronymic);

                                    int rowsAffected = await command.ExecuteNonQueryAsync();
                                    if (rowsAffected > 0)
                                    {
                                        changesMade = true;
                                        // Обновляем объект в памяти
                                        if (_changeName) _selectedEmployee.Name = FirstNameTextBox.Text;
                                        if (_changeLastName) _selectedEmployee.LastName = LastNameTextBox.Text;
                                        if (_changePatronymic) _selectedEmployee.Patronymic = PatronymicTextBox.Text;
                                    }
                                }
                            }

                            // Обновляем должность и зарплату
                            if (_changePost || _changeSalary)
                            {
                                if (PostComboBox.SelectedItem is Postlist selectedPost)
                                {
                                    var postId = selectedPost.Id;

                                    // Проверяем, есть ли уже связь с должностью
                                    string checkQuery = @"
                                        SELECT COUNT(*) FROM UserPost up
                                        INNER JOIN Users u ON up.UserID = u.Id
                                        WHERE up.UserID = @UserId AND up.PostID = @PostId AND u.RoleId = 2";
                                    using (var checkCommand = new MySqlCommand(checkQuery, connection, transaction))
                                    {
                                        checkCommand.Parameters.AddWithValue("@UserId", _selectedEmployee.ID);
                                        checkCommand.Parameters.AddWithValue("@PostId", postId);

                                        long existingCount = Convert.ToInt64(await checkCommand.ExecuteScalarAsync());

                                        if (existingCount == 0 && _changePost)
                                        {
                                            // Удаляем старую связь
                                            string deleteQuery = @"
                                                DELETE up FROM UserPost up
                                                INNER JOIN Users u ON up.UserID = u.Id
                                                WHERE up.UserID = @UserId AND u.RoleId = 2";
                                            using (var deleteCommand = new MySqlCommand(deleteQuery, connection, transaction))
                                            {
                                                deleteCommand.Parameters.AddWithValue("@UserId", _selectedEmployee.ID);
                                                await deleteCommand.ExecuteNonQueryAsync();
                                            }

                                            // Добавляем новую связь
                                            string insertQuery = "INSERT INTO UserPost (UserID, PostID) VALUES (@UserId, @PostId)";
                                            using (var insertCommand = new MySqlCommand(insertQuery, connection, transaction))
                                            {
                                                insertCommand.Parameters.AddWithValue("@UserId", _selectedEmployee.ID);
                                                insertCommand.Parameters.AddWithValue("@PostId", postId);
                                                await insertCommand.ExecuteNonQueryAsync();
                                            }
                                        }
                                    }

                                    // Обновляем зарплату
                                    if (_changeSalary && decimal.TryParse(SalaryTextBox.Text, out decimal newSalary))
                                    {
                                        string updateSalaryQuery = "UPDATE Postlist SET Salary = @Salary WHERE Id = @Id";
                                        using (var salaryCommand = new MySqlCommand(updateSalaryQuery, connection, transaction))
                                        {
                                            salaryCommand.Parameters.AddWithValue("@Id", postId);
                                            salaryCommand.Parameters.AddWithValue("@Salary", newSalary);
                                            await salaryCommand.ExecuteNonQueryAsync();
                                        }
                                    }

                                    changesMade = true;
                                }
                            }

                            await transaction.CommitAsync();

                            if (changesMade)
                            {
                                MessageBox.Show("Изменения успешно сохранены", "Успех",
                                    MessageBoxButton.OK, MessageBoxImage.Information);

                                // Обновляем список сотрудников
                                await LoadDataAsync();
                                ResetChangeFlags();
                            }
                            else
                            {
                                MessageBox.Show("Не было внесено изменений", "Информация",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            MessageBox.Show($"Ошибка сохранения изменений: {ex.Message}", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка соединения с базой данных: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void DeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedEmployee == null)
            {
                MessageBox.Show("Выберите сотрудника для удаления", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Проверяем, что это сотрудник
            if (_selectedEmployee.RoleID != 2)
            {
                MessageBox.Show("Выбранный пользователь не является сотрудником", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var result = MessageBox.Show($"Вы уверены, что хотите удалить сотрудника {_selectedEmployee.LastName} {_selectedEmployee.Name}?",
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (var connection = DatabaseContext.GetConnection())
                    {
                        await connection.OpenAsync();

                        using (var transaction = await connection.BeginTransactionAsync())
                        {
                            try
                            {
                                // Удаляем связи с должностями (только для сотрудников)
                                string deletePostQuery = @"
                                    DELETE up FROM UserPost up
                                    INNER JOIN Users u ON up.UserID = u.Id
                                    WHERE up.UserID = @UserId AND u.RoleId = 2";
                                using (var command = new MySqlCommand(deletePostQuery, connection, transaction))
                                {
                                    command.Parameters.AddWithValue("@UserId", _selectedEmployee.ID);
                                    await command.ExecuteNonQueryAsync();
                                }

                                // Удаляем пользователя (только если это сотрудник)
                                string deleteUserQuery = "DELETE FROM Users WHERE Id = @Id AND RoleId = 2";
                                using (var userCommand = new MySqlCommand(deleteUserQuery, connection, transaction))
                                {
                                    userCommand.Parameters.AddWithValue("@Id", _selectedEmployee.ID);
                                    int rowsAffected = await userCommand.ExecuteNonQueryAsync();

                                    if (rowsAffected > 0)
                                    {
                                        await transaction.CommitAsync();

                                        MessageBox.Show("Сотрудник успешно удален", "Успех",
                                            MessageBoxButton.OK, MessageBoxImage.Information);

                                        await LoadDataAsync();

                                        // Очищаем поля
                                        FirstNameTextBox.Text = "";
                                        LastNameTextBox.Text = "";
                                        PatronymicTextBox.Text = "";
                                        PostComboBox.SelectedIndex = -1;
                                        SalaryTextBox.Text = "";
                                        EmployeesComboBox.SelectedIndex = -1;
                                        _selectedEmployee = null;
                                        ResetChangeFlags();
                                    }
                                    else
                                    {
                                        await transaction.RollbackAsync();
                                        MessageBox.Show("Не удалось удалить сотрудника. Возможно, пользователь не является сотрудником.", "Ошибка",
                                            MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                await transaction.RollbackAsync();
                                MessageBox.Show($"Ошибка удаления сотрудника: {ex.Message}", "Ошибка",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка соединения с базой данных: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
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