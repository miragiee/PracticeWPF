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
        public string? GoodsID { get; set; }
        public string? GoodsAmount { get; set; }
        public string? WarehouseID { get; set; }
    }
}
