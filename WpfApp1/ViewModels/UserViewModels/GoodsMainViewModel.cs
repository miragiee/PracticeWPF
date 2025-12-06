using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace WpfApp1.ViewModels
{
    public class GoodsMainViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

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

        public GoodsMainViewModel()
        {
            LoadProducts();
        }

        public void LoadProducts()
        {
            Goods.Add(new Goods
            {
                ID = 1,
                Name = "Хлеб чёрный",
                Price = 50.99m,
                ImagePath = "/Images/bread.png",
                CategoryID = 1
            });

            Goods.Add(new Goods
            {
                ID = 2,
                Name = "Апельсин",
                Price = 89.50m,
                ImagePath = "/Images/orange.png",
                CategoryID = 2
            });

            Goods.Add(new Goods
            {
                ID = 3,
                Name = "Туалетная бумага",
                Price = 299.99m,
                ImagePath = "/Images/toiletpaper.png",
                CategoryID = 3
            });

            Goods.Add(new Goods
            {
                ID = 4,
                Name = "Грудка куриная",
                Price = 450.00m,
                ImagePath = "/Images/ChickenBreast.png",
                CategoryID = 4
            });

            Goods.Add(new Goods
            {
                ID = 5,
                Name = "Салат 'Цезарь'",
                Price = 320.00m,
                ImagePath = "/Images/salad.png",
                CategoryID = 5
            });
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
