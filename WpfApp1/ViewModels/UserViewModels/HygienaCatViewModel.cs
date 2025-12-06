using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfApp1.Models;

namespace WpfApp1.UserInterface.Categories
{
    public class HygienaCatViewModel : INotifyPropertyChanged
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

        public HygienaCatViewModel()
        {
            LoadProducts();
        }

        public void LoadProducts()
        {
            Goods.Add(new Goods
            {
                ID = 601,
                Name = "Туалетная бумага",
                Price = 299.99m,
                ImagePath = "/Images/toiletpaper.png",
                CategoryID = 8
            });

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}