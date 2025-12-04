using MySql.Data.MySqlClient;
using WpfApp1.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;

namespace WpfApp1.Services
{
    public class DatabaseService
    {
        // === USERS ===
        public async Task<List<Users>> GetAllUsersAsync()
        {
            var users = new List<Users>();

            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();
                    string query = "SELECT * FROM Users";

                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            users.Add(new Users
                            {
                                Id = reader.GetInt32("Id"),
                                RoleId = reader.GetInt32("RoleId"),
                                Login = reader["Login"]?.ToString(),
                                Password = reader["Password"]?.ToString(),
                                Name = reader["Name"]?.ToString(),
                                LastName = reader["LastName"]?.ToString(),
                                Patronymic = reader["Patronymic"]?.ToString(),
                                PhoneNumber = reader["PhoneNumber"]?.ToString(),
                                Email = reader["Email"]?.ToString(),
                                BirthDate = reader.GetDateTime("BirthDate"),
                                Address = reader["Address"]?.ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetAllUsers error: {ex.Message}");
            }

            return users;
        }

        public async Task<List<Users>> GetUsersByRoleAsync(int roleId)
        {
            var users = new List<Users>();

            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();
                    string query = "SELECT * FROM Users WHERE RoleId = @RoleId";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RoleId", roleId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                users.Add(new Users
                                {
                                    Id = reader.GetInt32("Id"),
                                    RoleId = reader.GetInt32("RoleId"),
                                    Login = reader["Login"]?.ToString(),
                                    Password = reader["Password"]?.ToString(),
                                    Name = reader["Name"]?.ToString(),
                                    LastName = reader["LastName"]?.ToString(),
                                    Patronymic = reader["Patronymic"]?.ToString(),
                                    PhoneNumber = reader["PhoneNumber"]?.ToString(),
                                    Email = reader["Email"]?.ToString(),
                                    BirthDate = reader.GetDateTime("BirthDate"),
                                    Address = reader["Address"]?.ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetUsersByRole error: {ex.Message}");
                // Пробрасываем исключение дальше, чтобы обработать в UI
                throw;
            }

            return users;
        }

        public async Task<bool> CreateUserAsync(Users user)
        {
            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();

                    string query = @"INSERT INTO Users (RoleId, Login, Password, Name, LastName, Patronymic, 
                                    PhoneNumber, Email, BirthDate, Address) 
                                    VALUES (@RoleId, @Login, @Password, @Name, @LastName, @Patronymic, 
                                    @PhoneNumber, @Email, @BirthDate, @Address)";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RoleId", user.RoleId);
                        command.Parameters.AddWithValue("@Login", user.Login);
                        command.Parameters.AddWithValue("@Password", user.Password);
                        command.Parameters.AddWithValue("@Name", user.Name);
                        command.Parameters.AddWithValue("@LastName", user.LastName);
                        command.Parameters.AddWithValue("@Patronymic", user.Patronymic);
                        command.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                        command.Parameters.AddWithValue("@Email", user.Email);
                        command.Parameters.AddWithValue("@BirthDate", user.BirthDate);
                        command.Parameters.AddWithValue("@Address", user.Address);

                        return await command.ExecuteNonQueryAsync() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AddUser error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateUserAsync(Users user)
        {
            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();

                    string query = @"UPDATE Users SET 
                                    RoleId = @RoleId, 
                                    Login = @Login, 
                                    Password = @Password, 
                                    Name = @Name, 
                                    LastName = @LastName, 
                                    Patronymic = @Patronymic, 
                                    PhoneNumber = @PhoneNumber, 
                                    Email = @Email, 
                                    BirthDate = @BirthDate, 
                                    Address = @Address 
                                    WHERE Id = @Id";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", user.Id);
                        command.Parameters.AddWithValue("@RoleId", user.RoleId);
                        command.Parameters.AddWithValue("@Login", user.Login);
                        command.Parameters.AddWithValue("@Password", user.Password);
                        command.Parameters.AddWithValue("@Name", user.Name);
                        command.Parameters.AddWithValue("@LastName", user.LastName);
                        command.Parameters.AddWithValue("@Patronymic", user.Patronymic);
                        command.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                        command.Parameters.AddWithValue("@Email", user.Email);
                        command.Parameters.AddWithValue("@BirthDate", user.BirthDate);
                        command.Parameters.AddWithValue("@Address", user.Address);

                        return await command.ExecuteNonQueryAsync() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UpdateUser error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();

