using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IsTakip.Models
{
	public class PDFViewModel
	{
        public int ID { get; set; }
        public String Urunler { get; set; }
        public int urunFiyat { get; set; }
        public int totalFiyat { get; set; }
        public string musteriAd { get; set; }
        public string musteriAdres { get; set; }
        public string musteriTel { get; set; }
        public DateTime tarih { get; set; }
        public int kdvFiyat { get; set; }
        public int kdv { get; set; }
    }
}