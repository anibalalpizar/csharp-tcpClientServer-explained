using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Server.Utils
{
    public class DatabaseUtils
    {
        public static bool TestDatabaseConnection()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al conectar a la base de datos: {ex.Message}");
                return false;
            }
        }

        public static SqlConnection GetConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
            return new SqlConnection(connectionString);
        }
    }
}
