using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using IsTakip.Models;

namespace IsTakip.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        DbIsTakipEntities1 db = new DbIsTakipEntities1();
        public ActionResult Index()
        {
            ViewBag.mesaj = null;
            return View();
        }
        [HttpPost]
        public ActionResult Index(string personelUsername, string personelPasswd)
        {
            var personel = (from p in db.TBL_PERSON where p.personelUsername == personelUsername && p.personelPasswd == personelPasswd select p).FirstOrDefault();

            if (personel != null)
            {
                Session["PersonelAdSoyad"] = personel.personelAdSoyad;
                Session["PersonelID"] = personel.ID;
                Session["PersonelBirimID"] = personel.personelBirimID;
                Session["PersonelYetkiID"] = personel.personelYetkiID;

                switch (personel.personelYetkiID)
                {
                    case 1:
                        return RedirectToAction("Index", "Yonetici");
                    case 2:
                        return RedirectToAction("Index", "Calisan");
                    default:
                        return View();
                }
            }
            else
            {
                ViewBag.mesaj = "Kullanıcı adı veya parola yanlış";
                return View();
            }
        }
    }
}