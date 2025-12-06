using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Classes;
using WpfApp1.Models;

namespace WpfApp1.Managers
{
    public class CartManager : INotifyPropertyChanged
    {
        private static CartManager _instance;
        private static object _instanceLock = new object();

        public static CartManager Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    return _instance ??= new CartManager();
                }
            }
        }

        public ObservableCollection<CartItem> Items { get; } = new ObservableCollection<CartItem>();

        private decimal _totalPrice;
        public decimal TotalPrice
        {
            get => _totalPrice;
            private set
            {
                _totalPrice = value;
                OnPropertyChanged();
            }
        }

        public event EventHandler CartChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        private CartManager()
        {
        }

        public void AddProduct(Goods goods, int Amount = 1)
        {
            var existingItem = Items.FirstOrDefault(item => item.Product.ID == goods.ID);

            if (existingItem != null)
            {
                existingItem.Amount += Amount;
            }
            else
            {
                Items.Add(new CartItem { Product = goods, Amount = Amount });
            }

            CartWasChanged();
        }

        public void DeleteProduct(Goods goods, int Amount = 1)
        {
            var existingItem = Items.FirstOrDefault(item => item.Product.ID == goods.ID);

            if (existingItem != null)
            {
                existingItem.Amount -= Amount;

                if (existingItem.Amount <= 0)
                {
                    Items.Remove(existingItem);
                }

                CartWasChanged();
            }
        }

        public void ClearCart()
        {
            Items.Clear();
            CartWasChanged();
        }

        private void CartWasChanged()
        {
            TotalPrice = Items.Sum(item => item.TotalPrice);
            CartChanged?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}