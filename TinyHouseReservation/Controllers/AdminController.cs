
using Microsoft.AspNetCore.Mvc;
using TinyHouseReservations.Models;
using Microsoft.Data.SqlClient;
using System.Linq;
using TinyHouseReservations.DataAccess;



namespace TinyHouseReservations.Controllers
{
    public class AdminController : Controller
    {
        private readonly AdminRepository _adminRepo;
        private readonly EvRepository _evRepo;
        private readonly RezervasyonRepository _rezervasyonRepo = new RezervasyonRepository();
        private readonly OdemeRepository _odemeRepo = new OdemeRepository();
        private object _context;

        public AdminController()
        {
            _adminRepo = new AdminRepository();
            _evRepo = new EvRepository();
            _rezervasyonRepo = new RezervasyonRepository(); // ← burada nesne oluştur
        }

        public IActionResult Dashboard()
        {
            var kullanicilar = _adminRepo.TumKullanicilariGetir();
            ViewBag.Toplam = kullanicilar.Count;
            ViewBag.Aktif = kullanicilar.Count(k => k.Durum);
            ViewBag.Pasif = kullanicilar.Count(k => !k.Durum);
            ViewBag.Admin = kullanicilar.Count(k => k.RolID == 1);// Ev verileri
            var evRepo = new EvRepository();
            var evler = evRepo.TumEvleriGetir();
            ViewBag.ToplamEv = evler.Count;
            ViewBag.AktifEv = evler.Count(e => e.Durum);
            ViewBag.PasifEv = evler.Count(e => !e.Durum);

            return View();
        }
        public IActionResult Ilanlar()
        {
            var evler = _evRepo.TumEvleriGetir();
            return View("Ilanlar", evler);
        }


        public IActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }

        public IActionResult KullaniciEkle()
        {
            return View("KullaniciEkle"); // Kullanıcı ekleme sayfası
        }
        // Kullanıcı ekleme işlemini yapacak POST action'ı
        [HttpPost]
        public IActionResult KullaniciEkle(Kullanici yeniKullanici)
        {
            if (ModelState.IsValid)
            {
                // Yeni kullanıcıyı veritabanına ekleyelim
                _adminRepo.KullaniciEkle(yeniKullanici);

                // Kullanıcı eklendikten sonra tekrar listeyi göster
                return RedirectToAction("Index"); // Kullanıcılar listesine yönlendir
            }

            return View(yeniKullanici); // Hata durumunda formu tekrar göster
        }
        public IActionResult KullaniciDuzenle(int id)
{
    var kullanici = _adminRepo.KullaniciGetirById(id);
    if (kullanici == null)
    {
        return NotFound();
    }
    return View(kullanici);
}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult KullaniciDuzenle(Kullanici model)
        {
            if (ModelState.IsValid)
            {
                var kullaniciDb = _adminRepo.KullaniciGetirById(model.KullaniciID);
                if (kullaniciDb == null)
                {
                    return NotFound();
                }

                kullaniciDb.Ad = model.Ad;
                kullaniciDb.Soyad = model.Soyad;
                kullaniciDb.Email = model.Email;
                kullaniciDb.Sifre = model.Sifre;
                kullaniciDb.RolID = model.RolID;
                kullaniciDb.Durum = model.Durum;

                try
                {
                    _adminRepo.KullaniciGuncelle(kullaniciDb);
                    return RedirectToAction("KullanicilariListele");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Güncelleme sırasında hata oluştu: " + ex.Message);
                }
            }

            // Eğer ModelState geçerli değilse veya hata varsa sayfayı tekrar göster ve hata mesajlarını göster
            return View(model);
        }

        [HttpPost]
        [HttpPost]
        public IActionResult RezervasyonIptal(int id)
        {
            _rezervasyonRepo.RezervasyonIptal(id);
            return RedirectToAction("Rezervasyonlar");
        }


        public IActionResult KullaniciSil(int id)
        {
            _adminRepo.KullaniciSil(id);
            return RedirectToAction("Index");
        }
        
        public IActionResult KullanicilariListele(string? durum, string? rol, string? arama)
        {
            var kullanicilar = _adminRepo.TumKullanicilariGetir();

            if (!string.IsNullOrEmpty(durum))
            {
                if (durum == "aktif")
                    kullanicilar = kullanicilar.Where(k => k.Durum == true).ToList();
                else if (durum == "pasif")
                    kullanicilar = kullanicilar.Where(k => k.Durum == false).ToList();
            }

            if (!string.IsNullOrEmpty(rol) && rol == "admin")
            {
                kullanicilar = kullanicilar.Where(k => k.RolID == 1).ToList();
            }

            if (!string.IsNullOrEmpty(arama))
            {
                string aramaLower = arama.ToLower();
                kullanicilar = kullanicilar.Where(k =>
                    (k.Ad + " " + k.Soyad).ToLower().Contains(aramaLower) ||
                    k.Email.ToLower().Contains(aramaLower)
                ).ToList();
            }

            return View("KullanicilariListele", kullanicilar);
        }
        public IActionResult Rezervasyonlar(string? durum)
        {
            var rezervasyonlar = _rezervasyonRepo.TumRezervasyonlariGetir();

            if (durum == "aktif")
                rezervasyonlar = rezervasyonlar.Where(r => r.Durum).ToList();
            else if (durum == "iptal")
                rezervasyonlar = rezervasyonlar.Where(r => !r.Durum).ToList();

            return View("RezervasyonlariListele", rezervasyonlar);
        }



