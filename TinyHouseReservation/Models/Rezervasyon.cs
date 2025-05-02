namespace TinyHouseReservations.Models
{
    public class Rezervasyon
    {
        public int RezervasyonID { get; set; }
        public int EvID { get; set; }
        public int KiraciID { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public bool OdemeDurumu { get; set; }
        public string Durum { get; set; }
    }
}
