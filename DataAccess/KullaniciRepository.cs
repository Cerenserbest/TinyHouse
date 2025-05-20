using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using TinyHouseReservations.Models;

namespace TinyHouseReservations.DataAccess
{
    public class KullaniciRepository
    {
        private readonly DatabaseHelper _db = new DatabaseHelper();

        public List<Kullanici> GetAll()
        {
            List<Kullanici> kullanicilar = new List<Kullanici>();

            using (SqlConnection conn = _db.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Kullanici", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    kullanicilar.Add(new Kullanici
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

            return kullanicilar;
        }

        public void Ekle(Kullanici k)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Kullanici (Ad, Soyad, Email, Sifre, RolID) VALUES (@Ad, @Soyad, @Email, @Sifre, @RolID)", conn);

                cmd.Parameters.AddWithValue("@Ad", k.Ad);
                cmd.Parameters.AddWithValue("@Soyad", k.Soyad);
                cmd.Parameters.AddWithValue("@Email", k.Email);
                cmd.Parameters.AddWithValue("@Sifre", k.Sifre);
                cmd.Parameters.AddWithValue("@RolID", k.RolID);

                cmd.ExecuteNonQuery();
            }
        }

        public Kullanici GirisYap(string email, string sifre)
        {
            Kullanici k = null;

            using (SqlConnection conn = _db.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Kullanici WHERE Email = @Email AND Sifre = @Sifre", conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Sifre", sifre);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    k = new Kullanici
                    {
                        KullaniciID = (int)reader["KullaniciID"],
                        Ad = reader["Ad"].ToString(),
                        Soyad = reader["Soyad"].ToString(),
                        Email = reader["Email"].ToString(),
                        Sifre = reader["Sifre"].ToString(),
                        RolID = (int)reader["RolID"]
                    };
                }
            }

            return k;
        }
    }
}

