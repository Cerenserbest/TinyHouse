using Microsoft.Data.SqlClient;

namespace TinyHouseReservations.DataAccess
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper()
        {
            // Buraya kendi SQL Server bağlantını yaz
            _connectionString = "Server=CEREN\\SQLEXPRESS;Database=TinyHouseDb;Trusted_Connection=True;TrustServerCertificate=True;";
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        internal object GetKullaniciByEmailAndSifre(string email, string sifre)
        {
            throw new NotImplementedException();
        }
    }
}
