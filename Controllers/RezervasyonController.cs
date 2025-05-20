using Microsoft.AspNetCore.Mvc;
using TinyHouseReservations.DataAccess;
using TinyHouseReservations.Models;

namespace TinyHouseReservations.Controllers
{
    public class RezervasyonController : Controller
    {
        private readonly RezervasyonRepository _repo = new RezervasyonRepository();

        public IActionResult Index()
        {
            int? evSahibiID = HttpContext.Session.GetInt32("KullaniciID");
            if (evSahibiID == null)
                return RedirectToAction("Giris", "Kullanici");

            var rezervasyonlar = _repo.GetByEvSahibiID(evSahibiID.Value);
            return View(rezervasyonlar);
        }
        public IActionResult KabulEt(int id)
        {
            _repo.DurumGuncelle(id, "Onaylandi");
            return RedirectToAction("Index");
        }

        public IActionResult Reddet(int id)
        {
            _repo.DurumGuncelle(id, "Iptal");
            return RedirectToAction("Index");
        }
        public IActionResult Detay(int id)
        {
            var detay = _repo.GetDetayByID(id);
            if (detay == null)
                return RedirectToAction("Index");

            return View(detay);
        }

    }

}
