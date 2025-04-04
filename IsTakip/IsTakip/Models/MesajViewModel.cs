using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IsTakip.Models
{
    public class MesajViewModel
	{
        public int ID { get; set; }
        public string Header { get; set; }
        public string Context { get; set; }
        public string Sender { get; set; }
        public DateTime? SendTime { get; set; }
        public bool? IsOriginal { get; set; }
        public int? PersonelID { get; set; }
        public string PersonelAdi { get; set; }
    }
}