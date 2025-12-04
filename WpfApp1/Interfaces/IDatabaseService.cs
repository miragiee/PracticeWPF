using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace WpfApp1.Interfaces
{
    public interface IDatabaseService
    {
        Task<List<Users>> GetAllUsersAsync();
        Task<Users> GetUsersByIdAsync(int id);
        Task<bool> CreateUsersAsync(Users user);
        Task<bool> UpdateUsersAsync(Users user);
        Task<bool> DeleteUsersAsync(int id);
        Task<bool> TestConnectionAsync();
    }
}
