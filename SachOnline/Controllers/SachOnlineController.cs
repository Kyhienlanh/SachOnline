using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using SachOnline.Models;
using PagedList;
using PagedList.Mvc;
namespace SachOnline.Controllers
{
    public class SachOnlineController : Controller
    {
        // GET: SachOnline
        /// <summary>
        /// GetChuDe 
        /// </summary>
        /// <returns></returns>
        private BookOnlineEntities db=new BookOnlineEntities();

        private List<SACH> LaySachMoi(int count)
        {
            return db.SACHes.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }

      
       /* public ActionResult Index()
        {
            var listSachMoi = LaySachMoi(20);
            return View(listSachMoi);

        }*/
        public ActionResult Index( int? page)
        {
           
            int pageSize = 3;
            int pageNumber = page ?? 1;
            var listSachMoi = LaySachMoi(20);


            return View(listSachMoi.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ChuDePartial()
        {
            var listchude = db.CHUDEs;
            return PartialView(listchude);
        }
        /// <summary>
        /// GetNhaXuatBan
        /// </summary>
        /// <returns></returns>
        public ActionResult NhaXuatBanPartial()
        {
            var listnxb = db.NHAXUATBANs;
            return PartialView(listnxb);
        }
        /// <summary>
        /// GetSlider
        /// </summary>
        /// <returns></returns>
        public ActionResult SliderPartial()
        {
            return PartialView();
        }
        /// <summary>
        /// GetSachBanNhieu
        /// </summary>
        /// <returns></returns>
        /// 

        private List<SACH> LaySachbannhieu(int count)
        {
            return db.SACHes.OrderByDescending(a => a.SoLuongBan).Take(count).ToList();
        }
        public ActionResult SachBanNhieuPartial()
        {
            var listsachbannhieu = LaySachbannhieu(6);
            return PartialView(listsachbannhieu);
           
        }
        public ActionResult FooterPartial()
        {
            return PartialView();
        }
        public ActionResult NavPartial()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                ViewBag.kHACHHANG = "Đăng ký";
                ViewBag.DangXuat = "Đăng nhập";
               
            }
            else
            {
                KHACHHANG kh = (KHACHHANG)Session["TaiKhoan"];
                ViewBag.kHACHHANG = "Xin chào " + kh.TaiKhoan;
                ViewBag.DangXuat = "Đăng xuất";
            }

            return PartialView();
        }
        public ActionResult Logout()
        {
            Session["TaiKhoan"] = null; // Clear the session
            return RedirectToAction("Index", "SachOnline"); // Redirect to the home page or any other appropriate page
        }

        public ActionResult SachTheoChuDe(int iMaCD, int? page)
        {
            ViewBag.MaCD = iMaCD;
            int pageSize = 3;
            int pageNumber = page ?? 1;

            // Apply an OrderBy clause to ensure sorted input
            var sach = from s in db.SACHes where s.MaCD == iMaCD orderby s.MaSach ascending select s;

            // Continue with Skip and Take
            return View(sach.ToPagedList(pageNumber, pageSize));
        }

       /* public ActionResult SachTheoNhaXuatBan(int id)
        {
            var item = from s in db.SACHes where s.MaNXB == id select s;
            return View(item);
        }*/
        public ActionResult SachTheoNhaXuatBan(int iMaCD, int? page)
        {
            ViewBag.MaCD = iMaCD;
            int pageSize = 3;
            int pageNumber = page ?? 1;

            // Apply an OrderBy clause to ensure sorted input
            var sach = from s in db.SACHes where s.MaNXB ==iMaCD orderby s.MaSach ascending select s;

            // Continue with Skip and Take
            return View(sach.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult BookDetail (int id)
        {
            var sach = from s in db.SACHes where s.MaSach == id select s;
            return View(sach.Single());
        }
    }
}