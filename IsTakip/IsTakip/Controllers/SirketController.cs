using IsTakip.Models;
using IsTakip.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IsTakip.Controllers
{
    public class SirketController : Controller
    {
        DbIsTakipEntities1 db = new DbIsTakipEntities1();
        GenericRepository<TBL_PERSON> repo = new GenericRepository<TBL_PERSON>();
        public void UserSession()
        {
            string personelAd = Convert.ToString(Session["PersonelAdSoyad"?.ToString() ?? "Kullanıcı tanımlanmamış"]);
            int birimID = Convert.ToInt32(Session["PersonelBirimID"]);

            var birim = (from b in db.TBL_BIRIMLER where b.ID == birimID select b).FirstOrDefault();

            if (birim != null && birim.birimAd != null)
                ViewBag.birimAd = birim.birimAd;
            else
                ViewBag.birimAd = "Birim adı yok";

            ViewBag.personelName = personelAd;
        }

        public ActionResult Index()
        {
            UserSession();
            int personelYetkiID = Convert.ToInt32(Session["PersonelYetkiID"]);

            if (personelYetkiID != 1)
                return RedirectToAction("Index", "Login");

            return View();
        }

        //Personel işlemleri
        [HttpGet]
        public ActionResult PersonelListele()
        {
            UserSession();
            int personelYetkiID = Convert.ToInt32(Session["PersonelYetkiID"]);

            if (personelYetkiID != 1)
            {
                return RedirectToAction("Index", "Login");
            }

            var model = new PersonelViewModel
            {
                Yoneticiler = db.TBL_PERSON.Where(x => x.personelYetkiID == 1 && x.personelAktiflik == true).ToList(),
                Personeller = db.TBL_PERSON.Where(x => x.personelYetkiID == 2 && x.personelAktiflik == true).ToList()
            };

            return View(model);
        }


        [HttpPost]
        public ActionResult PersonelSil(int id)
        {
            int personelYetkiID = Convert.ToInt32(Session["PersonelYetkiID"]);

            if (personelYetkiID != 1)
            {
                return RedirectToAction("Index", "Login");
            }

            var personel = db.TBL_PERSON.FirstOrDefault(x => x.ID == id);

            if (personel != null)
            {
                personel.personelAktiflik = false;
                db.SaveChanges();
            }

            return RedirectToAction("PersonelListele");
        }

        [HttpGet]
        public ActionResult PersonelDuzenle(int id)
        {
            UserSession();

            int personelYetkiID = Convert.ToInt32(Session["PersonelYetkiID"]);

            if (personelYetkiID != 1)
            {
                return RedirectToAction("Index", "Login");
            }

            TBL_PERSON p = repo.Find(x => x.ID == id);
            UserSession();
            return View(p);
        }

        [HttpPost]
        public ActionResult PersonelDuzenle(TBL_PERSON p)
        {
            TBL_PERSON w = repo.Find(x => x.ID == p.ID);
            w.personelAdSoyad = p.personelAdSoyad;
            w.personelUsername = p.personelUsername;
            w.personelPasswd = p.personelPasswd;
            w.personelYetkiID = p.personelYetkiID;
            repo.TUpdate(w);

            return RedirectToAction("PersonelListele");
        }
        [HttpGet]
        public ActionResult PersonelEkle()
        {
            UserSession();
            int personelYetkiID = Convert.ToInt32(Session["PersonelYetkiID"]);

            if (personelYetkiID != 1)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        [HttpPost]
        public ActionResult PersonelEkle(string personelAdSoyad, string personelUsername, string personelPasswd, int? personelYetkiID, int personelBirimID)
        {
            int personelYetkiId = Convert.ToInt32(Session["PersonelYetkiID"]);

            if (personelYetkiId != 1)
            {
                return RedirectToAction("Index", "Login");
            }

            TBL_PERSON personel = new TBL_PERSON();

            if (!personelYetkiID.HasValue)
                personelYetkiID = 1;


            personel.personelAdSoyad = personelAdSoyad;
            personel.personelUsername = personelUsername;
            personel.personelPasswd = personelPasswd;
            personel.personelYetkiID = personelYetkiID.Value;
            personel.personelBirimID = personelBirimID;
            personel.personelAktiflik = true;

            db.TBL_PERSON.Add(personel);
            db.SaveChanges();

            return RedirectToAction("PersonelListele");
        }
        [HttpGet]
        public ActionResult MesajGonder()
        {
            UserSession();
            int personelYetkiID = Convert.ToInt32(Session["PersonelYetkiID"]);
            if (personelYetkiID != 1)
                return RedirectToAction("Index", "Login");

            int birimID = Convert.ToInt32(Session["PersonelBirimID"]);
            var calisanlar = (from p in db.TBL_PERSON where p.personelBirimID == birimID && p.personelYetkiID == 2 && p.personelAktiflik == true select p).ToList();

            ViewBag.personel = calisanlar;

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MesajGonder(TBL_MESAJLAR mesaj)
        {
            mesaj.Sender = Convert.ToString(Session["PersonelAdSoyad"]);

            mesaj.SendTime = DateTime.Now;
            if (mesaj.IsOriginal != true)
                mesaj.IsOriginal = true;

            if (int.TryParse(Request.Form["PersonelID"], out int userID))
            {
                if (userID == 0)
                    mesaj.PersonelID = null;
                else
                    mesaj.PersonelID = userID;
            }

            db.TBL_MESAJLAR.Add(mesaj);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult MesajListele()
        {
            UserSession();
            int personelYetkiID = Convert.ToInt32(Session["PersonelYetkiID"]);

            if (personelYetkiID != 1)
            {
                return RedirectToAction("Index", "Login");
            }

            var mesajlar = db.TBL_MESAJLAR
            .Join(db.TBL_PERSON,
                  mesaj => mesaj.PersonelID,
                  personel => personel.ID,
                  (mesaj, personel) => new MesajViewModel
                  {
                      ID = mesaj.ID,
                      Header = mesaj.Header,
                      Context = mesaj.Context,
                      Sender = mesaj.Sender,
                      SendTime = mesaj.SendTime,
                      IsOriginal = mesaj.IsOriginal,
                      PersonelID = mesaj.PersonelID,
                      PersonelAdi = personel.personelAdSoyad
                  })
            .ToList();

            return View(mesajlar);
        }
        public ActionResult MesajSil(int id)
        {
            int personelYetkiID = Convert.ToInt32(Session["PersonelYetkiID"]);

            if (personelYetkiID != 1)
            {
                return RedirectToAction("Index", "Login");
            }

            var mesaj = db.TBL_MESAJLAR.Find(id);
            if (mesaj != null)
            {
                db.TBL_MESAJLAR.Remove(mesaj);
                db.SaveChanges();
            }

            return RedirectToAction("MesajListele");
        }
        [HttpGet]
        public ActionResult MesajDuzenle(int id)
        {
            UserSession();
            int personelYetkiID = Convert.ToInt32(Session["PersonelYetkiID"]);

            if (personelYetkiID != 1)
            {
                return RedirectToAction("Index", "Login");
            }

            var mesaj = db.TBL_MESAJLAR.Find(id);
            return View(mesaj);
        }
        [HttpPost]
        public ActionResult MesajDuzenle(TBL_MESAJLAR m)
        {
            int personelYetkiID = Convert.ToInt32(Session["PersonelYetkiID"]);

            if (personelYetkiID != 1)
            {
                return RedirectToAction("Index", "Login");
            }

            TBL_MESAJLAR w = db.TBL_MESAJLAR.Where(x => x.ID == m.ID).Single();
            w.Header = m.Header;
            w.Context = m.Context;
            w.SendTime = DateTime.Now;
            w.IsOriginal = false;
            db.SaveChanges();

            return RedirectToAction("MesajListele");
        }
    }
}