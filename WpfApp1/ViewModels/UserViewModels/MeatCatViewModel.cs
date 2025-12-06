using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfApp1.Models;

namespace WpfApp1.ViewModels
{
    public class MeatCatViewModel : INotifyPropertyChanged
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

        public MeatCatViewModel()
        {
            LoadProducts();
        }

        public void LoadProducts()
        {
            // Мясные продукты
            Goods.Add(new Goods
            {
                ID = 701,
                Name = "Свинина лопатка",
                Price = 350.00m,
                ImagePath = "/Images/notHalal.jpg",
                CategoryID = 4
            });

            Goods.Add(new Goods
            {
                ID = 702,
                Name = "Грудка куриная",
                Price = 450.00m,
                ImagePath = "/Images/ChickenBreast.png",
                CategoryID = 4
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}