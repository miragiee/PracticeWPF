using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class GoodsAccounting
    {
        public int Id { get; set; }
        public int GoodsID { get; set; }
        public int GoodsAmount { get; set; }
        public int WarehouseID { get; set; }

        public Goods? Goods { get; set; }
        public Warehouse? Warehouse { get; set; }
    }
}