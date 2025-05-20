namespace TinyHouseReservations.Models
{
    public class RezervasyonDetay
    {
        public int RezervasyonID { get; set; }
        public string EvBaslik { get; set; }
        public string KiraciAdSoyad { get; set; }
        public DateTime Baslangic { get; set; }
        public DateTime Bitis { get; set; }
        public string Durum { get; set; }
        public bool OdemeDurumu { get; set; }
    }
}
