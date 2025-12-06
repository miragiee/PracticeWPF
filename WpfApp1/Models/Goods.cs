using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApp1.Models
{
    public class Goods
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int CategoryID { get; set; }
        public string? ImagePath { get; set; }

        // Связи
        public Category? Category { get; set; }
        public List<SuppliersGoods>? SuppliersGoods { get; set; }
        public List<GoodsAccounting>? GoodsAccounting { get; set; }
        public List<OrdersGoods>? OrdersGoods { get; set; }
    }
}
