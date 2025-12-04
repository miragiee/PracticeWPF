using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfApp1.Models;

namespace WpfApp1.UserInterface
{
    public class BreadCatViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Goods> _goods = new ObservableCollection<Goods>();
        public ObservableCollection<Goods> Goods
        {
            get => _goods;
            set
            {
                _goods = value;
                OnPropertyChanged();
            }
        }

        public BreadCatViewModel()
        {
            LoadProducts();
        }

        public void LoadProducts()
        {
            Goods.Add(new Goods
            {
                Id = 1,
                Name = "Хлеб чёрный",
                Price = 50.99m,
                ImagePath = "/Images/bread.png",
                CategoryId = 1
            });

            Goods.Add(new Goods
            {
                Id = 2,
                Name = "Лаваш",
                Price = 89.50m,
                ImagePath = "/Images/lawash.jpg",
                CategoryId = 1
            });

            // Добавьте другие товары категории "Мучные изделия"
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}