using Microsoft.AspNetCore.Mvc;
using TinyHouseReservations.DataAccess;
using TinyHouseReservations.Models;

namespace TinyHouseReservations.Controllers
{
    public class EvController : Controller
    {
        private readonly EvRepository _repo = new EvRepository();

        [HttpGet]
        public IActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Ekle(Ev ev)
        {
            // Ev Sahibi ID'sini session'dan al
            int? evSahibiID = HttpContext.Session.GetInt32("KullaniciID");
            if (evSahibiID == null)
                return RedirectToAction("Giris", "Kullanici");

            ev.EvSahibiID = evSahibiID.Value;
            _repo.Ekle(ev);
            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            int? evSahibiID = HttpContext.Session.GetInt32("KullaniciID");
            if (evSahibiID == null)
                return RedirectToAction("Giris", "Kullanici");

            var dashboard = _repo.GetDashboard(evSahibiID.Value);
            return View(dashboard);
        }
        public IActionResult Listele()
        {
            int? evSahibiID = HttpContext.Session.GetInt32("KullaniciID");
            if (evSahibiID == null)
                return RedirectToAction("Giris", "Kullanici");

            var evler = _repo.GetEvlerByEvSahibiID(evSahibiID.Value);
            return View(evler);
        }
        [HttpGet]
        public IActionResult Duzenle(int id)
        {
            var ev = _repo.GetById(id);
            if (ev == null || ev.EvSahibiID != HttpContext.Session.GetInt32("KullaniciID"))
            {
                return RedirectToAction("Listele");
            }

            return View(ev);
        }

        [HttpPost]
        public IActionResult Duzenle(Ev ev)
        {
            _repo.Guncelle(ev);
            return RedirectToAction("Listele");
        }
        public IActionResult DurumDegistir(int id, bool aktifMi)
        {
            _repo.DurumGuncelle(id, aktifMi);
            return RedirectToAction("Listele");
        }

        public IActionResult Sil(int id)
        {
            bool silindi = _repo.Sil(id);

            if (!silindi)
            {
                TempData["SilmeHatasi"] = "Bu evin rezervasyon kaydı olduğu için ilanı silemezsiniz!";
            }

            return RedirectToAction("Listele");
        }


    }
}