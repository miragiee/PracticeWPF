namespace WpfApp1.Models
{
    public class Users
    {
        public int ID { get; set; }
        public int RoleID { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? Patronymic { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string? Address { get; set; }

        // Навигационные свойства
        public Role? Role { get; set; }
        public List<UserPost>? UserPosts { get; set; }
        public List<OrderEmployee>? OrderEmployees { get; set; }
        public List<Delivery>? Deliveries { get; set; }

        // Свойства для отображения
        public string FullName => $"{LastName} {Name} {Patronymic}";
        public string RoleName => Role?.RoleName ?? "Не указана";
    }
}