//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IsTakip.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TBL_ISLER
    {
        public int ID { get; set; }
        public string musteriAdSoyad { get; set; }
        public string isinAciklamasi { get; set; }
        public string isinBasligi { get; set; }
        public string musteriAdresi { get; set; }
        public string kullanilanMalzeme { get; set; }
        public Nullable<int> malzemeFiyat { get; set; }
        public int kdvOran { get; set; }
        public Nullable<int> totalFiyat { get; set; }
        public System.DateTime iletilenTarih { get; set; }
        public Nullable<System.DateTime> yapilanTarih { get; set; }
        public int isDurumID { get; set; }
        public int isPersonelID { get; set; }
        public string musteriTel { get; set; }
    
        public virtual TBL_PERSON TBL_PERSON { get; set; }
    }
}
