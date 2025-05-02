using Microsoft.AspNetCore.Mvc;
using TinyHouseReservations.DataAccess;

namespace TinyHouseReservations.Controllers
{
    public class AdminController : Controller
    {
        private readonly AdminRepository _adminRepo = new AdminRepository();

        public IActionResult Index()
        {
            var kullanicilar = _adminRepo.TumKullanicilariGetir();
            return View(kullanicilar); // Bu View listeyi gösterecek
        }
    }
}

