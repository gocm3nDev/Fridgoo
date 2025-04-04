using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using IsTakip.Models;
using IsTakip.Repositories;

namespace IsTakip.Controllers
{
    public class YoneticiController : Controller
    {
        DbIsTakipEntities1 entity = new DbIsTakipEntities1();
        GenericRepository<TBL_ISLER> repo = new GenericRepository<TBL_ISLER>();
        // GET: Yonetici
        public int AvgTime()
        {
            DbIsTakipEntities1 db = new DbIsTakipEntities1();

            var totalDuration = 0;
            var totalIs = db.TBL_ISLER.Count();

            if (totalIs > 0)
            {
                foreach (var x in db.TBL_ISLER)
                {
                    if (x.yapilanTarih != null && x.iletilenTarih != null)
                    {
                        TimeSpan duration = (TimeSpan)(x.yapilanTarih - x.iletilenTarih);
                        totalDuration += (int)duration.TotalHours;
                    }
                }
                totalDuration = totalDuration / totalIs;
            }

            if (totalDuration > 0)
            {
                return totalDuration;
            }
            return 0;
        }

        public void UserSession()
        {
            string personelAd = Convert.ToString(Session["PersonelAdSoyad"?.ToString() ?? "Kullanıcı tanımlanmamış"]);
            int birimID = Convert.ToInt32(Session["PersonelBirimID"]);

            var birim = (from b in entity.TBL_BIRIMLER where b.ID == birimID select b).FirstOrDefault();

            if (birim != null && birim.birimAd != null)
                ViewBag.birimAd = birim.birimAd;
            else
                ViewBag.birimAd = "Birim adı yok";

            ViewBag.personelName = personelAd;
        }

        public RedirectToRouteResult Permission()
        {
            

            return null;
        }

        public ActionResult Index()
        {
            UserSession();//Kullanıcı bilgilerini çekme
            int personelYetkiID = Convert.ToInt32(Session["PersonelYetkiID"]);

            if (personelYetkiID != 1)
                return RedirectToAction("Index", "Login");

            //Index sayfası son 3 iş listeleme
            var query = (from item in entity.TBL_ISLER
                         where item.isDurumID == 1
                         orderby item.ID descending
                         select new MusteriViewModel
                         {
                             MusteriAdSoyad = item.musteriAdSoyad,
                             MusteriAdres = item.musteriAdresi,
                             MusteriTel = item.musteriTel,
                             PersonelAdSoyad = item.TBL_PERSON.personelAdSoyad
                         }).Take(3).ToList();

            //Index sayfası istatistik verilerini çekme ve işleme
            var toplamCiro = entity.TBL_ISLER.Where(x => x.isDurumID == 1 && x.totalFiyat != null).Sum(x => (int)x.totalFiyat);
            var aktifSayi = entity.TBL_ISLER.Where(x => x.isDurumID == 4).Count();
            var ertelenmisSayi = entity.TBL_ISLER.Where(x => x.isDurumID == 3).Count();
            var isciSayisi = entity.TBL_PERSON.Where(x => x.personelAktiflik == true).Count();
            var gecmisSayisi = entity.TBL_ISLER.Where(x => x.isDurumID == 1).Count();

            ViewBag.ortSure = AvgTime();
            ViewBag.aktifSayi = aktifSayi;
            ViewBag.toplamCiro = toplamCiro;
            ViewBag.ertelenmisSayi = ertelenmisSayi;
            ViewBag.isciSayisi = isciSayisi;
            ViewBag.gecmisSayisi = gecmisSayisi;

            return View(query);
        }