                    string query = "DELETE FROM Users WHERE Id = @Id";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        return await command.ExecuteNonQueryAsync() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DeleteUser error: {ex.Message}");
                return false;
            }
        }

        // === GOODS ===
        public async Task<List<Goods>> GetAllGoodsAsync()
        {
            var goods = new List<Goods>();

            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();
                    string query = "SELECT * FROM Goods";

                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            goods.Add(new Goods
                            {
                                Id = reader.GetInt32("Id"),
                                Name = reader["Name"]?.ToString(),
                                Price = reader.GetDecimal("Price"),
                                CategoryId = reader.GetInt32("CategoryId")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetAllGoods error: {ex.Message}");
            }

            return goods;
        }

        public async Task<bool> AddGoodsAsync(Goods goods)
        {
            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();

                    string query = @"INSERT INTO Goods (Name, Price, CategoryId) 
                                    VALUES (@Name, @Price, @CategoryId)";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", goods.Name);
                        command.Parameters.AddWithValue("@Price", goods.Price);
                        command.Parameters.AddWithValue("@CategoryId", goods.CategoryId);

                        return await command.ExecuteNonQueryAsync() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AddGoods error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateGoodsAsync(Goods goods)
        {
            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();

                    string query = @"UPDATE Goods SET 
                                    Name = @Name, 
                                    Price = @Price, 
                                    CategoryId = @CategoryId 
                                    WHERE Id = @Id";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", goods.Id);
                        command.Parameters.AddWithValue("@Name", goods.Name);
                        command.Parameters.AddWithValue("@Price", goods.Price);
                        command.Parameters.AddWithValue("@CategoryId", goods.CategoryId);

                        return await command.ExecuteNonQueryAsync() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UpdateGoods error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteGoodsAsync(int id)
        {
            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();

                    string query = "DELETE FROM Goods WHERE Id = @Id";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        return await command.ExecuteNonQueryAsync() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DeleteGoods error: {ex.Message}");
                return false;
            }
        }

        // === ORDERS ===
        public async Task<List<Orders>> GetAllOrdersAsync()
        {
            var orders = new List<Orders>();

            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();
                    string query = "SELECT * FROM Orders";

                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            orders.Add(new Orders
                            {
                                Id = reader.GetInt32("Id"),
                                ClientId = reader.GetInt32("ClientId"),
                                TotalCost = reader.GetDecimal("TotalCost"),
                                Delivery = reader.GetBoolean("Delivery")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetAllOrders error: {ex.Message}");
            }

            return orders;
        }

        public async Task<bool> AddOrderAsync(Orders order)
        {
            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();

                    string query = @"INSERT INTO Orders (ClientId, TotalCost, Delivery, CookingTime) 
                                    VALUES (@ClientId, @TotalCost, @Delivery, @CookingTime)";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ClientId", order.ClientId);
                        command.Parameters.AddWithValue("@TotalCost", order.TotalCost);
                        command.Parameters.AddWithValue("@Delivery", order.Delivery);
                        command.Parameters.AddWithValue("@CookingTime", order.CookingTime);

                        return await command.ExecuteNonQueryAsync() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AddOrder error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateOrderAsync(Orders order)
        {
            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();

                    string query = @"UPDATE Orders SET 
                                    ClientId = @ClientId, 
                                    TotalCost = @TotalCost, 
                                    Delivery = @Delivery, 
                                    CookingTime = @CookingTime 
                                    WHERE Id = @Id";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", order.Id);
                        command.Parameters.AddWithValue("@ClientId", order.ClientId);
                        command.Parameters.AddWithValue("@TotalCost", order.TotalCost);
                        command.Parameters.AddWithValue("@Delivery", order.Delivery);
                        command.Parameters.AddWithValue("@CookingTime", order.CookingTime);

                        return await command.ExecuteNonQueryAsync() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UpdateOrder error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();

                    string query = "DELETE FROM Orders WHERE Id = @Id";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        return await command.ExecuteNonQueryAsync() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DeleteOrder error: {ex.Message}");
                return false;
            }
        }

        // === TEST CONNECTION ===
        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();

                    // Выполняем простой запрос для проверки
                    string query = "SELECT 1";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        var result = await command.ExecuteScalarAsync();
                        return result != null && result.ToString() == "1";
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error {ex.Number}: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection test error: {ex.Message}");
                return false;
            }
        }
    }
}