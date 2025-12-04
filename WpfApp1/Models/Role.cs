using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string? RoleName { get; set; }

        // Навигационные свойства
        public List<Users>? Users { get; set; }
    }
}