using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using WpfApp1.Classes;
using WpfApp1.Models;

namespace WpfApp1.Managers
{
    public class CartManager
    {
        private static CartManager _instance;
        private static object _instanceLock = new object();

        public static CartManager Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    return _instance ??=new CartManager();
                }
            }

        }

        public ObservableCollection<CartItem> Items { get; } = new ObservableCollection<CartItem>();
        public decimal TotalPrice => Items.Sum(item => item.TotalPrice);
        public event EventHandler CartChanged;

        private CartManager() 
        { 
        }

        public void AddProduct(Goods goods, int Amount = 1)
        {
            var existingItem = Items.FirstOrDefault(item => item.Goods.Id == goods.Id);

            if (existingItem != null)
            {
                existingItem.Amount += Amount;
            }

            else
            {
                Items.Add(new CartItem { Goods = goods, Amount = Amount });
            }

            CartWasChanged();
        }

        public void DeleteProduct(Goods goods, int Amount = 1)
        {
            var existingItem = Items.FirstOrDefault(item => item.Goods.Id == goods.Id);

            if(existingItem != null)
            {
                existingItem.Amount -= Amount;

                if(existingItem.Amount <= 0)
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
            CartChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
