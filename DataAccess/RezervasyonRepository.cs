using TinyHouseReservations.Models;
using Microsoft.Data.SqlClient;


namespace TinyHouseReservations.DataAccess
{
    public class RezervasyonRepository
    {
        private readonly DatabaseHelper _db = new DatabaseHelper();

        public List<RezervasyonDetay> GetByEvSahibiID(int evSahibiID)
        {
            List<RezervasyonDetay> rezervasyonlar = new List<RezervasyonDetay>();

            using (SqlConnection conn = _db.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
            SELECT R.RezervasyonID, E.Baslik, K.Ad + ' ' + K.Soyad AS KiraciAdSoyad,
                   R.BaslangicTarihi, R.BitisTarihi, R.Durum, R.OdemeDurumu
            FROM Rezervasyon R
            INNER JOIN Ev E ON R.EvID = E.EvID
            INNER JOIN Kullanici K ON R.KiraciID = K.KullaniciID
            WHERE E.EvSahibiID = @id
            ORDER BY R.BaslangicTarihi DESC", conn);

                cmd.Parameters.AddWithValue("@id", evSahibiID);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    rezervasyonlar.Add(new RezervasyonDetay
                    {
                        RezervasyonID = (int)reader["RezervasyonID"],
                        EvBaslik = reader["Baslik"].ToString(),
                        KiraciAdSoyad = reader["KiraciAdSoyad"].ToString(),
                        Baslangic = Convert.ToDateTime(reader["BaslangicTarihi"]),
                        Bitis = Convert.ToDateTime(reader["BitisTarihi"]),
                        Durum = reader["Durum"].ToString(),
                        OdemeDurumu = Convert.ToBoolean(reader["OdemeDurumu"])
                    });
                }
            }

            return rezervasyonlar;
        }
        public void DurumGuncelle(int rezervasyonID, string yeniDurum)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Rezervasyon SET Durum = @durum WHERE RezervasyonID = @id", conn);
                cmd.Parameters.AddWithValue("@durum", yeniDurum);
                cmd.Parameters.AddWithValue("@id", rezervasyonID);
                cmd.ExecuteNonQuery();
            }
        }
        public RezervasyonDetay GetDetayByID(int rezervasyonID)
        {
            RezervasyonDetay detay = null;

            using (SqlConnection conn = _db.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
            SELECT R.RezervasyonID, E.Baslik, K.Ad + ' ' + K.Soyad AS KiraciAdSoyad,
                   R.BaslangicTarihi, R.BitisTarihi, R.Durum, R.OdemeDurumu
            FROM Rezervasyon R
            INNER JOIN Ev E ON R.EvID = E.EvID
            INNER JOIN Kullanici K ON R.KiraciID = K.KullaniciID
            WHERE R.RezervasyonID = @id", conn);

                cmd.Parameters.AddWithValue("@id", rezervasyonID);
                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    detay = new RezervasyonDetay
                    {
                        RezervasyonID = (int)reader["RezervasyonID"],
                        EvBaslik = reader["Baslik"].ToString(),
                        KiraciAdSoyad = reader["KiraciAdSoyad"].ToString(),
                        Baslangic = Convert.ToDateTime(reader["BaslangicTarihi"]),
                        Bitis = Convert.ToDateTime(reader["BitisTarihi"]),
                        Durum = reader["Durum"].ToString(),
                        OdemeDurumu = Convert.ToBoolean(reader["OdemeDurumu"])
                    };
                }
            }

            return detay;
        }

    }
}
