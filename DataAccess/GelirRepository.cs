using Microsoft.Data.SqlClient;
using TinyHouseReservations.Models;
using System.Collections.Generic;
using System;
using TinyHouseReservations.Models;

namespace TinyHouseReservations.DataAccess
{
    public class GelirRepository
    {
        private readonly DatabaseHelper _db = new DatabaseHelper();

        public List<GelirDetay> GetGelirlerByEvSahibiID(int evSahibiID)
        {
            List<GelirDetay> gelirler = new List<GelirDetay>();

            using (SqlConnection conn = _db.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
                    SELECT E.Baslik, SUM(O.Tutar) AS ToplamGelir
                    FROM Odeme O
                    INNER JOIN Rezervasyon R ON O.RezervasyonID = R.RezervasyonID
                    INNER JOIN Ev E ON R.EvID = E.EvID
                    WHERE E.EvSahibiID = @id
                    GROUP BY E.Baslik", conn);

                cmd.Parameters.AddWithValue("@id", evSahibiID);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    gelirler.Add(new GelirDetay
                    {
                        EvBaslik = reader["Baslik"].ToString(),
                        ToplamGelir = Convert.ToDecimal(reader["ToplamGelir"])
                    });
                }
            }

            return gelirler;
        }
    }
}
