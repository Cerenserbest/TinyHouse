using System.Net.NetworkInformation;
using System.Reflection;
using Microsoft.Data.SqlClient;
using TinyHouseReservations.Models;
using System.Data;


namespace TinyHouseReservations.DataAccess
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper()
        {
            // Buraya kendi SQL Server bağlantını yaz
            _connectionString = "Server=BERSU\\SQLEXPRESS ;Database=TinyHouseDB;Trusted_Connection=True;TrustServerCertificate=True;";
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
        public Kullanici? GetKullaniciByEmailAndSifre(string email, string sifre)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM Kullanici WHERE Email = @Email AND Sifre = @Sifre";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Sifre", sifre);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Kullanici
                            {
                                KullaniciID = reader.GetInt32(reader.GetOrdinal("KullaniciID")),
                                Ad = reader.GetString(reader.GetOrdinal("Ad")),
                                Soyad = reader.GetString(reader.GetOrdinal("Soyad")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Sifre = reader.GetString(reader.GetOrdinal("Sifre")),
                                RolID = reader.GetInt32(reader.GetOrdinal("RolID"))
                            };
                        }
                    }
                }
            }

            return null;
        }
    
}
}
