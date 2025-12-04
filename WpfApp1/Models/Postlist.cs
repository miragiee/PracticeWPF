using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class Postlist
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal? Salary { get; set; }

        // Навигационные свойства
        public List<UserPost>? UserPosts { get; set; }
    }
}