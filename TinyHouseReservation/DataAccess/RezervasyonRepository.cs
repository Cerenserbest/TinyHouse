using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using TinyHouseReservations.Models;

namespace TinyHouseReservations.DataAccess
{
    public class RezervasyonRepository
    {
        private readonly DatabaseHelper _db = new DatabaseHelper();

        public Rezervasyon RezervasyonGetirById(int id)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
                    SELECT r.*, 
                           e.Baslik AS EvBaslik, 
                           e.Konum, 
                           k.Ad AS KullaniciAd
                    FROM Rezervasyon r
                    JOIN Ev e ON r.EvID = e.EvID
                    JOIN Kullanici k ON r.KiraciID = k.KullaniciID
                    WHERE r.RezervasyonID = @id", conn);

                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Rezervasyon
                    {
                        RezervasyonID = (int)reader["RezervasyonID"],
                        EvID = (int)reader["EvID"],
                        KullaniciID = (int)reader["KiraciID"],
                        BaslangicTarihi = Convert.ToDateTime(reader["BaslangicTarihi"]),
                        BitisTarihi = Convert.ToDateTime(reader["BitisTarihi"]),
                        OdemeDurumu = (bool)reader["OdemeDurumu"] ? "Ödendi" : "Bekleniyor",
                        Durum = reader["Durum"].ToString() == "Aktif",
                        EvBaslik = reader["EvBaslik"].ToString(),
                        Konum = reader["Konum"].ToString(),
                        KullaniciAd = reader["KullaniciAd"].ToString()
                    };
                }

                return null!;
            }
        }

        public List<Rezervasyon> TumRezervasyonlariGetir()
        {
            var liste = new List<Rezervasyon>();

            using (SqlConnection conn = _db.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Rezervasyon", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    liste.Add(new Rezervasyon
                    {
                        RezervasyonID = (int)reader["RezervasyonID"],
                        EvID = (int)reader["EvID"],
                        KullaniciID = (int)reader["KiraciID"],
                        BaslangicTarihi = Convert.ToDateTime(reader["BaslangicTarihi"]),
                        BitisTarihi = Convert.ToDateTime(reader["BitisTarihi"]),
                        OdemeDurumu = (bool)reader["OdemeDurumu"] ? "Ödendi" : "Bekleniyor",
                        Durum = reader["Durum"].ToString() == "Aktif"
                    });
                }
            }

            return liste;
        }

        public void RezervasyonIptal(int rezervasyonId)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Rezervasyonlar SET Durum = 'İptal' WHERE RezervasyonID = @id", conn);
                cmd.Parameters.AddWithValue("@id", rezervasyonId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}

