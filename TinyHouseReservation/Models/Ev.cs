namespace TinyHouseReservations.Models
{
    public class Ev
    {
        public int EvID { get; set; }
        public int EvSahibiID { get; set; }
        public string Baslik { get; set; }
        public string Aciklama { get; set; }
        public string Konum { get; set; }
        public decimal Fiyat { get; set; }
        public bool Durum { get; set; }
        public string GorselYolu { get; set; }
    }
}
