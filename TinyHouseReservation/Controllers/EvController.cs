using Microsoft.AspNetCore.Mvc;

namespace TinyHouseReservations.Controllers
{
    public class EvController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
