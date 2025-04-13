using IsTakip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IsTakip.MyMethodes
{
	public class GenericMethodes : Controller
	{
        DbIsTakipEntities1 db = new DbIsTakipEntities1();
        public void UserSession(HttpContextBase context)
        {
            string personelAd = Convert.ToString(context.Session["PersonelAdSoyad"?.ToString() ?? "Kullanıcı tanımlanmamış"]);
            int birimID = Convert.ToInt32(context.Session["PersonelBirimID"]);

            var birim = (from b in db.TBL_BIRIMLER where b.ID == birimID select b).FirstOrDefault();
            ViewBag.birimAd = birim.birimAd;
            ViewBag.personelName = personelAd;
        }
    }
}