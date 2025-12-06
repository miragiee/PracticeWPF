using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace WpfApp1.Services
{
    public class DatabaseService
    {
        // Получение пользователя по логину/email
        public async Task<Users?> GetUserByLoginAsync(string login)
        {
            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();

                    string query = @"
                SELECT u.*, r.RoleName 
                FROM Users u 
                LEFT JOIN Role r ON u.RoleId = r.Id 
                WHERE u.Login = @Login OR u.Email = @Login";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Login", login);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var user = new Users
                                {
                                    ID = reader.GetInt32("Id"),
                                    RoleID = reader.GetInt32("RoleId"),
                                    Login = reader["Login"]?.ToString(),
                                    Password = reader["Password"]?.ToString(),
                                    Name = reader["Name"]?.ToString(),
                                    LastName = reader["LastName"]?.ToString(),
                                    Patronymic = reader["Patronymic"]?.ToString(),
                                    PhoneNumber = reader["PhoneNumber"]?.ToString(),
                                    Email = reader["Email"]?.ToString(),
                                    BirthDate = reader.GetDateTime("BirthDate"),
                                    Address = reader["Address"]?.ToString()
                                };

                                if (!reader.IsDBNull(reader.GetOrdinal("RoleName")))
                                {
                                    user.Role = new Role
                                    {
                                        ID = reader.GetInt32("RoleId"),
                                        RoleName = reader["RoleName"]?.ToString()
                                    };
                                }

                                return user;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetUserByLoginAsync error: {ex.Message}");
            }

            return null;
        }

        // Обновление пароля
        public async Task<bool> UpdatePasswordAsync(int userId, string newPassword)
        {
            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();

                    string query = "UPDATE Users SET Password = @Password WHERE Id = @Id";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", userId);
                        command.Parameters.AddWithValue("@Password", newPassword);

                        return await command.ExecuteNonQueryAsync() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UpdatePasswordAsync error: {ex.Message}");
                return false;
            }
        }

        // === АВТОРИЗАЦИЯ ===
        public async Task<Users?> AuthenticateUserAsync(string login, string password)
        {
            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();

                    string query = @"
                        SELECT u.ID, u.RoleID, u.Login, u.Password, u.Name, u.LastName, 
                        u.Patronymic, u.PhoneNumber, u.Email, u.BirthDate, u.Address,
                        r.RoleName
                        FROM Users u
                        LEFT JOIN Roles r ON u.RoleID = r.RoleID
                        WHERE u.Login = @Login AND u.Password = @Password";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Login", login);
                        command.Parameters.AddWithValue("@Password", password);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var user = new Users
                                {
                                    ID = reader.GetInt32("ID"),
                                    RoleID = reader.GetInt32("RoleID"),
                                    Login = reader["Login"]?.ToString(),
                                    Password = reader["Password"]?.ToString(),
                                    Name = reader["Name"]?.ToString(),
                                    LastName = reader["LastName"]?.ToString(),
                                    Patronymic = reader["Patronymic"]?.ToString(),
                                    PhoneNumber = reader["PhoneNumber"]?.ToString(),
                                    Email = reader["Email"]?.ToString(),
                                    BirthDate = reader.GetDateTime("BirthDate"),
                                    Address = reader["Address"]?.ToString()
                                };

                                if (!reader.IsDBNull(reader.GetOrdinal("RoleName")))
                                {
                                    user.Role = new Role
                                    {
                                        ID = reader.GetInt32("RoleID"),
                                        RoleName = reader["RoleName"]?.ToString()
                                    };
                                }

                                return user;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AuthenticateUserAsync error: {ex.Message}");
            }

            return null;
        }

        // Проверка существования пользователя
        public async Task<bool> UserExistsAsync(string login, string email)
        {
            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();

                    string query = "SELECT COUNT(*) FROM Users WHERE Login = @Login OR Email = @Email";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Login", login);
                        command.Parameters.AddWithValue("@Email", email);

                        var count = Convert.ToInt32(await command.ExecuteScalarAsync());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UserExistsAsync error: {ex.Message}");
                return false;
            }
        }

        // Получение роли по имени
        public async Task<Role?> GetRoleByNameAsync(string roleName)
        {
            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();

                    string query = "SELECT * FROM Role WHERE RoleName = @RoleName";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RoleName", roleName);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new Role
                                {
                                    ID = reader.GetInt32("ID"),
                                    RoleName = reader["RoleName"]?.ToString()
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetRoleByNameAsync error: {ex.Message}");
            }

            return null;
        }

        // Вспомогательный метод для безопасного чтения TimeSpan из reader
        private TimeSpan SafeGetTimeSpan(System.Data.Common.DbDataReader reader, string columnName)
        {
            try
            {
                if (!reader.IsDBNull(reader.GetOrdinal(columnName)))
                {
                    var timeStr = reader[columnName]?.ToString();
                    if (TimeSpan.TryParse(timeStr, out var timeSpan))
                    {
                        return timeSpan;
                    }
                }
            }
            catch (Exception)
            {
                // Если возникла ошибка, возвращаем TimeSpan.Zero
            }
            return TimeSpan.Zero;
        }

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
                            var user = new Users
                            {
                                ID = reader.GetInt32("ID"),
                                RoleID = reader.GetInt32("RoleID"),
                                Login = reader["Login"]?.ToString(),
                                Password = reader["Password"]?.ToString(),
                                Name = reader["Name"]?.ToString(),
                                LastName = reader["LastName"]?.ToString(),
                                Patronymic = reader["Patronymic"]?.ToString(),
                                PhoneNumber = reader["PhoneNumber"]?.ToString(),
                                Email = reader["Email"]?.ToString(),
                                BirthDate = reader.GetDateTime("BirthDate"),
                                Address = reader["Address"]?.ToString()
                            };

                            users.Add(user);
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
                        command.Parameters.AddWithValue("@RoleID", roleId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                users.Add(new Users
                                {
                                    ID = reader.GetInt32("ID"),
                                    RoleID = reader.GetInt32("RoleID"),
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

                    string query = @"INSERT INTO Users (RoleID, Login, Password, Name, LastName, Patronymic, 
                                    PhoneNumber, Email, BirthDate, Address) 
                                    VALUES (@RoleID, @Login, @Password, @Name, @LastName, @Patronymic, 
                                    @PhoneNumber, @Email, @BirthDate, @Address)";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RoleID", user.RoleID);
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
                                    RoleID = @RoleID, 
                                    Login = @Login, 
                                    Password = @Password, 
                                    Name = @Name, 
                                    LastName = @LastName, 
                                    Patronymic = @Patronymic, 
                                    PhoneNumber = @PhoneNumber, 
                                    Email = @Email, 
                                    BirthDate = @BirthDate, 
                                    Address = @Address 
                                    WHERE ID = @ID";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", user.ID);
                        command.Parameters.AddWithValue("@RoleID", user.RoleID);
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

                    string query = "DELETE FROM Users WHERE ID = @ID";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", id);
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
                            // ИСПРАВЛЕНО: правильные имена столбцов (как в базе данных)
                            goods.Add(new Goods
                            {
                                ID = reader.GetInt32("ID"), // было "Id" - должно быть "ID"
                                Name = reader["Name"]?.ToString(),
                                Price = reader.GetDecimal("Price"),
                                CategoryID = reader.GetInt32("CategoryID") // было "CategoryId" - должно быть "CategoryID"
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
                        command.Parameters.AddWithValue("@CategoryId", goods.CategoryID);

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
                                    CategoryId = @CategoryId,
                                    WHERE Id = @Id";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", goods.ID);
                        command.Parameters.AddWithValue("@Name", goods.Name);
                        command.Parameters.AddWithValue("@Price", goods.Price);
                        command.Parameters.AddWithValue("@CategoryId", goods.CategoryID);

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
                    string query = @"SELECT * FROM Orders JOIN Users WHERE Orders.ClientID = Users.ID";

                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var order = new Orders
                            {
                                ID = reader.GetInt32("ID"),
                                ClientID = reader.GetInt32("ClientID"),
                                TotalCost = reader.GetDecimal("TotalCost"),
                                Delivery = reader.GetBoolean("Delivery"),
                                CookingTime = SafeGetTimeSpan(reader, "CookingTime")
                            };

                            orders.Add(order);
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

        public async Task<bool> UpdateOrderAsync(Orders order)
        {
            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();

                    string query = @"UPDATE Orders SET 
                                    ClientID = @ClientID, 
                                    TotalCost = @TotalCost, 
                                    Delivery = @Delivery, 
                                    CookingTime = @CookingTime,
                                    Weight = @Weight,
                                    DeliveryTime = @DeliveryTime,
                                    DeliveryAddress = @DeliveryAddress
                                    WHERE ID = @ID";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", order.ID);
                        command.Parameters.AddWithValue("@ClientID", order.ClientID);
                        command.Parameters.AddWithValue("@TotalCost", order.TotalCost);
                        command.Parameters.AddWithValue("@Delivery", order.Delivery);
                        command.Parameters.AddWithValue("@CookingTime", order.CookingTime.ToString());
                        command.Parameters.AddWithValue("@Weight", order.Weight);
                        command.Parameters.AddWithValue("@DeliveryTime", order.DeliveryTime.ToString());
                        command.Parameters.AddWithValue("@DeliveryAddress", order.DeliveryAddress);

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

                    string query = "DELETE FROM Orders WHERE ID = @ID";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", id);
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

        // === CATEGORY ===
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            var categories = new List<Category>();

            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();
                    string query = "SELECT * FROM Category";

                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            categories.Add(new Category
                            {
                                Id = reader.GetInt32("ID"),
                                Name = reader["Name"]?.ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetAllCategories error: {ex.Message}");
            }

            return categories;
        }


        // === ORDERS GOODS ===
        public async Task<List<OrdersGoods>> GetOrderGoodsAsync(int orderId)
        {
            var orderGoods = new List<OrdersGoods>();

            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();
                    string query = @"
                        SELECT og.*, g.Name as Goods_Name, g.Price as Goods_Price
                        FROM OrdersGoods og
                        LEFT JOIN Goods g ON og.GoodsID = g.ID
                        WHERE og.OrderID = @OrderId";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OrderId", orderId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var orderGood = new OrdersGoods
                                {
                                    Id = reader.GetInt32("Id"),
                                    OrderID = reader.GetInt32("OrderID"),
                                    GoodsID = reader.GetInt32("GoodsID"),
                                    Name = reader["Goods_Name"]?.ToString(),
                                    Price = reader.GetDecimal("Goods_Price"),
                                    Amount = reader.GetInt32("Amount")
                                };

                                if (!reader.IsDBNull(reader.GetOrdinal("GoodsID")))
                                {
                                    orderGood.Goods = new Goods
                                    {
                                        ID = reader.GetInt32("GoodsID"),
                                        Name = reader["Goods_Name"]?.ToString(),
                                        Price = reader.GetDecimal("Goods_Price")
                                    };
                                }

                                orderGoods.Add(orderGood);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetOrderGoods error: {ex.Message}");
            }

            return orderGoods;
        }

        // === TEST CONNECTION ===
        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();

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

        public async Task<bool> CreateOrderWithGoodsAsync(Orders order, List<OrderItem> orderItems)
        {
            using (var connection = DatabaseContext.GetConnection())
            {
                await connection.OpenAsync();
                var transaction = await connection.BeginTransactionAsync();

                try
                {
                    string orderQuery = @"
                        INSERT INTO Orders (ClientID, TotalCost, Delivery, CookingTime) 
                        VALUES (@ClientID, @TotalCost, @Delivery, @CookingTime);
                        SELECT LAST_INSERT_ID();";

                    int orderId;
                    using (var orderCommand = new MySqlCommand(orderQuery, connection, (MySqlTransaction)transaction))
                    {
                        orderCommand.Parameters.AddWithValue("@ClientID", order.ClientID);
                        orderCommand.Parameters.AddWithValue("@TotalCost", order.TotalCost);
                        orderCommand.Parameters.AddWithValue("@Delivery", order.Delivery);
                        orderCommand.Parameters.AddWithValue("@CookingTime", order.CookingTime.ToString());

                        orderId = Convert.ToInt32(await orderCommand.ExecuteScalarAsync());
                    }

                    foreach (var item in orderItems)
                    {
                        string orderGoodsQuery = @"
                            INSERT INTO OrdersGoods (OrderID, GoodsID, Amount) 
                            VALUES (@OrderID, @GoodsID, @Amount)";

                        using (var ogCommand = new MySqlCommand(orderGoodsQuery, connection, (MySqlTransaction)transaction))
                        {
                            ogCommand.Parameters.AddWithValue("@OrderID", orderId);
                            ogCommand.Parameters.AddWithValue("@GoodsID", item.GoodsId);
                            ogCommand.Parameters.AddWithValue("@Amount", item.Amount);
                            await ogCommand.ExecuteNonQueryAsync();
                        }
                    }

                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine($"CreateOrderWithGoods error: {ex.Message}");
                    return false;
                }
            }
        }

        // === МЕТОДЫ ДЛЯ СОТРУДНИКОВ ===

        // Получение всех заказов для пекаря
        public async Task<List<Orders>> GetAllOrdersForBakerAsync()
        {
            var orders = new List<Orders>();

            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();

                    string query = @"
                        SELECT o.*, 
                               CONCAT(u.Name, ' ', u.LastName) as ClientName
                        FROM Orders o
                        LEFT JOIN Users u ON o.ClientID = u.ID
                        ORDER BY o.ID DESC";

                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var order = new Orders
                            {
                                ID = reader.GetInt32("ID"),
                                ClientID = reader.GetInt32("ClientID"),
                                TotalCost = reader.GetDecimal("TotalCost"),
                                Delivery = reader.GetBoolean("Delivery"),
                                Weight = reader.GetDouble("Weight"),
                                DeliveryAddress = reader["DeliveryAddress"]?.ToString(),
                                CookingTime = SafeGetTimeSpan(reader, "CookingTime"),
                                DeliveryTime = SafeGetTimeSpan(reader, "DeliveryTime")
                            };

                            orders.Add(order);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetAllOrdersForBakerAsync error: {ex.Message}");
            }

            return orders;
        }

        // Получение товаров в заказе для кассира
        public async Task<List<OrdersGoods>> GetOrderGoodsForCashierAsync(int orderId)
        {
            var orderGoods = new List<OrdersGoods>();

            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();

                    string query = @"
                        SELECT og.*, g.Name, g.Price, g.ImagePath,
                               c.Name as CategoryName
                        FROM OrdersGoods og
                        LEFT JOIN Goods g ON og.GoodsID = g.ID
                        LEFT JOIN Category c ON g.CategoryId = c.ID
                        WHERE og.OrderID = @OrderID";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OrderID", orderId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var orderGood = new OrdersGoods
                                {
                                    Id = reader.GetInt32("Id"),
                                    OrderID = reader.GetInt32("OrderID"),
                                    GoodsID = reader.GetInt32("GoodsID"),
                                    Amount = reader.GetInt32("Amount"),
                                    Price = reader.GetDecimal("Price"),
                                    Name = reader["Name"]?.ToString()
                                };

                                orderGoods.Add(orderGood);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetOrderGoodsForCashierAsync error: {ex.Message}");
            }

            return orderGoods;
        }

        // Получение активных заказов для кассира
        public async Task<List<Orders>> GetActiveOrdersForCashierAsync()
        {
            var orders = new List<Orders>();

            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();

                    string query = @"
                        SELECT DISTINCT o.*
                        FROM Orders o
                        WHERE o.ID IN (
                            SELECT og.OrderID 
                            FROM OrdersGoods og 
                            WHERE og.OrderID = o.ID
                        )
                        ORDER BY o.ID DESC";

                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var order = new Orders
                            {
                                ID = reader.GetInt32("ID"),
                                ClientID = reader.GetInt32("ClientID"),
                                TotalCost = reader.GetDecimal("TotalCost"),
                                Delivery = reader.GetBoolean("Delivery"),
                                Weight = reader.GetDouble("Weight"),
                                DeliveryAddress = reader["DeliveryAddress"]?.ToString(),
                                CookingTime = SafeGetTimeSpan(reader, "CookingTime"),
                                DeliveryTime = SafeGetTimeSpan(reader, "DeliveryTime")
                            };

                            orders.Add(order);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetActiveOrdersForCashierAsync error: {ex.Message}");
            }

            return orders;
        }

        // Получение заказов на доставку
        public async Task<List<Orders>> GetOrdersForDeliveryAsync()
        {
            var orders = new List<Orders>();

            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();

                    string query = @"
                        SELECT o.*, 
                               CONCAT(u.Name, ' ', u.LastName) as ClientName,
                               u.PhoneNumber
                        FROM Orders o
                        LEFT JOIN Users u ON o.ClientID = u.ID
                        WHERE o.Delivery = true
                        ORDER BY o.DeliveryTime ASC";

                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var order = new Orders
                            {
                                ID = reader.GetInt32("ID"),
                                ClientID = reader.GetInt32("ClientID"),
                                TotalCost = reader.GetDecimal("TotalCost"),
                                Delivery = reader.GetBoolean("Delivery"),
                                Weight = reader.GetDouble("Weight"),
                                DeliveryAddress = reader["DeliveryAddress"]?.ToString(),
                                CookingTime = SafeGetTimeSpan(reader, "CookingTime"),
                                DeliveryTime = SafeGetTimeSpan(reader, "DeliveryTime")
                            };

                            orders.Add(order);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetOrdersForDeliveryAsync error: {ex.Message}");
            }

            return orders;
        }

        // Получение активных доставок
        public async Task<List<Delivery>> GetActiveDeliveriesAsync()
        {
            var deliveries = new List<Delivery>();

            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();

                    string query = @"
                        SELECT d.*, 
                               o.DeliveryAddress as OrderAddress,
                               CONCAT(u.Name, ' ', u.LastName) as EmployeeName
                        FROM Delivery d
                        LEFT JOIN Orders o ON d.OrderID = o.ID
                        LEFT JOIN Users u ON d.EmployeeID = u.ID
                        ORDER BY d.DeliveryTime ASC";

                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var delivery = new Delivery
                            {
                                Id = reader.GetInt32("Id"),
                                DeliveryAddress = reader["DeliveryAddress"]?.ToString(),
                                PickUpAddress = reader["PickUpAddress"]?.ToString(),
                                DeliveryTime = SafeGetTimeSpan(reader, "DeliveryTime"),
                                EmployeeID = reader.GetInt32("EmployeeID"),
                                OrderID = reader.GetInt32("OrderID")
                            };

                            deliveries.Add(delivery);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetActiveDeliveriesAsync error: {ex.Message}");
            }

            return deliveries;
        }

        // Создание доставки для заказа
        public async Task<bool> CreateDeliveryForOrderAsync(int orderId, int employeeId)
        {
            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();

                    // Сначала получаем информацию о заказе
                    string getOrderQuery = "SELECT * FROM Orders WHERE ID = @OrderID";
                    using (var getCommand = new MySqlCommand(getOrderQuery, connection))
                    {
                        getCommand.Parameters.AddWithValue("@OrderID", orderId);

                        using (var reader = await getCommand.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var deliveryAddress = reader["DeliveryAddress"]?.ToString();
                                var deliveryTime = SafeGetTimeSpan(reader, "DeliveryTime");

                                await reader.CloseAsync();

                                // Создаем запись о доставке
                                string insertQuery = @"
                                    INSERT INTO Delivery (DeliveryAddress, PickUpAddress, DeliveryTime, EmployeeID, OrderID) 
                                    VALUES (@DeliveryAddress, @PickUpAddress, @DeliveryTime, @EmployeeID, @OrderID)";

                                using (var insertCommand = new MySqlCommand(insertQuery, connection))
                                {
                                    insertCommand.Parameters.AddWithValue("@DeliveryAddress", deliveryAddress);
                                    insertCommand.Parameters.AddWithValue("@PickUpAddress", "Магазин 'Шестёрочка'");
                                    insertCommand.Parameters.AddWithValue("@DeliveryTime", deliveryTime.ToString());
                                    insertCommand.Parameters.AddWithValue("@EmployeeID", employeeId);
                                    insertCommand.Parameters.AddWithValue("@OrderID", orderId);

                                    return await insertCommand.ExecuteNonQueryAsync() > 0;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CreateDeliveryForOrderAsync error: {ex.Message}");
            }

            return false;
        }

        // Обновление статуса заказа
        public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
        {
            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();

                    string query = @"
                        UPDATE Orders 
                        SET Status = @Status 
                        WHERE ID = @OrderID";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Status", status);
                        command.Parameters.AddWithValue("@OrderID", orderId);

                        return await command.ExecuteNonQueryAsync() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UpdateOrderStatusAsync error: {ex.Message}");
            }

            return false;
        }

        // === ОБНОВЛЕНИЕ ДАННЫХ ПОЛЬЗОВАТЕЛЯ ===
        public async Task<bool> UpdateUserProfileAsync(Users user)
        {
            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();

                    string query = @"UPDATE Users SET 
                            Name = @Name, 
                            LastName = @LastName, 
                            Patronymic = @Patronymic, 
                            PhoneNumber = @PhoneNumber, 
                            Email = @Email, 
                            BirthDate = @BirthDate, 
                            Address = @Address 
                            WHERE ID = @ID";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", user.ID);
                        command.Parameters.AddWithValue("@Name", user.Name ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@LastName", user.LastName ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Patronymic", user.Patronymic ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Email", user.Email ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@BirthDate", user.BirthDate);
                        command.Parameters.AddWithValue("@Address", user.Address ?? (object)DBNull.Value);

                        return await command.ExecuteNonQueryAsync() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UpdateUserProfileAsync error: {ex.Message}");
                return false;
            }
        }

        // Метод для получения текущего пользователя по ID
        public async Task<Users?> GetUserByIdAsync(int userId)
        {
            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();

                    string query = @"SELECT Users.*, Role.RoleName 
                           FROM Users
                           LEFT JOIN Role ON Users.RoleID = Role.RoleID
                           WHERE Users.ID = @UserId";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", userId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var user = new Users
                                {
                                    ID = reader.GetInt32("ID"),
                                    RoleID = reader.GetInt32("RoleID"),
                                    Login = reader["Login"]?.ToString(),
                                    Password = reader["Password"]?.ToString(),
                                    Name = reader["Name"]?.ToString(),
                                    LastName = reader["LastName"]?.ToString(),
                                    Patronymic = reader["Patronymic"]?.ToString(),
                                    PhoneNumber = reader["PhoneNumber"]?.ToString(),
                                    Email = reader["Email"]?.ToString(),
                                    BirthDate = reader.GetDateTime("BirthDate"),
                                    Address = reader["Address"]?.ToString()
                                };

                                if (!reader.IsDBNull(reader.GetOrdinal("RoleName")))
                                {
                                    user.Role = new Role
                                    {
                                        ID = reader.GetInt32("RoleID"),
                                        RoleName = reader["RoleName"]?.ToString()
                                    };
                                }

                                return user;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetUserByIdAsync error: {ex.Message}");
            }

            return null;
        }

        // Добавьте этот метод в класс DatabaseService (рядом с GetAllGoodsAsync)
        public async Task<List<Goods>> GetRandomGoodsAsync(int count)
        {
            var goods = new List<Goods>();

            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();

                    // Используем RAND() для случайной выборки и LIMIT для ограничения количества
                    string query = $"SELECT * FROM Goods ORDER BY RAND() LIMIT {count}";

                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            goods.Add(new Goods
                            {
                                ID = reader.GetInt32("ID"),
                                Name = reader["Name"]?.ToString(),
                                Price = reader.GetDecimal("Price"),
                                CategoryID = reader.GetInt32("CategoryID"),
                                ImagePath = reader["ImagePath"]?.ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetRandomGoods error: {ex.Message}");
            }

            return goods;
        }
    }
}