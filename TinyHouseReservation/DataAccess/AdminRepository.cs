using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using TinyHouseReservations.Models;

namespace TinyHouseReservations.DataAccess
{
    public class AdminRepository
    {
        private readonly DatabaseHelper _db = new DatabaseHelper();

        // Örnek: Sistemdeki tüm kullanıcıları listele
        public List<Kullanici> TumKullanicilariGetir()
        {
            var liste = new List<Kullanici>();

            using (SqlConnection conn = _db.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Kullanici", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    liste.Add(new Kullanici
                    {
                        KullaniciID = (int)reader["KullaniciID"],
                        Ad = reader["Ad"].ToString(),
                        Soyad = reader["Soyad"].ToString(),
                        Email = reader["Email"].ToString(),
                        Sifre = reader["Sifre"].ToString(),
                        RolID = (int)reader["RolID"]
                    });
                }
            }

            return liste;
        }

        // Not: Diğer işlemler (kullanıcı ekle, sil, güncelle) sonradan eklenebilir
    }
}
