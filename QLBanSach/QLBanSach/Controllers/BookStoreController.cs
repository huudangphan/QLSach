using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLBanSach.Models;
using PagedList;
using PagedList.Mvc;
    

namespace QLBanSach.Controllers
{
    public class BookStoreController : Controller
    {
        QLYBANSACHEntities db = new QLYBANSACHEntities();
        // GET: BookStore
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SachMoi(int ? page)
        {
            int pagesize = 5;
            int pagenum = (page ?? 1);
      
            var list = db.SACHes.OrderByDescending(x => x.NgayCapNhat).ToPagedList(pagenum, pagesize);
            return View(list);
        }
        public ActionResult nxb()
        {
            var list = db.NHAXUATBANs.OrderBy(x => x.TenNXB).ToList();
            return PartialView(list);
        }
        public ActionResult chude()
        {
            var list = db.CHUDEs.OrderBy(x => x.TenChuDe).ToList();
            return PartialView(list);
        }
        public ActionResult Details(int id)
        {
            var sach = (from s in db.SACHes
                        join nxb in db.NHAXUATBANs
                        on s.MaNXB equals nxb.MaNXB
                        where s.MaSach==id
                        select new ViewSanPham()
                        {
                            maSach = s.MaSach,
                            tenSach = s.TenSach,
                            moTa = s.MoTa,
                            anhBia = s.AnhBia,
                            giaBan = (double)s.GiaBan,
                            tenNXB = nxb.TenNXB
                        }).SingleOrDefault();
            return View(sach);
        }
        public ActionResult SachTheoCD(int id)
        {
            var cd = db.SACHes.Where(x => x.MaCD == id).ToList();
            return View(cd);
        }
        public ActionResult SachTheoNXB(int id)
        {
            var nxb = db.SACHes.Where(x => x.MaNXB == id).ToList();
            return View(nxb);
        }
    }
}