using Microsoft.AspNetCore.Mvc;
using TinyHouseReservations.Models;
using TinyHouseReservations.DataAccess;

namespace TinyHouseReservations.Controllers
{
    public class GelirController : Controller
    {
        private readonly GelirRepository _repo = new GelirRepository();

        public IActionResult Index()
        {
            int? evSahibiID = HttpContext.Session.GetInt32("KullaniciID");
            if (evSahibiID == null)
                return RedirectToAction("Giris", "Kullanici");

            var gelirler = _repo.GetGelirlerByEvSahibiID(evSahibiID.Value);
            return View(gelirler);
        }
    }
}
