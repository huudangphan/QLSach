using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using QLBanSach.Models;

namespace QLBanSach.Controllers
{
    public class GioHangController : Controller
    {
        QLYBANSACHEntities db = new QLYBANSACHEntities();
        // GET: GioHang
        public ActionResult Index()
        {
            return View();
        }
        public List<GioHang> LayGioHang()
        {
            List<GioHang> listGioHang = Session["GioHang"] as List<GioHang>;
            if (listGioHang == null)
            {
                listGioHang = new List<GioHang>();
                Session["GioHang"] = listGioHang;
            }
            return listGioHang;
        }
        public ActionResult ThemGioHang(int iMaSach,string strUrl)
        {
            List<GioHang> listGioHang = LayGioHang();
            GioHang sanpham = listGioHang.Find(n => n.iMaSach == iMaSach);
            if (sanpham == null)
            {
                sanpham = new GioHang(iMaSach);
                listGioHang.Add(sanpham);
                return Redirect(strUrl);
            }
            else
            {
                sanpham.soLuong++;
                return Redirect(strUrl);
            }
        }
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> listGioHang = Session["GioHang"] as List<GioHang>;
            if (listGioHang != null)
            {
                iTongSoLuong = listGioHang.Sum(n => n.soLuong);

            }
            return iTongSoLuong;
        }
        private double TongTien()
        {
            double tongTien = 0;
            List<GioHang> listGioHang = Session["GioHang"] as List<GioHang>;
            if (listGioHang != null)
            {
                tongTien = listGioHang.Sum(n => n.thanhTien);
            }
            return tongTien;
        }
        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("SachMoi", "BookStore");
            }
            ViewBag.TongSoluong = TongSoLuong();
            ViewBag.TongThanhTien = TongTien();
            return View(lstGioHang);
        }
        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoluong = TongSoLuong();
            ViewBag.TongThanhTien = TongTien();
            return PartialView();
        }
        public ActionResult XoaGioHang(int MaSach)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sanpham = lstGioHang.SingleOrDefault(x => x.iMaSach == MaSach);
            if (sanpham != null)
            {
                lstGioHang.RemoveAll(x => x.iMaSach == MaSach);
                return RedirectToAction("GioHang");

            }
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("SachMoi", "BookStore");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaTatCaGioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            lstGioHang.Clear();
            return RedirectToAction("SachMoi", "BookStore");

        }
        public ActionResult CapNhatGioHang(int MaSach,FormCollection collection)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sanpham = lstGioHang.SingleOrDefault(x => x.iMaSach == MaSach);
            if (sanpham != null)
            {
                sanpham.soLuong = int.Parse(collection["txtSoLuong"].ToString());
            }
            return RedirectToAction("GioHang");
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["taiKhoan"] == null)
                return RedirectToAction("DangNhap", "KhachHang");
            else if (Session["GioHang"] == null)
                return RedirectToAction("Index", "BookStore");
            List<GioHang> lstGioHang = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }
        [HttpPost]
        public ActionResult DatHang(FormCollection f)
        {
            try
            {
                DONDATHANG ddh = new DONDATHANG();
                KHACHHANG kh = (KHACHHANG)Session["taiKhoan"];
                List<GioHang> gh = LayGioHang();
                var ngayGiao = String.Format("{0:MM/dd/yyyy}", f["txtNgayGiao"]);

                if (string.IsNullOrEmpty(ngayGiao))
                {
                    ViewData["loi1"] = "ngày giao không được rỗng";
                }
                else
                {
                    int thang1 = Int32.Parse(DateTime.Parse(ngayGiao).Month.ToString());
                    int ngay1 = Int32.Parse(DateTime.Parse(ngayGiao).Day.ToString());
                    int thang = Int32.Parse(DateTime.Now.Month.ToString());
                    int ngay = Int32.Parse(DateTime.Now.Day.ToString());

                    if ((thang1 < thang) || (thang1 == thang && ngay1 < ngay))
                    {
                        ViewData["loi2"] = "Ngay giao sai";
                        return View(gh);
                    }
                    else
                    {
                        ddh.MaKH = kh.MaKH;
                        ddh.NgayDH = DateTime.Now;
                        ddh.TinhTrangGiaoHang = false;
                        ddh.DaThanhToan = false;
                        ddh.NgayGiao = DateTime.Parse(ngayGiao);
                        db.DONDATHANGs.Add(ddh);
                        db.SaveChanges();
                        foreach (var item in gh)
                        {
                            CHITIETDONHANG ct = new CHITIETDONHANG();
                            ct.MaDH = ddh.MaDH;
                            GioHangExcel.maHD = ddh.MaDH;
                            ct.MaSach = item.iMaSach;
                            ct.SoLuong = item.soLuong;
                            ct.DonGia = Convert.ToDecimal(item.donGia);
                            db.CHITIETDONHANGs.Add(ct);
                            db.SaveChanges();
                        }
                        Session["GioHang"] = null;
                        return RedirectToAction("XacNhanDonHang", "GioHang");
                    }

                }
            }
            catch (Exception ex)
            {
                ViewBag.loi = "Số lượng sản phẩm còn trong kho không đủ";
                //return JavaScript(alert("Số lượng sản phẩm còn trong kho không đủ"));
                return Content("<script> alert('Số lượng sản phẩm còn trong kho không đủ') </script>");
               
            }
            return RedirectToAction("XacNhanDonHang", "GioHang");


        }
        public ActionResult XacNhanDonHang()
        {
            return View();
        }
        public IList<GioHangExcel>LayGioHangExcel()
        {
            KHACHHANG kh = (KHACHHANG)Session["taiKhoan"];
            var excelList = (from a in db.CHITIETDONHANGs
                             join b in db.DONDATHANGs
 on a.MaDH equals b.MaDH
                             join c in db.SACHes
                             on a.MaSach equals c.MaSach
                             where b.MaKH == kh.MaKH&&b.MaDH==GioHangExcel.maHD
                             select new GioHangExcel
                             {
                                 iMaSach = c.MaSach,
                                 iTenSach = c.TenSach,
                                 donGia = (double)c.GiaBan,
                                 soLuong = (int)a.SoLuong,
                                 taiKhoan = kh.TaiKhoan,
                                 hoTen = kh.HoTen,
                               
                             }).ToList();
            return excelList;


            

        }
        public ActionResult XuatExcel()
        {
            var gv = new GridView();
            gv.DataSource = this.LayGioHangExcel();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment ;filename=XuatExcell.xls");
            Response.ContentType = "aplication/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return View();


        }

    }
}