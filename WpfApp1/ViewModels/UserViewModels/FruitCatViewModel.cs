using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfApp1.Models;

namespace WpfApp1.UserInterface
{
    public class FruitCatViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Goods> _fruits = new ObservableCollection<Goods>();
        private ObservableCollection<Goods> _vegetables = new ObservableCollection<Goods>();

        public ObservableCollection<Goods> Fruits
        {
            get => _fruits;
            set
            {
                _fruits = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Goods> Vegetables
        {
            get => _vegetables;
            set
            {
                _vegetables = value;
                OnPropertyChanged();
            }
        }

        public FruitCatViewModel()
        {
            LoadProducts();
        }

        public void LoadProducts()
        {
            // Фрукты
            Fruits.Add(new Goods
            {
                ID = 401,
                Name = "Апельсин",
                Price = 89.50m,
                ImagePath = "/Images/orange.png",
                CategoryID = 2
            });

            Fruits.Add(new Goods
            {
                ID = 402,
                Name = "Яблоки",
                Price = 79.90m,
                ImagePath = "/Images/gapple.jpg",
                CategoryID = 2
            });

            // Овощи
            Vegetables.Add(new Goods
            {
                ID = 501,
                Name = "Помидоры",
                Price = 149.90m,
                ImagePath = "/Images/tomato.jpg",
                CategoryID = 3
            });

            Vegetables.Add(new Goods
            {
                ID = 502,
                Name = "Огурцы",
                Price = 89.50m,
                ImagePath = "/Images/cucumber.jpg",
                CategoryID = 3
            });

            Vegetables.Add(new Goods
            {
                ID = 503,
                Name = "Картофель",
                Price = 49.90m,
                ImagePath = "/Images/potato.jpg",
                CategoryID = 3
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}