using Microsoft.Data.SqlClient;
using TinyHouseReservations.Models;
using System.Collections.Generic;
using System;
using TinyHouseReservation.Models;

namespace TinyHouseReservations.DataAccess
{
    public class OdemeRepository
    {
        private readonly DatabaseHelper _db = new DatabaseHelper();

        public List<OdemeDetay> GetOdemelerByEvSahibiID(int evSahibiID)
        {
            List<OdemeDetay> odemeler = new List<OdemeDetay>();

            using (SqlConnection conn = _db.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
            SELECT E.Baslik, O.Tutar, O.OdemeTarihi, O.OdemeTipi
            FROM Odeme O
            INNER JOIN Rezervasyon R ON O.RezervasyonID = R.RezervasyonID
            INNER JOIN Ev E ON R.EvID = E.EvID
            WHERE E.EvSahibiID = @id
            ORDER BY O.OdemeTarihi DESC", conn);

                cmd.Parameters.AddWithValue("@id", evSahibiID);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    odemeler.Add(new OdemeDetay
                    {
                        EvBaslik = reader["Baslik"].ToString(),
                        Tutar = Convert.ToDecimal(reader["Tutar"]),
                        OdemeTarihi = Convert.ToDateTime(reader["OdemeTarihi"]),
                        OdemeTipi = reader["OdemeTipi"].ToString()
                    });
                }
            }

            return odemeler;
        }

    }
}
