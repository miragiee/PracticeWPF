using System;

namespace WpfApp1.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int GoodsId { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public Goods Goods { get; set; }
    }
}