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
    
    public partial class TBL_MESAJLAR
    {
        public int ID { get; set; }
        public string Header { get; set; }
        public string Context { get; set; }
        public string Sender { get; set; }
        public Nullable<System.DateTime> SendTime { get; set; }
        public Nullable<bool> IsOriginal { get; set; }
        public Nullable<int> PersonelID { get; set; }
    
        public virtual TBL_PERSON TBL_PERSON { get; set; }
    }
}