        public IActionResult EvleriListele(string? durum)
        {
            var evler = _evRepo.TumEvleriGetir();
            return View("EvleriListele", evler);
        }

        public IActionResult RezervasyonlariListele()
        {
            var rezervasyonRepo = new RezervasyonRepository();

            return View("RezervasyonlariListele", rezervasyonRepo.TumRezervasyonlariGetir());
        }
        [HttpGet]
        public IActionResult RezervasyonDetay(int id)
        {
            var rezervasyon = _rezervasyonRepo.RezervasyonGetirById(id);
            if (rezervasyon == null)
            {
                return NotFound();
            }
            return View(rezervasyon);
        }
        // GET: Admin/EvDuzenle/5
      

        // POST: Admin/EvDuzenle/5
       [HttpPost]
public IActionResult EvDuzenle(Ev guncellenenEv)
{
    if (!ModelState.IsValid)
    {
                // ModelState hatalarını logla veya debugla
                var errors = ModelState.Values
              .SelectMany(v => v.Errors)
              .Select(e => e.ErrorMessage)
              .ToList();

                ViewBag.ModelErrors = errors;

                return View(guncellenenEv);

            }

            _evRepo.EvGuncelle(guncellenenEv);
    return RedirectToAction("Ilanlar");
}
        [HttpGet]
        public IActionResult EvDuzenle(int id)
        {
            var ev = _evRepo.EvGetirById(id);
            if (ev == null)
            {
                return NotFound();
            }
            return View(ev); // View'e ev modelini gönderiyoruz
        }
        public IActionResult EvSil(int id)
        {
            _evRepo.EvSil(id); // Bu satır EvRepository.cs içinde EvSil metodunu çağırır
            return RedirectToAction("Ilanlar"); // Silme işleminden sonra ilanlar listesine dön
        }

     public IActionResult Odemeler()
{
    var odemeler = _odemeRepo.TumOdemeleriGetir();

    Console.WriteLine("🔍 TOPLAM ÖDEME: " + odemeler.Count);

    foreach (var o in odemeler)
    {
        Console.WriteLine($"🧾 ID: {o.OdemeID}, Kullanıcı: {o.KullaniciID}, Tutar: {o.Tutar}, Tarih: {o.Tarih}, Tip: {o.OdemeTipi}");
    }

    return View("Odemeler", odemeler);
}
        public IActionResult Istatistik()
        {
            var kullanicilar = _adminRepo.TumKullanicilariGetir();
            var rezervasyonlar = _rezervasyonRepo.TumRezervasyonlariGetir();
            var odemeler = _odemeRepo.TumOdemeleriGetir();

            var model = new
            {
                ToplamKullanici = kullanicilar.Count,
                AktifKullanici = kullanicilar.Count(k => k.Durum),
                YeniKullanici = kullanicilar.Count(k => k.KayitTarihi >= DateTime.Now.AddDays(-7)),
                ToplamRezervasyon = rezervasyonlar.Count,
                BugunRezervasyon = rezervasyonlar.Count(r => r.BaslangicTarihi.Date == DateTime.Now.Date),
                ToplamOdeme = odemeler.Count,
                ToplamTutar = odemeler.Sum(o => o.Tutar)
            };

            return View("Istatistik", model);
        }
        public JsonResult AylikGrafikVerileri()
        {
            var rezervasyonlar = _adminRepo.AylikRezervasyonSayisi();
            var odemeler = _adminRepo.AylikOdemeToplami();

            var result = new
            {
                aylar = rezervasyonlar.Select(x => x.Ay).ToList(),
                rezervasyonSayilari = rezervasyonlar.Select(x => x.RezervasyonSayisi).ToList(),
                odemeToplamlari = odemeler.Select(x => x.Tutar).ToList()
            };

            return Json(result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult KullaniciDurumDegistir(int id)
        {
            var kullanici = _adminRepo.KullaniciGetirById(id);
            if (kullanici != null)
            {
                kullanici.Durum = !kullanici.Durum; // aktifse pasif, pasifse aktif yap
                _adminRepo.KullaniciGuncelle(kullanici);
            }
            return RedirectToAction("KullanicilariListele");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult KullaniciDurumGuncelle(int id)
        {
          

            var kullanici = _adminRepo.KullaniciGetirById(id);
            if (kullanici == null)
            {
                TempData["Hata"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("KullanicilariListele");
            }

            kullanici.Durum = !kullanici.Durum;
            _adminRepo.KullaniciGuncelle(kullanici);

            TempData["Mesaj"] = "Kullanıcı durumu güncellendi.";
            return RedirectToAction("KullanicilariListele");
        }













    }
}
