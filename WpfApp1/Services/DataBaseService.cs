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
                            var user = new Users
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
                throw;
            }

            return users;
        }

        public async Task<Users> GetUserWithRoleAsync(int userId)
        {
            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();
                    string query = @"
                        SELECT u.*, r.Id as Role_Id, r.RoleName 
                        FROM Users u 
                        LEFT JOIN Role r ON u.RoleId = r.Id 
                        WHERE u.Id = @UserId";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var user = new Users
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
                                };

                                if (!reader.IsDBNull(reader.GetOrdinal("Role_Id")))
                                {
                                    user.Role = new Role
                                    {
                                        Id = reader.GetInt32("Role_Id"),
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
                Console.WriteLine($"GetUserWithRole error: {ex.Message}");
            }

            return null;
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
                                CategoryId = reader.GetInt32("CategoryId"),
                                ImagePath = reader["ImagePath"]?.ToString()
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

        public async Task<Goods> GetGoodsWithCategoryAsync(int goodsId)
        {
            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();
                    string query = @"
                        SELECT g.*, c.Id as Category_Id, c.Name as Category_Name 
                        FROM Goods g 
                        LEFT JOIN Category c ON g.CategoryId = c.Id 
                        WHERE g.Id = @GoodsId";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@GoodsId", goodsId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var goods = new Goods
                                {
                                    Id = reader.GetInt32("Id"),
                                    Name = reader["Name"]?.ToString(),
                                    Price = reader.GetDecimal("Price"),
                                    CategoryId = reader.GetInt32("CategoryId"),
                                    ImagePath = reader["ImagePath"]?.ToString()
                                };

                                if (!reader.IsDBNull(reader.GetOrdinal("Category_Id")))
                                {
                                    goods.Category = new Category
                                    {
                                        Id = reader.GetInt32("Category_Id"),
                                        Name = reader["Category_Name"]?.ToString()
                                    };
                                }

                                return goods;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetGoodsWithCategory error: {ex.Message}");
            }

            return null;
        }

        public async Task<bool> AddGoodsAsync(Goods goods)
        {
            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();

                    string query = @"INSERT INTO Goods (Name, Price, CategoryId, ImagePath) 
                                    VALUES (@Name, @Price, @CategoryId, @ImagePath)";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", goods.Name);
                        command.Parameters.AddWithValue("@Price", goods.Price);
                        command.Parameters.AddWithValue("@CategoryId", goods.CategoryId);
                        command.Parameters.AddWithValue("@ImagePath", goods.ImagePath);

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
                                    ImagePath = @ImagePath
                                    WHERE Id = @Id";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", goods.Id);
                        command.Parameters.AddWithValue("@Name", goods.Name);
                        command.Parameters.AddWithValue("@Price", goods.Price);
                        command.Parameters.AddWithValue("@CategoryId", goods.CategoryId);
                        command.Parameters.AddWithValue("@ImagePath", goods.ImagePath);

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
                            var order = new Orders
                            {
                                Id = reader.GetInt32("Id"),
                                ClientId = reader.GetInt32("ClientId"),
                                TotalCost = reader.GetDecimal("TotalCost"),
                                Delivery = reader.GetBoolean("Delivery"),
                                Weight = reader.GetDouble("Weight"),
                                DeliveryAddress = reader["DeliveryAddress"]?.ToString()
                            };

                            // Для TimeSpan используем альтернативный подход
                            var cookingTimeStr = reader["CookingTime"]?.ToString();
                            if (!string.IsNullOrEmpty(cookingTimeStr))
                            {
                                if (TimeSpan.TryParse(cookingTimeStr, out var cookingTime))
                                {
                                    order.CookingTime = cookingTime;
                                }
                            }

                            var deliveryTimeStr = reader["DeliveryTime"]?.ToString();
                            if (!string.IsNullOrEmpty(deliveryTimeStr))
                            {
                                if (TimeSpan.TryParse(deliveryTimeStr, out var deliveryTime))
                                {
                                    order.DeliveryTime = deliveryTime;
                                }
                            }

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

        public async Task<Orders> GetOrderWithDetailsAsync(int orderId)
        {
            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();
                    string query = @"
                        SELECT o.*, u.Id as Client_Id, u.Name as Client_Name, u.LastName as Client_LastName
                        FROM Orders o 
                        LEFT JOIN Users u ON o.ClientId = u.Id 
                        WHERE o.Id = @OrderId";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OrderId", orderId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var order = new Orders
                                {
                                    Id = reader.GetInt32("Id"),
                                    ClientId = reader.GetInt32("ClientId"),
                                    TotalCost = reader.GetDecimal("TotalCost"),
                                    Delivery = reader.GetBoolean("Delivery"),
                                    Weight = reader.GetDouble("Weight"),
                                    DeliveryAddress = reader["DeliveryAddress"]?.ToString()
                                };

                                // Для TimeSpan используем альтернативный подход
                                var cookingTimeStr = reader["CookingTime"]?.ToString();
                                if (!string.IsNullOrEmpty(cookingTimeStr))
                                {
                                    if (TimeSpan.TryParse(cookingTimeStr, out var cookingTime))
                                    {
                                        order.CookingTime = cookingTime;
                                    }
                                }

                                var deliveryTimeStr = reader["DeliveryTime"]?.ToString();
                                if (!string.IsNullOrEmpty(deliveryTimeStr))
                                {
                                    if (TimeSpan.TryParse(deliveryTimeStr, out var deliveryTime))
                                    {
                                        order.DeliveryTime = deliveryTime;
                                    }
                                }

                                if (!reader.IsDBNull(reader.GetOrdinal("Client_Id")))
                                {
                                    order.Client = new Users
                                    {
                                        Id = reader.GetInt32("Client_Id"),
                                        Name = reader["Client_Name"]?.ToString(),
                                        LastName = reader["Client_LastName"]?.ToString()
                                    };
                                }

                                return order;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetOrderWithDetails error: {ex.Message}");
            }

            return null;
        }

        public async Task<bool> AddOrderAsync(Orders order)
        {
            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();

                    string query = @"INSERT INTO Orders (ClientId, TotalCost, Delivery, CookingTime, Weight, DeliveryTime, DeliveryAddress) 
                                    VALUES (@ClientId, @TotalCost, @Delivery, @CookingTime, @Weight, @DeliveryTime, @DeliveryAddress)";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ClientId", order.ClientId);
                        command.Parameters.AddWithValue("@TotalCost", order.TotalCost);
                        command.Parameters.AddWithValue("@Delivery", order.Delivery);
                        command.Parameters.AddWithValue("@CookingTime", order.CookingTime);
                        command.Parameters.AddWithValue("@Weight", order.Weight);
                        command.Parameters.AddWithValue("@DeliveryTime", order.DeliveryTime);
                        command.Parameters.AddWithValue("@DeliveryAddress", order.DeliveryAddress);

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
                                    CookingTime = @CookingTime,
                                    Weight = @Weight,
                                    DeliveryTime = @DeliveryTime,
                                    DeliveryAddress = @DeliveryAddress
                                    WHERE Id = @Id";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", order.Id);
                        command.Parameters.AddWithValue("@ClientId", order.ClientId);
                        command.Parameters.AddWithValue("@TotalCost", order.TotalCost);
                        command.Parameters.AddWithValue("@Delivery", order.Delivery);
                        command.Parameters.AddWithValue("@CookingTime", order.CookingTime);
                        command.Parameters.AddWithValue("@Weight", order.Weight);
                        command.Parameters.AddWithValue("@DeliveryTime", order.DeliveryTime);
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
                                Id = reader.GetInt32("Id"),
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

        // === ROLE ===
        public async Task<List<Role>> GetAllRolesAsync()
        {
            var roles = new List<Role>();

            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();
                    string query = "SELECT * FROM Role";

                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            roles.Add(new Role
                            {
                                Id = reader.GetInt32("Id"),
                                RoleName = reader["RoleName"]?.ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetAllRoles error: {ex.Message}");
            }

            return roles;
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
                        LEFT JOIN Goods g ON og.GoodsID = g.Id
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
                                        Id = reader.GetInt32("GoodsID"),
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

        // В классе DatabaseService добавьте:

        // === POSTLIST ===
        public async Task<List<Postlist>> GetAllPostsAsync()
        {
            var posts = new List<Postlist>();

            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();
                    string query = "SELECT * FROM Postlist";

                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            posts.Add(new Postlist
                            {
                                Id = reader.GetInt32("Id"),
                                Name = reader["Name"]?.ToString(),
                                Salary = reader.IsDBNull("Salary") ? (decimal?)null : reader.GetDecimal("Salary")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetAllPosts error: {ex.Message}");
            }

            return posts;
        }

        public async Task<int> CreatePostAsync(Postlist post)
        {
            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();

                    string query = @"INSERT INTO Postlist (Name, Salary) 
                            VALUES (@Name, @Salary);
                            SELECT LAST_INSERT_ID();";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", post.Name);
                        command.Parameters.AddWithValue("@Salary", post.Salary);

                        var result = await command.ExecuteScalarAsync();
                        return result != null ? Convert.ToInt32(result) : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CreatePost error: {ex.Message}");
                return 0;
            }
        }

        // === USERPOST ===
        public async Task<bool> CreateUserPostAsync(UserPost userPost)
        {
            try
            {
                using (var connection = DatabaseContext.GetConnection())
                {
                    await connection.OpenAsync();

                    string query = @"INSERT INTO UserPost (UserID, PostID) 
                            VALUES (@UserID, @PostID)";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", userPost.UserID);
                        command.Parameters.AddWithValue("@PostID", userPost.PostID);

                        return await command.ExecuteNonQueryAsync() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CreateUserPost error: {ex.Message}");
                return false;
            }
        }

        // Метод для поиска или создания должности
        public async Task<int> GetOrCreatePostIdAsync(string postName, decimal? salary)
        {
            try
            {
                // Сначала ищем существующую должность
                var posts = await GetAllPostsAsync();
                var existingPost = posts.FirstOrDefault(p =>
                    p.Name != null && p.Name.Equals(postName, StringComparison.OrdinalIgnoreCase));

                if (existingPost != null)
                {
                    return existingPost.Id;
                }

                // Если не найдено, создаем новую
                var newPost = new Postlist
                {
                    Name = postName,
                    Salary = salary
                };

                return await CreatePostAsync(newPost);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetOrCreatePostId error: {ex.Message}");
                return 0;
            }
        }
    }
}