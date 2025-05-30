
namespace TinyHouseReservations.Models
{
    public class Kullanici
    {
        public int KullaniciID { get; set; }
        public string Ad { get; set; } = string.Empty;
        public string Soyad { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Sifre { get; set; } = string.Empty;
        public int RolID { get; set; }
        public bool Durum { get; set; } // true = aktif, false = pasif
        public DateTime KayitTarihi { get; internal set; }
    }
}
