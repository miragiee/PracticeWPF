using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class Orders
    {
        private Client _client = new Client();
        private string? _deliveryAddress;
        public int Id { get; set; }
        public int ClientId { get; set; }
        public decimal TotalCost { get; set; }
        public bool Delivery { get; set; }
        public TimeSpan CookingTime { get; set; }
        public int[]? OrderedGoodsID {  get; set; }

        public string GoodsDisplay => OrderedGoodsID != null ? string.Join(", ", OrderedGoodsID) : string.Empty;
       
        public double Weight { get; set; }

        public TimeSpan DeliveryTime { get; set; }

        public string? DeliveryAddress
        {
            get
            {
                return _deliveryAddress;
            }
            set
            {
                _deliveryAddress = value;
            }
        }
    }
}
