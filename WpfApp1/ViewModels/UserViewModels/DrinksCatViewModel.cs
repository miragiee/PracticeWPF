using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfApp1.Models;

namespace WpfApp1.UserInterface
{
    public class DrinksCatViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Goods> _nonAlcoholicGoods = new ObservableCollection<Goods>();
        private ObservableCollection<Goods> _alcoholicGoods = new ObservableCollection<Goods>();

        public ObservableCollection<Goods> NonAlcoholicGoods
        {
            get => _nonAlcoholicGoods;
            set
            {
                _nonAlcoholicGoods = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Goods> AlcoholicGoods
        {
            get => _alcoholicGoods;
            set
            {
                _alcoholicGoods = value;
                OnPropertyChanged();
            }
        }

        public DrinksCatViewModel()
        {
            LoadProducts();
        }

        public void LoadProducts()
        {
            // Безалкогольные напитки
            NonAlcoholicGoods.Add(new Goods
            {
                Id = 101,
                Name = "Добрый Кола",
                Price = 99.99m,
                ImagePath = "/Images/dobryCola.jpg",
                CategoryId = 6 // ID для напитков
            });

            // Алкогольные напитки
            AlcoholicGoods.Add(new Goods
            {
                Id = 201,
                Name = "Пиво Светлое Нефильтрованное",
                Price = 189.90m,
                ImagePath = "/Images/pevo.jpg",
                CategoryId = 7 // ID для алкогольных напитков
            });

            // Добавьте другие напитки по аналогии
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}