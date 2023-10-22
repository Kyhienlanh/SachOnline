using SachOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SachOnline.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang

        private BookOnlineEntities db=new BookOnlineEntities();

 
        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            return lstGioHang;
        }



        public ActionResult ThemGioHang(int ms, string url)
        {
            List<GioHang> lstGioHang = LayGioHang();

            // Check if lstGioHang is null, and if so, initialize it
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
            }

            GioHang sp = lstGioHang.Find(n => n.iMaSach == ms);

            if (sp == null)
            {
                sp = new GioHang(ms);
                lstGioHang.Add(sp);
            }
            else
            {
                sp.iSoLuong++;
            }

            // Save the updated shopping cart back to the session
            Session["GioHang"] = lstGioHang;

            return Redirect(url);
        }

        private double TongSoLuong()
        {
            int iTongSoLuong = 0;
            
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                iTongSoLuong = lstGioHang.Sum(n => n.iSoLuong);

            }
            return iTongSoLuong;
        }

        private double TongTien()
        {
            double dTongTien = 0;
            List<GioHang> lstGiaHang = Session["GioHang"] as List<GioHang>;
            if(lstGiaHang != null)
            {
                dTongTien = lstGiaHang.Sum(n => n.dThanhTien);
            }

            return dTongTien;

        }
        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            if (lstGioHang == null)
            {
                return RedirectToAction("Index", "SachOnline");
            }

            int iTongSoLuong = 0;
            iTongSoLuong++;



            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            
            return View(lstGioHang);
        }
        

        public ActionResult Index()
        {
            return View();
        }



        public ActionResult GioHangPartial() 
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }
        public ActionResult XoaSPKhoiGioHang(int iMaSach)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.SingleOrDefault(n => n.iMaSach == iMaSach);
            if (sp != null)
            {
                lstGioHang.RemoveAll(n => n.iMaSach == iMaSach);
                if (lstGioHang.Count == 0)
                {
                    return RedirectToAction("Index", "SachOnline");
                }
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult CapNhatGioHang(int iMaSach, FormCollection f)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.SingleOrDefault(n => n.iMaSach == iMaSach);
            if (sp != null)
            {
                sp.iSoLuong = int.Parse(f["txtSoLuong"].ToString());
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult Xoagiohang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            lstGioHang.Clear();
            return RedirectToAction("Index", "SachOnline");
        }
        [HttpGet]
        public ActionResult Dathang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString()=="")
            {
                return RedirectToAction("DangNhap", "User");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "SachOnline");
            }
            List<GioHang> lstGioHang = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }

        [HttpPost]
        public ActionResult Dathang(FormCollection f)
        {
            // Check if the user is logged in
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "User");
            }

            // Check if the shopping cart is empty
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "SachOnline");
            }

            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["TaiKhoan"];
            List<GioHang> lstGioHang = LayGioHang();
            ddh.MaKH = kh.MaKH;
            ddh.NgayDat = DateTime.Now;

            // Correct date formatting and parsing
            var NgayGiao = f["NgayGiao"];
            DateTime ngayGiaoParsed;
            if (DateTime.TryParse(NgayGiao, out ngayGiaoParsed))
            {
                ddh.NgayGiao = ngayGiaoParsed;
            }
            else
            {
                // Handle invalid date input gracefully, e.g., provide an error message.
            }

            ddh.TinhTrangGiaoHang = 1;
            ddh.DaThanhToan = false;

            db.DONDATHANGs.Add(ddh);
            db.SaveChanges();

            foreach (var item in lstGioHang)
            {
                CHITIETDATHANG ctdh = new CHITIETDATHANG();
                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.MaSach = item.iMaSach;
                ctdh.SoLuong = item.iSoLuong;
                ctdh.DonGia = (decimal)item.dDonGia;
                db.CHITIETDATHANGs.Add(ctdh);
            }

            db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XacNhanDonHang", "GioHang");
        }

        public ActionResult XacNhanDonHang()
        {
            return View();
        }
       
       
         


    }
}