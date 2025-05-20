namespace TinyHouseReservations.Models
{
    public class EvDashboardViewModel
    {
        public int ToplamEv { get; set; }
        public int ToplamRezervasyon { get; set; }
        public int BekleyenRezervasyon { get; set; }
        public decimal ToplamGelir { get; set; }
        public List<string> SonYorumlar { get; set; } = new List<string>();
    }
}
