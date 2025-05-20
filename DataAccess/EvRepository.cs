using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using TinyHouseReservation.Models;
using TinyHouseReservations.Models;

namespace TinyHouseReservations.DataAccess
{
    public class EvRepository
    {
        private readonly DatabaseHelper _db = new DatabaseHelper();

        public void Ekle(Ev ev)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Ev (EvSahibiID, Baslik, Aciklama, Konum, Fiyat, Durum, GorselYolu) " +
                    "VALUES (@EvSahibiID, @Baslik, @Aciklama, @Konum, @Fiyat, @Durum, @GorselYolu)", conn);

                cmd.Parameters.AddWithValue("@EvSahibiID", ev.EvSahibiID);
                cmd.Parameters.AddWithValue("@Baslik", ev.Baslik);
                cmd.Parameters.AddWithValue("@Aciklama", ev.Aciklama);
                cmd.Parameters.AddWithValue("@Konum", ev.Konum);
                cmd.Parameters.AddWithValue("@Fiyat", ev.Fiyat);
                cmd.Parameters.AddWithValue("@Durum", ev.Durum);
                cmd.Parameters.AddWithValue("@GorselYolu", ev.GorselYolu ?? "");

                cmd.ExecuteNonQuery();
            }
        }
        public EvDashboardViewModel GetDashboard(int evSahibiID)
        {
            var result = new EvDashboardViewModel();

            using (SqlConnection conn = _db.GetConnection())
            {
                conn.Open();

                // Toplam Ev
                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Ev WHERE EvSahibiID = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", evSahibiID);
                    result.ToplamEv = (int)cmd.ExecuteScalar();
                }

                // Toplam Rezervasyon + Bekleyen
                using (var cmd = new SqlCommand(@"
            SELECT COUNT(*) FROM Rezervasyon R 
            INNER JOIN Ev E ON R.EvID = E.EvID
            WHERE E.EvSahibiID = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", evSahibiID);
                    result.ToplamRezervasyon = (int)cmd.ExecuteScalar();
                }

                using (var cmd = new SqlCommand(@"
            SELECT COUNT(*) FROM Rezervasyon R 
            INNER JOIN Ev E ON R.EvID = E.EvID
            WHERE E.EvSahibiID = @id AND R.Durum = 'Beklemede'", conn))
                {
                    cmd.Parameters.AddWithValue("@id", evSahibiID);
                    result.BekleyenRezervasyon = (int)cmd.ExecuteScalar();
                }

                // Toplam Gelir → Fonksiyonu çağır (fn_EvSahibiGeliri)
                using (var cmd = new SqlCommand("SELECT dbo.fn_EvSahibiGeliri(@id)", conn))
                {
                    cmd.Parameters.AddWithValue("@id", evSahibiID);
                    result.ToplamGelir = Convert.ToDecimal(cmd.ExecuteScalar());
                }

                // Son 3 yorum
                using (var cmd = new SqlCommand(@"
            SELECT TOP 3 YorumMetni FROM Yorum Y
            INNER JOIN Rezervasyon R ON Y.RezervasyonID = R.RezervasyonID
            INNER JOIN Ev E ON R.EvID = E.EvID
            WHERE E.EvSahibiID = @id
            ORDER BY YorumTarihi DESC", conn))
                {
                    cmd.Parameters.AddWithValue("@id", evSahibiID);
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        result.SonYorumlar.Add(reader["YorumMetni"].ToString());
                    }
                }
            }

            return result;
        }
        public List<Ev> GetEvlerByEvSahibiID(int evSahibiID)
        {
            List<Ev> evler = new List<Ev>();

            using (SqlConnection conn = _db.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Ev WHERE EvSahibiID = @id", conn);
                cmd.Parameters.AddWithValue("@id", evSahibiID);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    evler.Add(new Ev
                    {
                        EvID = (int)reader["EvID"],
                        EvSahibiID = (int)reader["EvSahibiID"],
                        Baslik = reader["Baslik"].ToString(),
                        Aciklama = reader["Aciklama"].ToString(),
                        Konum = reader["Konum"].ToString(),
                        Fiyat = Convert.ToDecimal(reader["Fiyat"]),
                        Durum = Convert.ToBoolean(reader["Durum"]),
                        GorselYolu = reader["GorselYolu"].ToString()
                    });
                }
            }

            return evler;
        }
        public Ev GetById(int id)
        {
            Ev ev = null;

            using (SqlConnection conn = _db.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Ev WHERE EvID = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    ev = new Ev
                    {
                        EvID = (int)reader["EvID"],
                        EvSahibiID = (int)reader["EvSahibiID"],
                        Baslik = reader["Baslik"].ToString(),
                        Aciklama = reader["Aciklama"].ToString(),
                        Konum = reader["Konum"].ToString(),
                        Fiyat = Convert.ToDecimal(reader["Fiyat"]),
                        Durum = Convert.ToBoolean(reader["Durum"]),
                        GorselYolu = reader["GorselYolu"].ToString()
                    };
                }
            }

            return ev;
        }
        public void Guncelle(Ev ev)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    @"UPDATE Ev SET Baslik = @Baslik, Aciklama = @Aciklama, Konum = @Konum,
              Fiyat = @Fiyat, Durum = @Durum, GorselYolu = @GorselYolu WHERE EvID = @EvID", conn);

                cmd.Parameters.AddWithValue("@Baslik", ev.Baslik);
                cmd.Parameters.AddWithValue("@Aciklama", ev.Aciklama);
                cmd.Parameters.AddWithValue("@Konum", ev.Konum);
                cmd.Parameters.AddWithValue("@Fiyat", ev.Fiyat);
                cmd.Parameters.AddWithValue("@Durum", ev.Durum);
                cmd.Parameters.AddWithValue("@GorselYolu", ev.GorselYolu ?? "");
                cmd.Parameters.AddWithValue("@EvID", ev.EvID);

                cmd.ExecuteNonQuery();
            }
        }
        public void DurumGuncelle(int evID, bool yeniDurum)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Ev SET Durum = @Durum WHERE EvID = @EvID", conn);
                cmd.Parameters.AddWithValue("@Durum", yeniDurum);
                cmd.Parameters.AddWithValue("@EvID", evID);
                cmd.ExecuteNonQuery();
            }
        }

        public bool Sil(int id)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                conn.Open();

                // Önce bu evin rezervasyonu var mı kontrol edelim
                SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Rezervasyon WHERE EvID = @id", conn);
                checkCmd.Parameters.AddWithValue("@id", id);

                int rezervasyonSayisi = (int)checkCmd.ExecuteScalar();

                if (rezervasyonSayisi > 0)
                {
                    // Rezervasyon var → Silme!
                    return false;
                }
                else
                {
                    // Rezervasyon yok → Sil
                    SqlCommand deleteCmd = new SqlCommand("DELETE FROM Ev WHERE EvID = @id", conn);
                    deleteCmd.Parameters.AddWithValue("@id", id);
                    deleteCmd.ExecuteNonQuery();
                    return true;
                }
            }
        }

    }
}