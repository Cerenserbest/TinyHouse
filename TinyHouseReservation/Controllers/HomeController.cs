using Microsoft.AspNetCore.Mvc;
using TinyHouseReservations.DataAccess;  // Kullanýcý verisi için repository
using TinyHouseReservations.Models;     // Kullanýcý modelini kullanabilmek için

namespace TinyHouseReservations.Controllers
{
    public class HomeController : Controller
    {
        private readonly KullaniciRepository _repo;

        public HomeController()
        {
            _repo = new KullaniciRepository(); // Repository'i baþlatýyoruz
        }

        public IActionResult Index()
        {
            // Veritabanýndaki tüm kullanýcýlarý alýyoruz
            var kullanicilar = _repo.GetAll();
            return View(kullanicilar); // Kullanýcý listesini view'a gönderiyoruz
        }

        // Login sayfasýna yönlendiren metod
        public IActionResult Login()
        {
            return View();
        }

        // Login iþlemini gerçekleþtiren metod
        [HttpPost]
        public IActionResult Login(string email, string sifre)
        {
            // Girilen email ve þifreyi kontrol ediyoruz
            var kullanici = _repo.GirisYap(email, sifre);

            if (kullanici != null)
            {
                // Kullanýcý giriþi baþarýlýysa, veriyi session'a kaydediyoruz
                HttpContext.Session.SetInt32("KullaniciID", kullanici.KullaniciID);
                return RedirectToAction("Index");
            }

            // Hata mesajý
            ViewBag.Hata = "Geçersiz e-posta veya þifre.";
            return View();
        }
    }
}
