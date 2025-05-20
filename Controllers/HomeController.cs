using Microsoft.AspNetCore.Mvc;
using TinyHouseReservations.DataAccess;  // Kullanıcı verisi için repository
using TinyHouseReservations.Models;     // Kullanıcı modelini kullanabilmek için

namespace TinyHouseReservations.Controllers
{
    public class HomeController : Controller
    {
        private readonly KullaniciRepository _repo;

        public HomeController()
        {
            _repo = new KullaniciRepository(); // Repository'i başlatıyoruz
        }

        public IActionResult Index()
        {
            // Veritabanındaki tüm kullanıcıları alıyoruz
            var kullanicilar = _repo.GetAll();
            return View(kullanicilar); // Kullanıcı listesini view'a gönderiyoruz
        }

        // Login sayfasına yönlendiren metod
        public IActionResult Login()
        {
            return View();
        }

        // Login işlemini gerçekleştiren metod
        [HttpPost]
        public IActionResult Login(string email, string sifre)
        {
            // Girilen email ve şifreyi kontrol ediyoruz
            var kullanici = _repo.GirisYap(email, sifre);

            if (kullanici != null)
            {
                // Kullanıcı girişi başarılıysa, veriyi session'a kaydediyoruz
                HttpContext.Session.SetInt32("KullaniciID", kullanici.KullaniciID);
                return RedirectToAction("Index");
            }

            // Hata mesajı
            ViewBag.Hata = "Geçersiz e-posta veya şifre.";
            return View();
        }
    }
}