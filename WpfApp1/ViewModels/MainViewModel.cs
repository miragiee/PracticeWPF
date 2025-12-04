using WpfApp1.Models;
using WpfApp1;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WpfApp1.Services;

namespace WpfApp1.ViewModels
{
    public class MainViewModel
    {
        private DatabaseService databaseService;

        // Простые публичные свойства - никакого INotifyPropertyChanged
        public ObservableCollection<Users> Users { get; set; }
        public Users SelectedUser { get; set; }
        public string StatusMessage { get; set; }
        public bool IsConnected { get; set; }

        // Команды
        public ICommand LoadDataCommand { get; }
        public ICommand AddUserCommand { get; }
        public ICommand UpdateUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand TestConnectionCommand { get; }

        public MainViewModel()
        {
            databaseService = new DatabaseService();
            Users = new ObservableCollection<Users>();

            // Простые команды
            LoadDataCommand = new RelayCommand(LoadData);
            AddUserCommand = new RelayCommand(AddUser);
            UpdateUserCommand = new RelayCommand(UpdateUser);
            DeleteUserCommand = new RelayCommand(DeleteUser);
            TestConnectionCommand = new RelayCommand(TestConnection);
        }

        private async void TestConnection()
        {
            IsConnected = await databaseService.TestConnectionAsync();
            StatusMessage = IsConnected ? "Подключено" : "Не подключено";

            // В реальном приложении здесь нужно уведомлять UI
            Console.WriteLine(StatusMessage);
        }

        private async void LoadData()
        {
            try
            {
                var users = await databaseService.GetAllUsersAsync();
                Users.Clear();

                foreach (var user in users)
                {
                    Users.Add(user);
                }

                StatusMessage = $"Загружено {Users.Count} пользователей";
                Console.WriteLine(StatusMessage);
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка: {ex.Message}";
                Console.WriteLine(StatusMessage);
            }
        }

        private async void AddUser()
        {
            var newUser = new Users
            {
                RoleId = 1,
                Login = "new_user",
                Password = "password",
                Name = "Имя",
                LastName = "Фамилия",
                Patronymic = "Отчество",
                PhoneNumber = "+79991234567",
                Email = "test@mail.com",
                BirthDate = DateTime.Now.AddYears(-30),
                Address = "Адрес"
            };

            var success = await databaseService.CreateUserAsync(newUser);
            StatusMessage = success ? "Пользователь добавлен" : "Ошибка добавления";
            Console.WriteLine(StatusMessage);

            if (success)
            {
                LoadData(); // Перезагружаем данные
            }
        }

        private async void UpdateUser()
        {
            if (SelectedUser == null)
            {
                StatusMessage = "Выберите пользователя";
                Console.WriteLine(StatusMessage);
                return;
            }

            // Меняем имя для примера
            SelectedUser.Name += " (изменено)";

            var success = await databaseService.UpdateUserAsync(SelectedUser);
            StatusMessage = success ? "Пользователь обновлен" : "Ошибка обновления";
            Console.WriteLine(StatusMessage);
        }

        private async void DeleteUser()
        {
            if (SelectedUser == null)
            {
                StatusMessage = "Выберите пользователя";
                Console.WriteLine(StatusMessage);
                return;
            }

            var success = await databaseService.DeleteUserAsync(SelectedUser.Id);
            StatusMessage = success ? "Пользователь удален" : "Ошибка удаления";
            Console.WriteLine(StatusMessage);

            if (success)
            {
                Users.Remove(SelectedUser);
                SelectedUser = null;
            }
        }
    }

    // Простейшая реализация команды
    public class RelayCommand : ICommand
    {
        private readonly Action execute;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action execute)
        {
            this.execute = execute;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            execute?.Invoke();
        }
    }
}