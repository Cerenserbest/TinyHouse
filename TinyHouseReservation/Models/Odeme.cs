namespace TinyHouseReservations.Models
{
    public class Odeme
    {
        public int OdemeID { get; set; }
        public int KullaniciID { get; set; }
        public decimal Tutar { get; set; }
        public DateTime Tarih { get; set; }
        public string? OdemeTipi { get; set; }
       
    }
}

