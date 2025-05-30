using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using TinyHouseReservations.Models;
using TinyHouseReservations.DataAccess;

namespace TinyHouseReservations.Controllers
{
    public class AdminRepository
    {
        private readonly DatabaseHelper _db = new DatabaseHelper();
        public SqlConnection GetConnection()
        {
            string connectionString = "Server=localhost\\SQLExpress;Database=TinyHouseDB;Trusted_Connection=True;";
            return new SqlConnection(connectionString);
        }

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
                        RolID = (int)reader["RolID"],
                        Durum = (bool)reader["Durum"]

                    });
                }
            }

            return liste;
        }

        internal void KullaniciEkle(Kullanici yeniKullanici)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
            INSERT INTO Kullanici (Ad, Soyad, Email, Sifre, RolID)
            VALUES (@Ad, @Soyad, @Email, @Sifre, @RolID)", conn);

                cmd.Parameters.AddWithValue("@Ad", yeniKullanici.Ad);
                cmd.Parameters.AddWithValue("@Soyad", yeniKullanici.Soyad);
                cmd.Parameters.AddWithValue("@Email", yeniKullanici.Email);
                cmd.Parameters.AddWithValue("@Sifre", yeniKullanici.Sifre); // Şifreyi ileride hashle
                cmd.Parameters.AddWithValue("@RolID", yeniKullanici.RolID);

                cmd.ExecuteNonQuery();
            }
        
    }

        public Kullanici KullaniciGetirById(int id)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Kullanici WHERE KullaniciID = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Kullanici
                    {
                        KullaniciID = (int)reader["KullaniciID"],
                        Ad = reader["Ad"].ToString(),
                        Soyad = reader["Soyad"].ToString(),
                        Email = reader["Email"].ToString(),
                        Sifre = reader["Sifre"].ToString(),
                        RolID = (int)reader["RolID"],
             
                        Durum = (bool)reader["Durum"]

                    };
                }
            }
            return null;
        }
        public void KullaniciGuncelle(Kullanici k)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
            UPDATE Kullanici 
            SET Ad = @Ad, Soyad = @Soyad, Email = @Email, Sifre = @Sifre, RolID = @RolID, Durum = @Durum
            WHERE KullaniciID = @id", conn);

                cmd.Parameters.AddWithValue("@id", k.KullaniciID);
                cmd.Parameters.AddWithValue("@Ad", k.Ad);
                cmd.Parameters.AddWithValue("@Soyad", k.Soyad);
                cmd.Parameters.AddWithValue("@Email", k.Email);
                cmd.Parameters.AddWithValue("@Sifre", k.Sifre);
                cmd.Parameters.AddWithValue("@RolID", k.RolID);
                cmd.Parameters.AddWithValue("@Durum", k.Durum);

                cmd.ExecuteNonQuery();
            }
        }



        public void KullaniciSil(int id)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Kullanici WHERE KullaniciID = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
        public void KullaniciDurumDegistir(int id)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                conn.Open();

                // 1. Mevcut durumu al
                SqlCommand getCmd = new SqlCommand("SELECT Durum FROM Kullanici WHERE KullaniciID = @id", conn);
                getCmd.Parameters.AddWithValue("@id", id);
                object result = getCmd.ExecuteScalar();

                if (result == null) return; // Böyle bir kullanıcı yoksa çık

                bool mevcutDurum = Convert.ToBoolean(result);

                // 2. Tersini yaz
                SqlCommand updateCmd = new SqlCommand("UPDATE Kullanici SET Durum = @durum WHERE KullaniciID = @id", conn);
                updateCmd.Parameters.AddWithValue("@durum", !mevcutDurum);
                updateCmd.Parameters.AddWithValue("@id", id);
                updateCmd.ExecuteNonQuery();
            }
        }

        public List<(string Ay, int RezervasyonSayisi)> AylikRezervasyonSayisi()
{
    var data = new List<(string, int)>();
    using (var conn = _db.GetConnection())
    {
        conn.Open();
        SqlCommand cmd = new SqlCommand(@"
            SELECT 
                FORMAT(BaslangicTarihi, 'yyyy-MM') AS Ay,
                COUNT(*) AS RezervasyonSayisi
            FROM Rezervasyon
            GROUP BY FORMAT(BaslangicTarihi, 'yyyy-MM')
            ORDER BY Ay", conn);

        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            data.Add((reader["Ay"].ToString(), (int)reader["RezervasyonSayisi"]));
        }
    }
    return data;
}

public List<(string Ay, decimal Tutar)> AylikOdemeToplami()
{
    var data = new List<(string, decimal)>();
    using (var conn = _db.GetConnection())
    {
        conn.Open();
        SqlCommand cmd = new SqlCommand(@"
            SELECT 
                FORMAT(OdemeTarihi, 'yyyy-MM') AS Ay,
                SUM(Tutar) AS Tutar
            FROM Odeme
            GROUP BY FORMAT(OdemeTarihi, 'yyyy-MM')
            ORDER BY Ay", conn);

        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            data.Add((reader["Ay"].ToString(), (decimal)reader["Tutar"]));
        }
    }
    return data;
}



        // Not: Diğer işlemler (kullanıcı ekle, sil, güncelle) sonradan eklenebilir
    }
}
