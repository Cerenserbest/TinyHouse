// En üstte bu satırı kullan:
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using TinyHouseReservations.Models;


namespace TinyHouseReservations.DataAccess
{
    public class EvRepository
    {
        private readonly DatabaseHelper _db = new DatabaseHelper();

        // Örnek: Evleri listele (kendi ilanları için)
        public List<Ev> EvleriGetir(int evSahibiId)
        {
            var evler = new List<Ev>();

            using (SqlConnection conn = _db.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Ev WHERE EvSahibiID = @id", conn);
                cmd.Parameters.AddWithValue("@id", evSahibiId);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    evler.Add(new Ev
                    {
                        EvID = (int)reader["EvID"],
                        Baslik = reader["Baslik"].ToString(),
                        Aciklama = reader["Aciklama"].ToString(),
                        Konum = reader["Konum"].ToString(),
                        Fiyat = (decimal)reader["Fiyat"],
                        Durum = (bool)reader["Durum"],
                        GorselYolu = reader["GorselYolu"].ToString()
                    });
                }
            }

            return evler;
        }
        public List<Ev> TumEvleriGetir()
        {
            var evler = new List<Ev>();

            using (SqlConnection conn = _db.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Ev", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    evler.Add(new Ev
                    {
                        EvID = (int)reader["EvID"],
                        Baslik = reader["Baslik"].ToString(),
                        Aciklama = reader["Aciklama"].ToString(),
                        Konum = reader["Konum"].ToString(),
                        Fiyat = (decimal)reader["Fiyat"],
                        Durum = (bool)reader["Durum"],
                        GorselYolu = reader["GorselYolu"].ToString()
                    });
                }
            }

            return evler;
        }

        public Ev? EvGetirById(int id)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Ev WHERE EvID = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Ev
                    {
                        EvID = (int)reader["EvID"],
                        Baslik = reader["Baslik"].ToString(),
                        Aciklama = reader["Aciklama"].ToString(),
                        Konum = reader["Konum"].ToString(),
                        Fiyat = (decimal)reader["Fiyat"],
                        Durum = (bool)reader["Durum"],
                        GorselYolu = reader["GorselYolu"].ToString()
                    };
                }
            }
            return null!;
        }


        public void EvGuncelle(Ev ev)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"UPDATE Ev 
            SET Baslik = @Baslik, Aciklama = @Aciklama, Konum = @Konum, 
                Fiyat = @Fiyat, Durum = @Durum, GorselYolu = @GorselYolu 
            WHERE EvID = @EvID", conn);

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
        public void EvSil(int id)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Ev WHERE EvID = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }



        // Buraya İlan Ekle, Güncelle, Sil vs. fonksiyonlar eklenebilir
    }
}
