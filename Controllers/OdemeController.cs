using Microsoft.AspNetCore.Mvc;
using TinyHouseReservations.DataAccess;
using TinyHouseReservations.Models;

namespace TinyHouseReservations.Controllers
{
    public class OdemeController : Controller
    {
        private readonly OdemeRepository _repo = new OdemeRepository();

        public IActionResult Index()
        {
            int? evSahibiID = HttpContext.Session.GetInt32("KullaniciID");
            if (evSahibiID == null)
                return RedirectToAction("Giris", "Kullanici");

            var odemeler = _repo.GetOdemelerByEvSahibiID(evSahibiID.Value);
            return View(odemeler);
        }
    }
}
