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

        // Buraya İlan Ekle, Güncelle, Sil vs. fonksiyonlar eklenebilir
    }
}
