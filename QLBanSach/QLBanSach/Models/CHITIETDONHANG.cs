//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QLBanSach.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CHITIETDONHANG
    {
        public Nullable<int> SoLuong { get; set; }
        public Nullable<decimal> DonGia { get; set; }
        public int MaDH { get; set; }
        public int MaSach { get; set; }
    
        public virtual DONDATHANG DONDATHANG { get; set; }
    }
}
