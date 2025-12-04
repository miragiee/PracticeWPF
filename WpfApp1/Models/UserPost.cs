using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class UserPost
    {
        public int UP_ID { get; set; }
        public int UserID { get; set; }
        public int PostID { get; set; }

        // Навигационные свойства
        public Users? User { get; set; }
        public Postlist? Post { get; set; }
    }
}