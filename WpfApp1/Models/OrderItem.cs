﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class OrderItem
    {
        public int GoodsId { get; set; }
        public int Amount { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
    }
}