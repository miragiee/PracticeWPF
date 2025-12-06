using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfApp1.Models;

namespace WpfApp1.ViewModels
{
    public class ReadyMealCatViewModel : INotifyPropertyChanged
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

        public ReadyMealCatViewModel()
        {
            LoadProducts();
        }

        public void LoadProducts()
        {
            // Готовые блюда
            Goods.Add(new Goods
            {
                ID = 801,
                Name = "Салат 'Цезарь'",
                Price = 320.00m,
                ImagePath = "/Images/salad.png",
                CategoryID = 5
            });

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}