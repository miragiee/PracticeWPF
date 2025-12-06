using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using WpfApp1.Models;
using WpfApp1.Services;
using WpfApp1.UserInterface;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace WpfApp1
{
    public partial class ManageOrders : Window, INotifyPropertyChanged
    {
        private DatabaseService dbService = new DatabaseService();

        private ObservableCollection<Orders> _orders;
        private Orders _selectedOrder;
        private string _newDeliveryAddress;
        private bool _isLoading;

        public ObservableCollection<Orders> Orders
        {
            get => _orders;
            set
            {
                _orders = value;
                OnPropertyChanged(nameof(Orders));
            }
        }

        public Orders SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                _selectedOrder = value;
                OnPropertyChanged(nameof(SelectedOrder));
                OnPropertyChanged(nameof(IsOrderSelected));
                OnPropertyChanged(nameof(IsDeliveryOrder));
                OnPropertyChanged(nameof(CanChangeAddress));
                OnPropertyChanged(nameof(DeliveryText)); // Добавлено
                OnPropertyChanged(nameof(OrderInfoText)); // Добавлено

                if (value != null)
                {
                    NewDeliveryAddress = value.DeliveryAddress ?? "";
                }
                else
                {
                    NewDeliveryAddress = "";
                }
            }
        }

        public string NewDeliveryAddress
        {
            get => _newDeliveryAddress;
            set
            {
                _newDeliveryAddress = value;
                OnPropertyChanged(nameof(NewDeliveryAddress));
                OnPropertyChanged(nameof(CanChangeAddress));
            }
        }

        public bool IsOrderSelected => SelectedOrder != null;
        public bool IsDeliveryOrder => SelectedOrder?.Delivery == true;
        public bool CanChangeAddress => IsOrderSelected && IsDeliveryOrder && !string.IsNullOrWhiteSpace(NewDeliveryAddress);

        // Добавлено свойство DeliveryText
        public string DeliveryText
        {
            get
            {
                if (SelectedOrder == null) return "Не выбран";
                return SelectedOrder.Delivery ? "Да" : "Нет";
            }
        }

        // Добавлено свойство OrderInfoText
        public string OrderInfoText
        {
            get
            {
                if (SelectedOrder == null) return "Заказ не выбран";
                return $"Заказ #{SelectedOrder.ID} | Клиент: {SelectedOrder.ClientID} | Сумма: {SelectedOrder.TotalCost:C2} | Доставка: {DeliveryText}";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ManageOrders()
        {
            InitializeComponent();
            DataContext = this;
            Orders = new ObservableCollection<Orders>();
            Loaded += Window_Loaded;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadOrdersAsync();
        }

        private async Task LoadOrdersAsync()
        {
            try
            {
                _isLoading = true;
                OrdersComboBox.IsEnabled = false;

                var orders = await dbService.GetAllOrdersAsync();

                Orders.Clear();
                foreach (var order in orders)
                {
                    Orders.Add(order);
                }

                if (Orders.Count > 0)
                {
                    SelectedOrder = Orders.First();
                }

                OrdersComboBox.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки заказов: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                LoadTestData();
            }
            finally
            {
                _isLoading = false;
            }
        }

        private void LoadTestData()
        {
            Orders.Clear();

            // Тестовые данные
            Orders.Add(new Orders
            {
                ID = 1,
                ClientID = 101,
                TotalCost = 390,
                Delivery = true,
                CookingTime = new TimeSpan(1, 0, 0),
                DeliveryAddress = "ул. Ленина, д. 15, кв. 42"
            });

            Orders.Add(new Orders
            {
                ID = 2,
                ClientID = 102,
                TotalCost = 1250,
                Delivery = false,
                CookingTime = new TimeSpan(0, 45, 0)
            });

            Orders.Add(new Orders
            {
                ID = 3,
                ClientID = 103,
                TotalCost = 2730,
                Delivery = true,
                CookingTime = new TimeSpan(1, 30, 0),
                DeliveryAddress = "пр. Мира, д. 67, кв. 89"
            });

            Orders.Add(new Orders
            {
                ID = 4,
                ClientID = 104,
                TotalCost = 890,
                Delivery = true,
                CookingTime = new TimeSpan(0, 30, 0),
                DeliveryAddress = "ул. Советская, д. 23, кв. 5"
            });

            Orders.Add(new Orders
            {
                ID = 5,
                ClientID = 105,
                TotalCost = 1560,
                Delivery = false,
                CookingTime = new TimeSpan(1, 15, 0)
            });

            if (Orders.Count > 0)
            {
                SelectedOrder = Orders.First();
            }
        }

        private async void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedOrder == null)
            {
                MessageBox.Show("Выберите заказ для изменения",
                    "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!SelectedOrder.Delivery)
            {
                MessageBox.Show("Этот заказ не предназначен для доставки. Изменение адреса недоступно.",
                    "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (string.IsNullOrWhiteSpace(NewDeliveryAddress))
            {
                MessageBox.Show("Введите новый адрес доставки",
                    "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Обновляем адрес в объекте заказа
                SelectedOrder.DeliveryAddress = NewDeliveryAddress;

                // Сохраняем в базу данных
                var result = await dbService.UpdateOrderAsync(SelectedOrder);

                if (result)
                {
                    MessageBox.Show("Адрес доставки успешно изменен!",
                        "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    OnPropertyChanged(nameof(OrderInfoText)); // Обновляем текст
                }
                else
                {
                    MessageBox.Show("Не удалось изменить адрес доставки",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении изменений: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void CancelOrder_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedOrder == null)
            {
                MessageBox.Show("Выберите заказ для отмены",
                    "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Вы уверены, что хотите отменить заказ №{SelectedOrder.ID}?\n" +
                                       $"Клиент ID: {SelectedOrder.ClientID}\n" +
                                       $"Сумма: {SelectedOrder.TotalCost:C2}\n\n" +
                                       "Это действие нельзя отменить!",
                                       "Подтверждение отмены заказа",
                                       MessageBoxButton.YesNo,
                                       MessageBoxImage.Warning,
                                       MessageBoxResult.No);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var success = await dbService.DeleteOrderAsync(SelectedOrder.ID);

                    if (success)
                    {
                        MessageBox.Show("Заказ успешно отменен!",
                            "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Удаляем из списка
                        Orders.Remove(SelectedOrder);
                        SelectedOrder = Orders.FirstOrDefault();
                    }
                    else
                    {
                        MessageBox.Show("Не удалось отменить заказ",
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при отмене заказа: {ex.Message}",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void DontChange_Click(object sender, RoutedEventArgs e)
        {
            // Восстанавливаем оригинальный адрес
            if (SelectedOrder != null)
            {
                NewDeliveryAddress = SelectedOrder.DeliveryAddress ?? "";
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

        private void OrdersComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (!_isLoading && OrdersComboBox.SelectedItem is Orders selected)
            {
                SelectedOrder = selected;
            }
        }
    }
}