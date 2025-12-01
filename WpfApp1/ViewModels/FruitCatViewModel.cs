using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;
using WpfApp1.UserInterface;

namespace WpfApp1.ViewModels
{
    public class FruitCatViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Goods> _fruits = new ObservableCollection<Goods>();
        private ObservableCollection<Goods> _vegetables = new ObservableCollection<Goods>();

        public event PropertyChangedEventHandler? PropertyChanged;

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
            
        }

        public void LoadProducts()
        {
            Fruits.Add(
                new Goods
                {
                    Id = 1,
                    Name = "Яблоко зелёное",
                    Price = 40,
                    CategoryId = 3,
                    ImagePath = "/Images/gapple.jpgs"
                }
            );

            Fruits.Add(
                new Goods
                {
                    Id = 2,
                    Name = "Апельсин",
                    Price = 30,
                    CategoryId = 3,
                    ImagePath = "/Images/orange.png"

                 }
            );

            Vegetables.Add
            (
                new Goods
                {
                    Id = 3,
                    Name = "Картофель",
                    Price = 10,
                    CategoryId = 3,
                    ImagePath = "/Images/potato.jpg"
                }
            );
            Vegetables.Add
            (
                new Goods
                {
                    Id = 4,
                    Name = "Огурец",
                    Price = 10,
                    CategoryId = 3,
                    ImagePath = "/Images/cucumber.jpg"
                }
            );
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
