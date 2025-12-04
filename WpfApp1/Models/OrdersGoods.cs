using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class OrdersGoods
    {
        //private Goods _goods;
        private decimal _totalPrice;
        private decimal _price;
        private int _amount;
        public int Id { get; set; }
        public required string Name { get; set; }
        public int OrderID { get; set; }
        public int GoodsID { get; set; }
        public decimal Price
        {
            get { return _price; }
            set
            {
                _price = value;
                CalculateTotalPrice();
            }
        }
        public int Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                CalculateTotalPrice();
            }
        }
        public decimal TotalPrice
        {
            get { return _totalPrice; }
            set { _totalPrice = value; }
        }

        // Навигационные свойства
        public Orders? Order { get; set; }
        public Goods? Goods { get; set; }

        private void CalculateTotalPrice()
        {
            _totalPrice = _amount * _price;
        }
    }
}