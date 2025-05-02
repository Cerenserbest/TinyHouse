using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using TinyHouseReservations.Models;

namespace TinyHouseReservations.DataAccess
{
    public class RezervasyonRepository
    {
        private readonly DatabaseHelper _db = new DatabaseHelper();

        // Örnek: Kiracının yaptığı tüm rezervasyonları getir
        public List<Rezervasyon> KiraciRezervasyonlariniGetir(int kiraciId)
        {
            var liste = new List<Rezervasyon>();

            using (SqlConnection conn = _db.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Rezervasyon WHERE KiraciID = @id", conn);
                cmd.Parameters.AddWithValue("@id", kiraciId);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    liste.Add(new Rezervasyon
                    {
                        RezervasyonID = (int)reader["RezervasyonID"],
                        EvID = (int)reader["EvID"],
                        KiraciID = (int)reader["KiraciID"],
                        BaslangicTarihi = (DateTime)reader["BaslangicTarihi"],
                        BitisTarihi = (DateTime)reader["BitisTarihi"],
                        OdemeDurumu = (bool)reader["OdemeDurumu"],
                        Durum = reader["Durum"].ToString()
                    });
                }
            }

            return liste;
        }

        // Daha sonra: Ekle, Güncelle, Iptal Et gibi metotlar eklenebilir
    }
}

