using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLBanSach.Models;

namespace QLBanSach.Controllers
{
    public class KhachHangController : Controller
    {
        QLYBANSACHEntities db = new QLYBANSACHEntities();
        // GET: KhachHang
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(ModelKhachHang kh)
        {
            var check = db.KHACHHANGs.Where(x => x.TaiKhoan == kh.username).SingleOrDefault();
            if (check == null)
            {
                try
                {
                    KHACHHANG khach = new KHACHHANG();
                    khach.HoTen = kh.tenKH;
                    khach.TaiKhoan = kh.username;
                    khach.MatKHAU = kh.password;
                    khach.DiaChiKH = kh.diaChi;
                    khach.DienThoaiKH = kh.sdt;
                    khach.NgaySinh = kh.ngaySinh;
                    khach.Email = kh.email;
                    db.KHACHHANGs.Add(khach);
                    db.SaveChanges();
                    ViewBag.thongbao = "Đăng ký thành công";
                }
                catch (Exception ex)
                {

                    ViewBag.ex = ex;
                }
               
            }
            else
            {
                ViewBag.error = "Tên đăng nhập đã tồn tại";
            }
            
            return View();
        }
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            string username = f["TaiKhoan"];
            string password = f["MatKhau"];
            if(string.IsNullOrEmpty(username))
            {
                ViewData["Loi1"] = "Tên đăng nhập không được bỏ trống";
            }
            else if(string.IsNullOrEmpty(password))
            {
                ViewData["Loi2"] = "Mật khẩu không được bỏ trống";
            }
            else
            {
                KHACHHANG kh = db.KHACHHANGs.Where(x => x.TaiKhoan == username && x.MatKHAU == password).SingleOrDefault();
                if(kh!=null)
                {
                    ViewBag.ThongBao = "Chào mừng " + kh.HoTen;
                    Session["taiKhoan"] = kh;
                    Session["ten"] = kh.HoTen;
                    return RedirectToAction("SachMoi", "BookStore");
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();                
        }
        public ActionResult DangXuat()
        {
            Session.Clear();
            return View();
        }


    }
}