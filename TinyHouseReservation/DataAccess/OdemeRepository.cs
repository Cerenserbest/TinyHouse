using Microsoft.Data.SqlClient;

using TinyHouseReservation.Models;
using TinyHouseReservations.Models;

namespace TinyHouseReservations.DataAccess
{
    public class OdemeRepository
    {
        private readonly DatabaseHelper _db = new DatabaseHelper();

        public DatabaseHelper Get_db()
        {
            return _db;
        }

        [Obsolete]
        public List<Odeme> TumOdemeleriGetir()
        {
            List<Odeme> odemeler = new List<Odeme>();

            using (SqlConnection conn = _db.GetConnection())
            {
                conn.Open(); SqlCommand cmd = new SqlCommand(@"
    SELECT 
        o.OdemeID, 
        ISNULL(r.KiraciID, 0) AS KiraciID, 
        o.Tutar, 
        o.OdemeTarihi, 
        o.OdemeTipi
    FROM Odeme o
    LEFT JOIN Rezervasyon r ON o.RezervasyonID = r.RezervasyonID
", conn);


                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    odemeler.Add(new Odeme
                    {
                        OdemeID = reader.GetInt32(reader.GetOrdinal("OdemeID")),
                        KullaniciID = reader.GetInt32(reader.GetOrdinal("KiraciID")),
                        Tutar = reader.GetDecimal(reader.GetOrdinal("Tutar")),
                        Tarih = reader.GetDateTime(reader.GetOrdinal("OdemeTarihi")),
                        OdemeTipi = reader["OdemeTipi"].ToString()
                    });
                }

            }

            return odemeler;
        }

       
    }
}
