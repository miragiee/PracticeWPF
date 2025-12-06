using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class Orders
    {
        private Users _client = new Users();
        private string? _deliveryAddress;
        public int ID { get; set; }
        public int ClientID { get; set; }
        public decimal TotalCost { get; set; }
        public bool Delivery { get; set; }
        public TimeSpan CookingTime { get; set; }
        public int[]? OrderedGoodsID { get; set; }
        public string GoodsDisplay
        {
            get
            {
                if (OrderedGoodsID != null && OrderedGoodsID.Length > 0)
                {
                    return string.Join(", ", OrderedGoodsID.Select(id => $"Товар #{id}"));
                }
                return "Нет товаров";
            }
        }

        public double Weight { get; set; }

        public TimeSpan DeliveryTime { get; set; }

        public string? DeliveryAddress { get; set; }

        // Навигационные свойства
        public Users? Client { get; set; }
        public List<OrdersGoods>? OrdersGoods { get; set; }
        public Delivery? OrderDelivery { get; set; }
        public List<OrderEmployee>? OrderEmployees { get; set; }

        public string DisplayInfo
        {
            get
            {
                string deliveryText = Delivery ? "Доставка" : "Самовывоз";
                return $"Заказ #{ID} | Клиент: {ClientID} | Сумма: {TotalCost:C2} | {deliveryText}";
            }
        }
    }
}