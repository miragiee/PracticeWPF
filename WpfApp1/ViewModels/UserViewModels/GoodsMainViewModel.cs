using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Models;
using WpfApp1.Services;

namespace WpfApp1.ViewModels
{
    public class GoodsMainViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseService _databaseService;
        private ObservableCollection<Goods> _goods = new ObservableCollection<Goods>();
        private ObservableCollection<Goods> _popularGoods1 = new ObservableCollection<Goods>();
        private ObservableCollection<Goods> _popularGoods2 = new ObservableCollection<Goods>();

        public ObservableCollection<Goods> Goods
        {
            get => _goods;
            set
            {
                _goods = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Goods> PopularGoods1
        {
            get => _popularGoods1;
            set
            {
                _popularGoods1 = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Goods> PopularGoods2
        {
            get => _popularGoods2;
            set
            {
                _popularGoods2 = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public GoodsMainViewModel()
        {
            _databaseService = new DatabaseService();
            LoadProductsAsync();
        }

        private async void LoadProductsAsync()
        {
            try
            {
                // Загружаем 8 случайных товаров (по 4 для каждого блока)
                var randomGoods = await _databaseService.GetRandomGoodsAsync(8);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Goods.Clear();
                    PopularGoods1.Clear();
                    PopularGoods2.Clear();

                    // Добавляем все товары в общую коллекцию (опционально)
                    foreach (var good in randomGoods)
                    {
                        Goods.Add(good);
                    }

                    // Разделяем на два блока (первые 4 - в первый блок, остальные - во второй)
                    for (int i = 0; i < randomGoods.Count; i++)
                    {
                        if (i < 4)
                        {
                            PopularGoods1.Add(randomGoods[i]);
                        }
                        else
                        {
                            PopularGoods2.Add(randomGoods[i]);
                        }
                    }

                    // Уведомляем об изменении всех коллекций
                    OnPropertyChanged(nameof(Goods));
                    OnPropertyChanged(nameof(PopularGoods1));
                    OnPropertyChanged(nameof(PopularGoods2));
                });
            }
            catch (Exception ex)
            {
                // В случае ошибки используем тестовые данные как запасной вариант
                MessageBox.Show($"Ошибка загрузки товаров: {ex.Message}\nИспользуются тестовые данные.",
                    "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                LoadTestProducts();
            }
        }

        // Запасной метод на случай ошибки загрузки из БД
        private void LoadTestProducts()
        {
            Goods.Add(new Goods
            {
                ID = 1,
                Name = "Лаваш",
                Price = 60m,
                ImagePath = "/Images/lawash.jpg",
                CategoryID = 1
            });

            Goods.Add(new Goods
            {
                ID = 4,
                Name = "Апельсины",
                Price = 90m,
                ImagePath = "/Images/orange.png",
                CategoryID = 2
            });

            Goods.Add(new Goods
            {
                ID = 5,
                Name = "Картофель Молодой",
                Price = 20m,
                ImagePath = "/Images/potatoe.jpg",
                CategoryID = 3
            });

            Goods.Add(new Goods
            {
                ID = 6,
                Name = "Огурцы Короткоплодные",
                Price = 40m,
                ImagePath = "/Images/cucumber.jpg",
                CategoryID = 3
            });

            Goods.Add(new Goods
            {
                ID = 7,
                Name = "Куриная Грудка",
                Price = 250m,
                ImagePath = "/Images/ChickenBreast.png",
                CategoryID = 4
            });

            Goods.Add(new Goods
            {
                ID = 9,
                Name = "Зева 4 рулона",
                Price = 240m,
                ImagePath = "/Images/toiletpaper.png",
                CategoryID = 9
            });

            // Разделяем на два блока
            for (int i = 0; i < Goods.Count; i++)
            {
                if (i < 4)
                {
                    PopularGoods1.Add(Goods[i]);
                }
                else if (i < 8 && i < Goods.Count)
                {
                    PopularGoods2.Add(Goods[i]);
                }
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}