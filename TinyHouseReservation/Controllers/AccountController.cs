using Microsoft.AspNetCore.Mvc;
using TinyHouseReservations.DataAccess;
using TinyHouseReservations.Models;


namespace TinyHouseReservation.Controllers
{
    public class AccountController : Controller
    {
        private readonly DatabaseHelper _databaseHelper;

        public AccountController()
        {
            _databaseHelper = new DatabaseHelper();
        }
        // GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string sifre)
        {
           
            {
                var kullanici = _databaseHelper.GetKullaniciByEmailAndSifre(email, sifre);
                if (kullanici != null && kullanici.RolID == 1)
                {
                    // Admin başarılı giriş
                    return RedirectToAction("Index", "Admin");
                }

                ViewBag.Hata = "Geçersiz giriş!";
                return View();
            }
        }
    }
}

