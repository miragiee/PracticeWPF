using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfApp1.Models;

namespace WpfApp1.ViewModels
{
    public class SweetsCatViewModel : INotifyPropertyChanged
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

        public SweetsCatViewModel()
        {
            LoadProducts();
        }

        public void LoadProducts()
        {
            // Сладости
            Goods.Add(new Goods
            {
                Id = 901,
                Name = "Пряники Тульские",
                Price = 250.00m,
                ImagePath = "/Images/tulskyPryanik.jpg",
                CategoryId = 9
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}