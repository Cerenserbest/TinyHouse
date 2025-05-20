using Microsoft.AspNetCore.Mvc;
using TinyHouseReservations.DataAccess;
using TinyHouseReservations.Models;

namespace TinyHouseReservations.Controllers
{
    public class KullaniciController : Controller
    {
        private readonly KullaniciRepository _repo = new KullaniciRepository();

        // Kullanıcıları listelemek için
        public IActionResult Index()
        {
            var kullanicilar = _repo.GetAll();
            return View(kullanicilar);
        }

        // GET: Kullanici/Ekle
        [HttpGet]
        public IActionResult Ekle()
        {
            return View();
        }

        // ✅ GET: Kullanici/Giris – Formu ilk açtığında çalışır
        [HttpGet]
        public IActionResult Giris()
        {
            return View();
        }

        // ✅ POST: Kullanici/Giris – Form gönderilince çalışır
        [HttpPost]
        public IActionResult Giris(string email, string sifre)
        {
            var kullanici = _repo.GirisYap(email, sifre);

            if (kullanici != null)
            {
                HttpContext.Session.SetInt32("KullaniciID", kullanici.KullaniciID);
                HttpContext.Session.SetString("AdSoyad", kullanici.Ad + " " + kullanici.Soyad);
                HttpContext.Session.SetInt32("RolID", kullanici.RolID);

                switch (kullanici.RolID)
                {
                    case 1: return RedirectToAction("Index", "Admin");
                    case 2: return RedirectToAction("Index", "Ev");
                    case 3: return RedirectToAction("Index", "Rezervasyon");
                }
            }

            ViewBag.Hata = "E-posta veya şifre hatalı!";
            return View();
        }

        // POST: Kullanici/Ekle
        [HttpPost]
        public IActionResult Ekle(Kullanici k)
        {
            if (ModelState.IsValid)
            {
                _repo.Ekle(k);
                TempData["Mesaj"] = "Kayıt başarılı! Kullanıcı bilgilerini girerek giriş yapabilirsiniz.";
                return RedirectToAction("Index", "Home"); // Ana sayfaya yönlendir
            }
            return View(k);
        }

    }
}
