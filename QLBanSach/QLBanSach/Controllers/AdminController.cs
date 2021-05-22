using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLBanSach.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace QLBanSach.Controllers
{
    public class AdminController : Controller
    {
        QLYBANSACHEntities db = new QLYBANSACHEntities();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            string username = f["username"].ToString();
            string password = f["password"].ToString();
            if(string.IsNullOrEmpty(username))
            {
                ViewBag.Loi1 = "Tên đăng nhập không được bỏ trống";
            }
            else if(string.IsNullOrEmpty(password))
            {
                ViewBag.Loi2 = "Mật khẩu không được để trống";
            }
            else
            {
                Admin ad = db.Admins.Where(x => x.userAdmin == username && x.passAdmin == password).SingleOrDefault();
                if(ad!=null)
                {
                    Session["Admin"] = ad;
                    return RedirectToAction("TrangChu");
                }
            }
            ViewBag.ThongBao = "Tài khoản hoặc mật khẩu không đúng";

            return View();
        }
        public ActionResult Sach(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;

            return View(db.SACHes.ToList().OrderBy(n=>n.MaSach).ToPagedList(pageNumber,pageSize));
        }
        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
          
            return View();
        }
        [HttpPost]
        public ActionResult Add(SACH sach, HttpPostedFileBase fileUp)
        {
            if (ModelState.IsValid)
            {
                // Luu ten file
                var fileName = Path.GetFileName(fileUp.FileName);
                // Luu duong dan
                var path = Path.Combine(Server.MapPath("~/Assit/Images/Images"), fileName);
                // kiem tra hinh anh da ton tai chua
                if (System.IO.File.Exists(path))
                {
                    ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                }
                else { fileUp.SaveAs(path); }
                ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
                ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
               
                SACH a = new SACH() { AnhBia = fileUp.FileName };
                sach.AnhBia = fileUp.FileName;
                
                db.SACHes.Add(sach);
               
                db.SaveChanges();
              
            }
            return RedirectToAction("AddTacGia", "Admin");
        }
        [HttpGet]
        public ActionResult AddTacGia()
        {
            ViewBag.MaTG = new SelectList(db.TACGIAs.ToList().OrderBy(n => n.TenTG), "MaTG", "TenTG");
            return View();
        }
        [HttpPost]
        public ActionResult AddTacGia(VIETSACH viet)
        {

            if(ModelState.IsValid)
            {
                VIETSACH vietsach = new VIETSACH();
                vietsach.MaTG = viet.MaTG;
                vietsach.MaSach = (db.SACHes.Select(x => x.MaSach).DefaultIfEmpty(0).Max());
                vietsach.VaiTro = viet.VaiTro;
                vietsach.ViTri = viet.ViTri;              
                ViewBag.MaTG = new SelectList(db.TACGIAs.ToList().OrderBy(n => n.TenTG), "MaTG", "TenTG");
                db.VIETSACHes.Add(vietsach);
                db.SaveChanges();
            }
            return View();
        }
        [HttpGet]
        public ActionResult AddnewTG()
        {
            return View();
        }
       [HttpPost]
       public ActionResult AddnewTG(TACGIA tg)
        {
            TACGIA tacgia = new TACGIA();
            tacgia.TenTG = tg.TenTG;
            tacgia.TieuSu = tg.TieuSu;
            tacgia.DiaChi = tg.DiaChi;
            tacgia.DienThoai = tg.DienThoai;
            db.TACGIAs.Add(tacgia);
            db.SaveChanges();
            return RedirectToAction("AddTacGia", "Admin");
        }





        [HttpGet]
        public ActionResult Edit(int maSach)
        {
            SACH sach = db.SACHes.Where(n => n.MaSach == maSach).FirstOrDefault();

            return View(sach);
        }
        [HttpPost]
        public ActionResult Edit(SACH sach)
        {
            var sanpham = db.SACHes .Where(c => c.MaSach == sach.MaSach).FirstOrDefault();
            sanpham.TenSach = sach.TenSach;
            sanpham.GiaBan = sach.GiaBan;
            db.SaveChanges();
            ViewBag.TB = "Chỉnh sửa thành công";
            return View();
        }

        [HttpGet]
        public ActionResult Delete(int? maSach)
        {
            SACH sp = db.SACHes.Where(c => c.MaSach == maSach).SingleOrDefault();
            return View(sp);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int maSach)
        {
            try
            {
                var sanpham = db.SACHes.Find(maSach);
                db.SACHes.Remove(sanpham);
                db.SaveChanges();
                return RedirectToAction("Sach", "Admin");
            }
            catch (Exception)
            {
                ViewBag.loi = "Không thể xóa sản phẩm đã tồn tại trong hóa đơn";
                return View();
            }
           

        }
        public ActionResult QuanLyDonHang()
        {
            return View(db.DONDATHANGs.ToList());
        }
        public ActionResult Details(int id)
        {
            ModelTemp.madh = id;
            return View(db.CHITIETDONHANGs.Where(x => x.MaDH == id).ToList());
        }
        public ActionResult Guihang()
        {
            var list = db.CHITIETDONHANGs.Where(x => x.MaDH == ModelTemp.madh).ToList();
            
                foreach (var item in list)
                {
                    var sach = db.SACHes.Where(x => x.MaSach == item.MaSach).SingleOrDefault();
                    if(check(sach.SoLuongTon,item.SoLuong))
                    {
                        sach.SoLuongTon -= item.SoLuong;
                    var dh = db.DONDATHANGs.Where(x => x.MaDH == ModelTemp.madh).SingleOrDefault();
                    dh.DaThanhToan = true;
                    dh.TinhTrangGiaoHang = true;
                        db.SaveChanges();
                    }
                else
                {
                    return Content("<script> alert('Số lượng sản phẩm còn trong kho không đủ') </script>");
                }



            }
            return RedirectToAction("QuanLyDonHang", "Admin");

        }

        

        public bool check(int? slTon,int? slMua)
        {
            if (slTon > slMua)
                return true;
            return false;
        }
    }
}