using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class Orders
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public decimal totalCost { get; set; }
        public bool Delivery { get; set; }
    }
}
