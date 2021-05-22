using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using ClosedXML.Excel;

namespace QLBanSach.Models
{
    public class GioHangExcel
    {
        public static int maHD{ get; set; }

        QLYBANSACHEntities db = new QLYBANSACHEntities();
        public int iMaSach { get; set; }
        public string iTenSach { get; set; }    
        
        public double donGia { get; set; }
        public int soLuong { get; set; }
        public double thanhTien { get { return soLuong * donGia; } }
        public string taiKhoan { get; set; }
        public string hoTen { get; set; }
        public GioHangExcel()
        {
            //iMaSach = maSach;
            //SACH sach = db.SACHes.Single(x => x.MaSach == iMaSach);
            //iTenSach = sach.TenSach;
            //iAnhBia = sach.AnhBia;
            //donGia = double.Parse(sach.GiaBan.ToString());
            //soLuong = 1;
        }
        public DateTime ngayDat;
    }
}