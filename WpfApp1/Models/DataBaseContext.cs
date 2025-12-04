using System.Configuration;
using MySql.Data.MySqlClient;

namespace WpfApp1.Models
{
    public static class DatabaseContext
    {
        // Обновленная строка подключения с правильными параметрами
        public static string ConnectionString { get; } =
            "Server=tompsons.beget.tech;Database=tompsons_stud21;Uid=tompsons_stud21;Pwd=123456Zz;Port=3306;SslMode=Preferred;CharSet=utf8;ConnectionTimeout=30;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public static async Task<bool> TestConnectionAsync()
        {
            try
            {
                using (var connection = GetConnection())
                {
                    await connection.OpenAsync();

                    // Проверяем, что соединение действительно открыто
                    return connection.State == System.Data.ConnectionState.Open;
                }
            }
            catch (MySqlException mysqlEx)
            {
                Console.WriteLine($"MySQL Error: {mysqlEx.Number} - {mysqlEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test connection failed: {ex.Message}");
                return false;
            }
        }
    }
}