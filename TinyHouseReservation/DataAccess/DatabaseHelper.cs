using Microsoft.Data.SqlClient; 

namespace TinyHouseReservations.DataAccess
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper()
        {
            // Buraya kendi SQL Server bağlantını yaz
            _connectionString = "Server=BEYZA;Database=TinyHouseDB;Trusted_Connection=True;TrustServerCertificate=True;";
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
