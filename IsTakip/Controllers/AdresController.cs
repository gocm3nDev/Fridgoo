using IsTakip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IsTakip.Controllers
{
    public class AdresController : Controller
    {
        DbIsTakipEntities1 db = new DbIsTakipEntities1();
        // GET: Adres
        [HttpPost]
        public JsonResult Kaydet(TBL_KONUM konum)
        {
            try
            {
                using (db)
                {
                    var yeniAdres = new TBL_KONUM
                    {
                        Konum = konum.Konum
                    };

                    if (yeniAdres != null)
                    {
                        db.TBL_KONUM.Add(yeniAdres);
                        db.SaveChanges();
                    }
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}