using Microsoft.AspNetCore.Mvc;
using TinyHouseReservations.Models;
using TinyHouseReservations.DataAccess;

namespace TinyHouseReservations.Controllers
{
    public class YorumController : Controller
    {
        private readonly YorumRepository _repo = new YorumRepository();

        public IActionResult Index()
        {
            int? evSahibiID = HttpContext.Session.GetInt32("KullaniciID");
            if (evSahibiID == null)
                return RedirectToAction("Giris", "Kullanici");

            var yorumlar = _repo.GetByEvSahibiID(evSahibiID.Value);
            return View(yorumlar);
        }
    }
}
