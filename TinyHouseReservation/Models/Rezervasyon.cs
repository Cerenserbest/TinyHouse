namespace TinyHouseReservations.Models
{
    public class Rezervasyon
    {
        public int RezervasyonID { get; set; }
        public int EvID { get; set; }
        public int KiraciID { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public string? OdemeDurumu { get; set; }
        public bool Durum { get; set; }

        // Ekstra gösterim alanları (sadece 1 kez!)
        public string? EvBaslik { get; set; }
        public string? Konum { get; set; }
        public string? KullaniciAd { get; set; }
        public int KullaniciID { get; set; }





    }
}
 