using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLBanSach.Models
{
    public class ViewSanPham
    {
        public int maSach { get; set; }
        public string tenSach { get; set; }
        public string moTa { get; set; }
        public string anhBia { get; set; }
        public double giaBan { get; set; }
        public string tenNXB { get; set; }
    }
}