        //İş atama
        [HttpGet]
        public ActionResult Ata()
        {
            UserSession();
            int personelYetkiID = Convert.ToInt32(Session["PersonelYetkiID"]);

            if (personelYetkiID != 1)
                return RedirectToAction("Index", "Login");

            int birimID = Convert.ToInt32(Session["PersonelBirimID"]);

            var calisanlar = (from p in entity.TBL_PERSON where p.personelBirimID == birimID && p.personelYetkiID == 2 && p.personelAktiflik == true select p).ToList();

            ViewBag.personel = calisanlar;

            var birim = (from b in entity.TBL_BIRIMLER where b.ID == birimID select b).FirstOrDefault();

            ViewBag.birim = birim.birimAd;

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Ata(string musteriAdSoyad, string isinBasligi, string isinAciklamasi, string musteriAdresi, string musteriTel, int selectPer)
        {
            UserSession();
            int personelYetkiID = Convert.ToInt32(Session["PersonelYetkiID"]);

            if (personelYetkiID != 1)
                return RedirectToAction("Index", "Login");

            TBL_ISLER yeniIs = new TBL_ISLER();
            yeniIs.musteriAdSoyad = musteriAdSoyad;
            yeniIs.isinBasligi = isinBasligi;
            yeniIs.isinAciklamasi = isinAciklamasi;
            yeniIs.musteriAdresi = musteriAdresi;
            yeniIs.musteriTel = musteriTel;
            yeniIs.isPersonelID = selectPer;
            yeniIs.iletilenTarih = DateTime.Now;
            yeniIs.isDurumID = 4;

            entity.TBL_ISLER.Add(yeniIs);
            entity.SaveChanges();

            return View();
        }

        [HttpGet]
        public ActionResult Takip()
        {
            UserSession();
            int personelYetkiID = Convert.ToInt32(Session["PersonelYetkiID"]);

            if (personelYetkiID != 1)
                return RedirectToAction("Index", "Login");

            int birimID = Convert.ToInt32(Session["PersonelBirimID"]);

            var calisanlar = (from p in entity.TBL_PERSON where p.personelBirimID == birimID && p.personelYetkiID == 2 && p.personelAktiflik == true select p).ToList();

            ViewBag.personel = calisanlar;

            var birim = (from b in entity.TBL_BIRIMLER where b.ID == birimID select b).FirstOrDefault();

            ViewBag.birim = birim.birimAd;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Takip(int selectPer)
        {
            UserSession();
            int personelYetkiID = Convert.ToInt32(Session["PersonelYetkiID"]);

            if (personelYetkiID != 1)
                return RedirectToAction("Index", "Login");

            var secilenPersonel = (from p in entity.TBL_PERSON where p.ID == selectPer && p.personelAktiflik == true select p).FirstOrDefault();

            if (secilenPersonel == null)
            {
                return Content("Seçilen personel bulunamadı");
            }

            Session["secilen"] = secilenPersonel;

            return RedirectToAction("Listele", "Yonetici");
        }

        [HttpGet]
        public ActionResult Listele()
        {
            UserSession();
            int personelYetkiID = Convert.ToInt32(Session["PersonelYetkiID"]);

            if (personelYetkiID != 1)
                return RedirectToAction("Index", "Login");

            TBL_PERSON secilenPersonel = (TBL_PERSON)Session["secilen"];
            var isler = (from j in entity.TBL_ISLER where j.isPersonelID == secilenPersonel.ID orderby j.iletilenTarih descending select j).ToList();

            ViewBag.isler = isler;
            ViewBag.personel = secilenPersonel;

            return View();
        }

        [HttpGet]
        public ActionResult IsiBitir(int id)
        {
            UserSession();
            int personelYetkiID = Convert.ToInt32(Session["PersonelYetkiID"]);

            if (personelYetkiID != 1)
                return RedirectToAction("Index", "Login");

            TBL_ISLER Is = repo.Find(x => x.ID == id);
            return View(Is);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IsiBitir(TBL_ISLER f)
        {
            UserSession();
            int personelYetkiID = Convert.ToInt32(Session["PersonelYetkiID"]);

            if (personelYetkiID != 1)
                return RedirectToAction("Index", "Login");

            TBL_ISLER w = repo.Find(x => x.ID == f.ID);
            w.musteriAdSoyad = f.musteriAdSoyad;
            w.isinBasligi = f.isinBasligi;
            w.isinAciklamasi = f.isinAciklamasi;
            w.musteriAdresi = f.musteriAdresi;
            w.musteriTel = f.musteriTel;
            w.isDurumID = 1;
            w.kullanilanMalzeme = f.kullanilanMalzeme;
            w.malzemeFiyat = f.malzemeFiyat;
            w.totalFiyat = f.totalFiyat;
            w.yapilanTarih = DateTime.Now;
            repo.TUpdate(w);

            return RedirectToAction("Listele", "Yonetici");
        }

        public ActionResult Gecmis(string filter, string filterRadio)
        {
            UserSession();
            int personelYetkiID = Convert.ToInt32(Session["PersonelYetkiID"]);

            if (personelYetkiID != 1)
                return RedirectToAction("Index", "Login");

            if (filterRadio == "tele")
            {
                var gecmis = entity.TBL_ISLER.Where(x => string.IsNullOrEmpty(filter) | x.musteriTel.Contains(filter)).ToList();
                return View(gecmis);
            }
            else if (filterRadio == "adrese")
            {
                var gecmis = entity.TBL_ISLER.Where(x => string.IsNullOrEmpty(filter) | x.musteriAdresi.Contains(filter)).ToList();
                return View(gecmis);
            }
            else
            {
                var gecmis = entity.TBL_ISLER.Where(x => string.IsNullOrEmpty(filter) | x.musteriAdSoyad.Contains(filter)).ToList();
                return View(gecmis);
            }
        }

        [HttpPost]
        public ActionResult Sil(int id)
        {
            UserSession();
            int personelYetkiID = Convert.ToInt32(Session["PersonelYetkiID"]);

            if (personelYetkiID != 1)
                return RedirectToAction("Index", "Login");

            var sil = entity.TBL_ISLER.Find(id);

            if (sil != null)
            {
                entity.TBL_ISLER.Remove(sil);
                entity.SaveChanges();
            }

            return RedirectToAction("Gecmis");
        }
        [HttpGet]
        public ActionResult Ertele(int id)
        {
            UserSession();
            int personelYetkiID = Convert.ToInt32(Session["PersonelYetkiID"]);

            if (personelYetkiID != 1)
                return RedirectToAction("Index", "Login");

            return Ertele(new TBL_ISLER { ID = id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Ertele(TBL_ISLER f)
        {
            UserSession();
            int personelYetkiID = Convert.ToInt32(Session["PersonelYetkiID"]);

            if (personelYetkiID != 1)
                return RedirectToAction("Index", "Login");

            TBL_ISLER w = repo.Find(x => x.ID == f.ID);
            w.isDurumID = 3;
            repo.TUpdate(w);

            return RedirectToAction("ErtelenmisIsler");
        }
        [HttpGet]
        public ActionResult ErtelenmisIsler()
        {
            UserSession();
            int personelYetkiID = Convert.ToInt32(Session["PersonelYetkiID"]);

            if (personelYetkiID != 1)
                return RedirectToAction("Index", "Login");

            var ertelenmis = entity.TBL_ISLER.Where(x => x.isDurumID == 3).ToList();
            return View(ertelenmis);
        }
        public ActionResult CikisYap()
        {
            Session.Clear();
            Session.Abandon();

            Response.Cache.SetExpires(System.DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            return RedirectToAction("Index", "Login");
        }
    }
}