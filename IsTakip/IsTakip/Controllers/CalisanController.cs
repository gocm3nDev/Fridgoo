using IsTakip.Models;
using IsTakip.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IsTakip.Controllers
{
    public class CalisanController : Controller
    {
        DbIsTakipEntities1 db = new DbIsTakipEntities1();
        GenericRepository<TBL_ISLER> repo = new GenericRepository<TBL_ISLER>();
        public void UserSession()
        {
            string personelAd = Convert.ToString(Session["PersonelAdSoyad"?.ToString() ?? "Kullanıcı tanımlanmamış"]);
            int birimID = Convert.ToInt32(Session["PersonelBirimID"]);

            var birim = (from b in db.TBL_BIRIMLER where b.ID == birimID select b).FirstOrDefault();

            if (birim != null || birim.birimAd != null)
                ViewBag.birimAd = birim.birimAd;
            else
                ViewBag.birimAd = "Birim adı yok";

            ViewBag.personelName = personelAd;
        }

        public ActionResult Index()
        {
            UserSession();
            int yetkiTurID = Convert.ToInt32(Session["YetkiTurID"]);

            if (yetkiTurID <= 3 && yetkiTurID >= 1)
                return RedirectToAction("Index", "Login");

            return View();
        }

        [HttpGet]
        public ActionResult Isler()
        {
            UserSession();
            int yetkiTurID = Convert.ToInt32(Session["YetkiTurID"]);

            if (yetkiTurID <= 3 && yetkiTurID >= 1)
                return RedirectToAction("Index", "Login");

            int secilen = (int)Session["PersonelID"];
            var isler = (from j in db.TBL_ISLER where j.isPersonelID == secilen && j.isDurumID == 4 select j).OrderByDescending(j => j.iletilenTarih).ToList();
            
            ViewBag.isler = isler;

            return View();
        }

        [HttpGet]
        public ActionResult Bitir(int id)
        {
            UserSession();
            int yetkiTurID = Convert.ToInt32(Session["YetkiTurID"]);

            if (yetkiTurID <= 3 && yetkiTurID >= 1)
                return RedirectToAction("Index", "Login");

            TBL_ISLER Is = db.TBL_ISLER.Find(id);

            return View(Is);
        }
        [HttpPost]
        public ActionResult Bitir(TBL_ISLER f)
        {
            UserSession();
            int yetkiTurID = Convert.ToInt32(Session["YetkiTurID"]);

            if (yetkiTurID <= 3 && yetkiTurID >= 1)
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

            return RedirectToAction("Isler", "Calisan");
        }

        [HttpPost]
        public ActionResult Ertele(int id , TBL_ISLER t)
        {
            TBL_ISLER w = db.TBL_ISLER.Where(x => x.ID == t.ID).FirstOrDefault();
            w.isDurumID = 3;
            db.SaveChanges();

            return RedirectToAction("Isler", "Calisan");
        }
        [HttpGet]
        public ActionResult Ertelenmisler()
        {
            UserSession();

            int secilen = (int)Session["PersonelID"];
            var isler = db.TBL_ISLER.Where(x => x.isPersonelID == secilen && x.isDurumID == 3).ToList();

            return View(isler);
        }

        [HttpGet]
        public ActionResult Mesajlar()
        {
            UserSession();

            int secilen = (int)Session["PersonelID"];
            var mesajlar = db.TBL_MESAJLAR.Where(x => x.PersonelID == secilen || x.PersonelID == null).ToList();

            return View(mesajlar);
        }

        [HttpPost]
        public ActionResult MesajSil(int id)
        {
            var mesaj = db.TBL_MESAJLAR.Where(x => x.ID == id).FirstOrDefault();
            db.TBL_MESAJLAR.Remove(mesaj);
            db.SaveChanges();

            return RedirectToAction("Mesajlar", "Calisan");
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