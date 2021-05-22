using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClosedXML.Excel;
using System.IO;

namespace QLBanSach.Models
{
    public class GioHang
    {
        QLYBANSACHEntities db = new QLYBANSACHEntities();
        public int iMaSach { get; set; }
        public string iTenSach { get; set; }
        public string iAnhBia { get; set; }
        public double donGia { get; set; }
        public int soLuong { get; set; }
        public double thanhTien { get { return soLuong * donGia; } }
        public GioHang(int maSach)
        {
            iMaSach = maSach;
            SACH sach = db.SACHes.Single(x => x.MaSach == iMaSach);
            iTenSach = sach.TenSach;
            iAnhBia = sach.AnhBia;
            donGia = double.Parse(sach.GiaBan.ToString());
            soLuong = 1;
        }
            
    }
}