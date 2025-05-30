using Microsoft.AspNetCore.Mvc;
using TinyHouseReservations.DataAccess;

namespace TinyHouseReservations.Controllers
{
    public class EvController : Controller
    {
        private readonly EvRepository _evRepo = new EvRepository(); // ← bu satırı class içine ekle
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult IlanlariListele()
        {
            var evler = _evRepo.TumEvleriGetir();
            return View(evler);
        }

        public IActionResult Ilanlar()
        {
            var evler = _evRepo.TumEvleriGetir(); // ← EvRepository'deki metot zaten hazır
            return View(evler); // View'a listeyi gönderiyoruz
        }
    }
}
    

