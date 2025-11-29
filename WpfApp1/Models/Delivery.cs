using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class Delivery
    {
        public int Id { get; set; }
        public string? DeliveryAddress { get; set; }
        public string? PickUpAdress {  get; set; }
        public TimeSpan DeliveryTime { get; set; }
        public int EmployeeID { get; set; }
        public int OrderID { get; set; }
    }
}
