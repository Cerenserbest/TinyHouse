using Microsoft.Data.SqlClient;
using TinyHouseReservations.Models;
using System.Collections.Generic;
using System;

namespace TinyHouseReservations.DataAccess
{
    public class YorumRepository
    {
        private readonly DatabaseHelper _db = new DatabaseHelper();

        public List<YorumDetay> GetByEvSahibiID(int evSahibiID)
        {
            List<YorumDetay> yorumlar = new List<YorumDetay>();

            using (SqlConnection conn = _db.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
                    SELECT E.Baslik, K.Ad + ' ' + K.Soyad AS KiraciAdSoyad, 
                           Y.Puan, Y.YorumMetni, Y.YorumTarihi
                    FROM Yorum Y
                    INNER JOIN Rezervasyon R ON Y.RezervasyonID = R.RezervasyonID
                    INNER JOIN Ev E ON R.EvID = E.EvID
                    INNER JOIN Kullanici K ON R.KiraciID = K.KullaniciID
                    WHERE E.EvSahibiID = @id
                    ORDER BY Y.YorumTarihi DESC", conn);

                cmd.Parameters.AddWithValue("@id", evSahibiID);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    yorumlar.Add(new YorumDetay
                    {
                        EvBaslik = reader["Baslik"].ToString(),
                        KiraciAdSoyad = reader["KiraciAdSoyad"].ToString(),
                        Puan = Convert.ToInt32(reader["Puan"]),
                        YorumMetni = reader["YorumMetni"].ToString(),
                        YorumTarihi = Convert.ToDateTime(reader["YorumTarihi"])
                    });
                }
            }

            return yorumlar;
        }
    }
}
